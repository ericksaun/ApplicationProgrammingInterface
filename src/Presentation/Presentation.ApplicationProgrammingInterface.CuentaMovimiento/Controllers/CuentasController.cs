using Application.AppProgrammingInt.Services;
using Domain.AppProgrammingInt.Models;
using Domain.AppProgrammingInt.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Presentation.ApplicationProgrammingInterface.CuentaMovimiento.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CuentasController : ControllerBase
    {
        private readonly IApCuentaServices _cuentaServices;
        private readonly ILogger<CuentasController> _logger;

        public CuentasController(IApCuentaServices cuentaServices ,ILogger<CuentasController> logger)
        {
            _cuentaServices = cuentaServices ?? throw new ArgumentNullException(nameof(cuentaServices));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        public async Task<IActionResult> GetCuentas()
        {
            _logger.LogInformation("Iniciando la obtención de todas las cuentas.");
            var cuentas = await _cuentaServices.GetAllCuentasAsync();
            _logger.LogInformation("Se obtuvieron {Count} cuentas.", cuentas.Count());
            return Ok(cuentas);
        }

        [HttpGet]
        public async Task<IActionResult> GetCuenta(int id)
        {
            _logger.LogInformation("Iniciando la búsqueda de la cuenta con ID {Id}.", id);
            try
            {
                var cuenta = await _cuentaServices.GetCuentaByIdAsync(id);
                _logger.LogInformation("Cuenta con ID {Id} encontrada.", id);
                return Ok(cuenta);
            }
            catch (KeyNotFoundException)
            {
                _logger.LogWarning("Cuenta con ID {Id} no encontrada.", id);
                return NotFound($"Cuenta con ID {id} no encontrada.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al buscar la cuenta con ID {Id}.", id);
                return StatusCode(500, "Error interno del servidor.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateCuenta([FromBody] VMApCuenta cuenta)
        {
            if (cuenta == null)
            {
                _logger.LogWarning("Intento de crear una cuenta nula.");
                return BadRequest("La cuenta no puede ser nula.");
            }

            if (string.IsNullOrWhiteSpace(cuenta.NombreCliente))
            {
                _logger.LogWarning("El nombre del cliente es obligatorio para crear una cuenta.");
                return BadRequest("El nombre del cliente es obligatorio.");
            }

            try
            {
                _logger.LogInformation("Buscando persona cliente por nombre: {NombreCliente}", cuenta.NombreCliente);
                ApPersona persona = await _cuentaServices.GetPersonaClientebyNameAsync(cuenta.NombreCliente);

                if (persona.ApCliente == null)
                {
                    _logger.LogWarning("No se encontró un cliente asociado a la persona: {NombreCliente}", cuenta.NombreCliente);
                    return BadRequest("No se encontró un cliente asociado a la persona.");
                }

                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                };

                ApCuenta? apCuenta = JsonConvert.DeserializeObject<ApCuenta>(JsonConvert.SerializeObject(cuenta), settings);
                if (apCuenta == null)
                {
                    _logger.LogWarning("No se pudo deserializar la cuenta.");
                    return BadRequest("Error al procesar la cuenta.");
                }

                // Asignar cliente y número de cuenta
                apCuenta.CuIdClienteNavigation = persona.ApCliente;
                apCuenta.CuIdCliente = persona.ApCliente.ClIdCliente;
                

                await _cuentaServices.AddCuentaAsync(apCuenta);

                _logger.LogInformation("Cuenta creada exitosamente con ID {Id}.", apCuenta.CuIdCuenta);
                return CreatedAtAction(nameof(GetCuenta), new { id = apCuenta.CuIdCuenta }, apCuenta);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear la cuenta.");
                return StatusCode(500, "Error interno del servidor.");
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCuenta(int id, [FromBody] VMApCuenta modificacion)
        {
            if (modificacion == null)
            {
                _logger.LogWarning("Intento de actualizar una cuenta con datos nulos.");
                return BadRequest("La modificación no puede ser nula.");
            }

            _logger.LogInformation("Iniciando la actualización de la cuenta con ID {Id}.", id);
            try
            {
                var existingCuenta = await _cuentaServices.GetCuentaByIdAsync(id);

                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                };

                JsonConvert.PopulateObject(JsonConvert.SerializeObject(modificacion), existingCuenta, settings);
                await _cuentaServices.UpdateCuentaAsync(existingCuenta);
                _logger.LogInformation("Cuenta con ID {Id} actualizada exitosamente.", id);

                return Ok(existingCuenta);
            }
            catch (KeyNotFoundException)
            {
                _logger.LogWarning("Cuenta con ID {Id} no encontrada para actualización.", id);
                return NotFound($"Cuenta con ID {id} no encontrada.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar la cuenta con ID {Id}.", id);
                return StatusCode(500, "Error interno del servidor.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCuenta(int id)
        {
            _logger.LogInformation("Iniciando la eliminación de la cuenta con ID {Id}.", id);
            try
            {
                await _cuentaServices.DeleteCuentaAsync(id);
                _logger.LogInformation("Cuenta con ID {Id} eliminada exitosamente.", id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                _logger.LogWarning("Cuenta con ID {Id} no encontrada para eliminación.", id);
                return NotFound($"Cuenta con ID {id} no encontrada.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar la cuenta con ID {Id}.", id);
                return StatusCode(500, "Error interno del servidor.");
            }
        }
    }
}
