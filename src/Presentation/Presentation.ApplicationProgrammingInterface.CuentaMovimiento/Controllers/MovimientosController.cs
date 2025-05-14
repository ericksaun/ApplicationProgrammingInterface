using Application.AppProgrammingInt.Services;
using Domain.AppProgrammingInt.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Presentation.ApplicationProgrammingInterface.CuentaMovimiento.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MovimientosController : ControllerBase
    {
        private readonly IApMovimientoServices _apMovimientoServices;
        private readonly IApCuentaServices _apCuentaServices;
        private readonly ILogger<MovimientosController> _logger;

        public MovimientosController(IApMovimientoServices apMovimientoServices, IApCuentaServices apCuentaServices, ILogger<MovimientosController> logger)
        {
            _apMovimientoServices = apMovimientoServices ?? throw new ArgumentNullException(nameof(apMovimientoServices));
            _apCuentaServices = apCuentaServices ?? throw new ArgumentNullException(nameof(apCuentaServices));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllMovimientos()
        {
            _logger.LogInformation("Inicio: Obtener todos los movimientos.");
            try
            {
                _logger.LogDebug("Llamando a GetAllMovimientosAsync.");
                var movimientos = await _apMovimientoServices.GetAllMovimientosAsync();
                _logger.LogInformation("Éxito: Se obtuvieron {Count} movimientos.", movimientos.Count());
                return Ok(movimientos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los movimientos.");
                return StatusCode(500, "Error interno al obtener los movimientos.");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetMovimientoById([FromQuery] int id)
        {
            _logger.LogInformation("Inicio: Buscar movimiento por ID {Id}.", id);
            try
            {
                _logger.LogDebug("Llamando a GetMovimientoByIdAsync con ID {Id}.", id);
                var movimiento = await _apMovimientoServices.GetMovimientoByIdAsync(id);

                if (movimiento == null)
                {
                    _logger.LogWarning("No encontrado: Movimiento con ID {Id}.", id);
                    return NotFound($"Movimiento con ID {id} no encontrado.");
                }

                _logger.LogInformation("Éxito: Movimiento con ID {Id} encontrado.", id);
                return Ok(movimiento);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al buscar el movimiento con ID {Id}.", id);
                return StatusCode(500, "Error interno al buscar el movimiento.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddMovimiento([FromBody] VMApMovimiento movimiento)
        {
            _logger.LogInformation("Inicio: Crear nuevo movimiento.");
            if (movimiento == null)
            {
                _logger.LogWarning("Validación fallida: Movimiento nulo recibido.");
                return BadRequest("El movimiento no puede ser nulo.");
            }

            try
            {
                _logger.LogDebug("Serializando y deserializando el movimiento recibido.");
                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                };

                ApMovimiento? apMovimiento = JsonConvert.DeserializeObject<ApMovimiento>(JsonConvert.SerializeObject(movimiento), settings);

                if (apMovimiento == null)
                {
                    _logger.LogWarning("Deserialización fallida: No se pudo crear el objeto ApMovimiento.");
                    return BadRequest("No se pudo procesar el movimiento.");
                }

                if (movimiento.MoIdCuentaNavigation == null || string.IsNullOrEmpty(movimiento.MoIdCuentaNavigation.CuNumeroCuenta))
                {
                    _logger.LogWarning("Validación fallida: No se proporcionó número de cuenta.");
                    return BadRequest("Debe proporcionar el número de cuenta.");
                }

                _logger.LogDebug("Buscando cuenta asociada al movimiento: {Cuenta}", movimiento.MoIdCuentaNavigation.CuNumeroCuenta);
                ApCuenta? cuenta = (await _apCuentaServices.GetCuentasByCriteriaAsync(x => x.CuNumeroCuenta == movimiento.MoIdCuentaNavigation.CuNumeroCuenta)).FirstOrDefault();

                if (cuenta == null)
                {
                    _logger.LogWarning("No se encontró la cuenta asociada al movimiento.");
                    return BadRequest("Cuenta no encontrada.");
                }

                _logger.LogDebug("Procesando el tipo de movimiento: {Tipo}", movimiento.MoTipoMovimiento);

                switch (movimiento.MoTipoMovimiento?.ToUpperInvariant())
                {
                    case "DEPOSITO":
                        _logger.LogDebug("Tipo DEPOSITO detectado. Saldo anterior: {Saldo}", cuenta.CuSaldoInicial);
                        apMovimiento.MoSaldo =  cuenta.CuSaldoInicial + (movimiento.MoValor ?? 0);
                        cuenta.CuSaldoInicial = apMovimiento.MoSaldo;
                        _logger.LogInformation("Nuevo saldo después del depósito: {Saldo}", apMovimiento.MoSaldo);
                        break;

                    case "RETIRO":
                        _logger.LogDebug("Tipo RETIRO detectado. Saldo anterior: {Saldo}", cuenta.CuSaldoInicial);
                        if ((cuenta.CuSaldoInicial - (movimiento.MoValor ?? 0)) < 0)
                        {
                            _logger.LogWarning("Fondos insuficientes para el retiro.");
                            return BadRequest("Fondos insuficientes.");
                        }
                        apMovimiento.MoSaldo = cuenta.CuSaldoInicial - (movimiento.MoValor ?? 0);
                        cuenta.CuSaldoInicial = apMovimiento.MoSaldo;
                        _logger.LogInformation("Nuevo saldo después del retiro: {Saldo}", apMovimiento.MoSaldo);
                        break;

                    default:
                        _logger.LogWarning("Tipo de movimiento no soportado: {Tipo}", movimiento.MoTipoMovimiento);
                        return BadRequest("Tipo de movimiento no soportado.");
                }

                apMovimiento.MoIdCuenta = cuenta.CuIdCuenta;
                apMovimiento.MoIdCuentaNavigation = cuenta;

                _logger.LogDebug("Llamando a AddMovimientoAsync.");
                await _apMovimientoServices.AddMovimientoAsync(apMovimiento);

                _logger.LogDebug("Actualizando saldo de la cuenta.");
                await _apCuentaServices.UpdateCuentaAsync(cuenta);

                _logger.LogInformation("Éxito: Movimiento con ID {Id} creado.", apMovimiento.MoIdMovimientos);

                return CreatedAtAction(nameof(GetMovimientoById), new { id = apMovimiento.MoIdMovimientos }, apMovimiento);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear el movimiento.");
                return StatusCode(500, "Error interno al crear el movimiento.");
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateMovimiento([FromQuery] int id, [FromBody] VMApMovimiento modificacion)
        {
            _logger.LogInformation("Inicio: Actualizar movimiento con ID {Id}.", id);
            if (modificacion == null)
            {
                _logger.LogWarning("Validación fallida: Modificación nula recibida.");
                return BadRequest("La modificación no puede ser nula.");
            }

            try
            {
                _logger.LogDebug("Llamando a GetMovimientoByIdAsync para obtener el movimiento a actualizar.");
                var movimiento = await _apMovimientoServices.GetMovimientoByIdAsync(id);

                if (movimiento == null)
                {
                    _logger.LogWarning("No encontrado: Movimiento con ID {Id} para actualización.", id);
                    return NotFound($"Movimiento con ID {id} no encontrado.");
                }

                _logger.LogDebug("Actualizando propiedades del movimiento.");
                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                };

                JsonConvert.PopulateObject(JsonConvert.SerializeObject(modificacion), movimiento, settings);

                _logger.LogDebug("Llamando a UpdateMovimientoAsync.");
                await _apMovimientoServices.UpdateMovimientoAsync(movimiento);
                _logger.LogInformation("Éxito: Movimiento con ID {Id} actualizado.", id);

                return Ok(movimiento);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el movimiento con ID {Id}.", id);
                return StatusCode(500, "Error interno al actualizar el movimiento.");
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteMovimiento([FromQuery] int id)
        {
            _logger.LogInformation("Inicio: Eliminar movimiento con ID {Id}.", id);
            try
            {
                _logger.LogDebug("Llamando a DeleteMovimientoAsync.");
                await _apMovimientoServices.DeleteMovimientoAsync(id);
                _logger.LogInformation("Éxito: Movimiento con ID {Id} eliminado.", id);
                return Ok($"Movimiento con ID {id} eliminado correctamente.");
            }
            catch (KeyNotFoundException)
            {
                _logger.LogWarning("No encontrado: Movimiento con ID {Id} para eliminación.", id);
                return NotFound($"Movimiento con ID {id} no encontrado.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el movimiento con ID {Id}.", id);
                return StatusCode(500, "Error interno al eliminar el movimiento.");
            }
        }


        
    }
}
