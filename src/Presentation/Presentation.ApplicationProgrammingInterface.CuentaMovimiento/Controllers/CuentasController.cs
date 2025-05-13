using Application.AppProgrammingInt.Services;
using Domain.AppProgrammingInt.Models;
using Domain.AppProgrammingInt.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Presentation.ApplicationProgrammingInterface.CuentaMovimiento.Models;

namespace Presentation.ApplicationProgrammingInterface.CuentaMovimiento.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CuentasController : ControllerBase
    {
        private readonly IApCuentaServices _cuentaServices;
        private readonly ILogger<CuentasController> _logger;

        public CuentasController(IApCuentaServices cuentaServices, ILogger<CuentasController> logger)
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

        [HttpGet("{id}")]
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
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateCuenta([FromBody] ApCuenta cuenta)
        {
            if (cuenta == null)
            {
                _logger.LogWarning("Intento de crear una cuenta nula.");
                return BadRequest("La cuenta no puede ser nula.");
            }

            _logger.LogInformation("Iniciando la creación de una nueva cuenta.");
            await _cuentaServices.AddCuentaAsync(cuenta);
            _logger.LogInformation("Cuenta con ID {Id} creada exitosamente.", cuenta.CuIdCuenta);
            return CreatedAtAction(nameof(GetCuenta), new { id = cuenta.CuIdCuenta }, cuenta);
        }

        [HttpPut("{id}")]
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
                throw;
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
                throw;
            }
        }
    }
}
