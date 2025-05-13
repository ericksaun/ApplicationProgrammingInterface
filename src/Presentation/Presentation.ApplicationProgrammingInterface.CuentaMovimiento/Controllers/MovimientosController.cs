using Application.AppProgrammingInt.Services;
using Domain.AppProgrammingInt.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Presentation.ApplicationProgrammingInterface.CuentaMovimiento.Models;

namespace Presentation.ApplicationProgrammingInterface.CuentaMovimiento.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MovimientosController : ControllerBase
    {
        private readonly IApMovimientoServices _apMovimientoServices;
        private readonly ILogger<MovimientosController> _logger;

        public MovimientosController(IApMovimientoServices apMovimientoServices, ILogger<MovimientosController> logger)
        {
            _apMovimientoServices = apMovimientoServices ?? throw new ArgumentNullException(nameof(apMovimientoServices));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllMovimientos()
        {
            _logger.LogInformation("Iniciando la obtención de todos los movimientos.");
            var movimientos = await _apMovimientoServices.GetAllMovimientosAsync();
            _logger.LogInformation("Se obtuvieron {Count} movimientos.", movimientos.Count());
            return Ok(movimientos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMovimientoById(int id)
        {
            _logger.LogInformation("Iniciando la búsqueda del movimiento con ID {Id}.", id);
            try
            {
                var movimiento = await _apMovimientoServices.GetMovimientoByIdAsync(id);

                if (movimiento == null)
                {
                    _logger.LogWarning("Movimiento con ID {Id} no encontrado.", id);
                    return NotFound($"Movimiento con ID {id} no encontrado.");
                }

                _logger.LogInformation("Movimiento con ID {Id} encontrado.", id);
                return Ok(movimiento);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al buscar el movimiento con ID {Id}.", id);
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddMovimiento([FromBody] ApMovimiento movimiento)
        {
            if (movimiento == null)
            {
                _logger.LogWarning("Intento de agregar un movimiento nulo.");
                return BadRequest("El movimiento no puede ser nulo.");
            }

            _logger.LogInformation("Iniciando la creación de un nuevo movimiento.");
            await _apMovimientoServices.AddMovimientoAsync(movimiento);
            _logger.LogInformation("Movimiento con ID {Id} creado exitosamente.", movimiento.MoIdMovimientos);
            return CreatedAtAction(nameof(GetMovimientoById), new { id = movimiento.MoIdMovimientos }, movimiento);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMovimiento(int id, [FromBody] VMMovimiento modificacion)
        {
            if (modificacion == null)
            {
                _logger.LogWarning("Intento de actualizar un movimiento con datos nulos.");
                return BadRequest("La modificación no puede ser nula.");
            }

            _logger.LogInformation("Iniciando la actualización del movimiento con ID {Id}.", id);
            try
            {
                var movimiento = await _apMovimientoServices.GetMovimientoByIdAsync(id);

                if (movimiento == null)
                {
                    _logger.LogWarning("Movimiento con ID {Id} no encontrado para actualización.", id);
                    return NotFound($"Movimiento con ID {id} no encontrado.");
                }

                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                };

                JsonConvert.PopulateObject(JsonConvert.SerializeObject(modificacion), movimiento, settings);
                await _apMovimientoServices.UpdateMovimientoAsync(movimiento);
                _logger.LogInformation("Movimiento con ID {Id} actualizado exitosamente.", id);

                return Ok(movimiento);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el movimiento con ID {Id}.", id);
                throw;
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovimiento(int id)
        {
            _logger.LogInformation("Iniciando la eliminación del movimiento con ID {Id}.", id);
            try
            {
                await _apMovimientoServices.DeleteMovimientoAsync(id);
                _logger.LogInformation("Movimiento con ID {Id} eliminado exitosamente.", id);
                return Ok($"Movimiento con ID {id} eliminado correctamente.");
            }
            catch (KeyNotFoundException)
            {
                _logger.LogWarning("Movimiento con ID {Id} no encontrado para eliminación.", id);
                return NotFound($"Movimiento con ID {id} no encontrado.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el movimiento con ID {Id}.", id);
                throw;
            }
        }
    }
}
