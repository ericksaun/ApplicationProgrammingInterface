using Application.AppProgrammingInt.Services;
using Domain.AppProgrammingInt.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Presentation.ApplicationProgrammingInterface.Models;

namespace Presentation.ApplicationProgrammingInterface.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PersonasController : ControllerBase
    {
        private readonly IApPersonasServices _apPersonaservices;
        private readonly ILogger<PersonasController> _logger;

        public PersonasController(IApPersonasServices apPersonaservices, ILogger<PersonasController> logger)
        {
            _apPersonaservices = apPersonaservices ?? throw new ArgumentNullException(nameof(apPersonaservices));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPersonas()
        {
            _logger.LogInformation("Iniciando la obtención de todas las personas.");
            var personas = await _apPersonaservices.GetAllPersonasAsync();
            _logger.LogInformation("Se obtuvieron {Count} personas.", personas.Count());
            return Ok(personas);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPersonaById(int id)
        {
            _logger.LogInformation("Iniciando la búsqueda de la persona con ID {Id}.", id);
            try
            {
                var persona = await _apPersonaservices.GetPersonasByCriteriaAsync(c => c.PsIdPersona == id);

                if (persona == null)
                {
                    _logger.LogWarning("Persona con ID {Id} no encontrada.", id);
                    return NotFound($"Persona con ID {id} no encontrada.");
                }

                _logger.LogInformation("Persona con ID {Id} encontrada.", id);
                return Ok(persona);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al buscar la persona con ID {Id}.", id);
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddPersona([FromBody] ApPersona persona)
        {
            if (persona == null)
            {
                _logger.LogWarning("Intento de agregar una persona nula.");
                return BadRequest("La persona no puede ser nula.");
            }

            _logger.LogInformation("Iniciando la creación de una nueva persona.");
            await _apPersonaservices.AddPersonaAsync(persona);
            _logger.LogInformation("Persona con ID {Id} creada exitosamente.", persona.PsIdPersona);
            return CreatedAtAction(nameof(GetPersonaById), new { id = persona.PsIdPersona }, persona);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePersona(int id, [FromBody] VMApPersona modificacion)
        {
            if (modificacion == null)
            {
                _logger.LogWarning("Intento de actualizar una persona con datos nulos.");
                return BadRequest("La modificación no puede ser nula.");
            }

            _logger.LogInformation("Iniciando la actualización de la persona con ID {Id}.", id);
            try
            {
                var persona = (await _apPersonaservices.GetPersonasByCriteriaAsync(c => c.PsIdPersona == id)).FirstOrDefault();

                if (persona == null)
                {
                    _logger.LogWarning("Persona con ID {Id} no encontrada para actualización.", id);
                    return NotFound($"Persona con ID {id} no encontrada.");
                }

                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                };

                JsonConvert.PopulateObject(JsonConvert.SerializeObject(modificacion), persona, settings);
                await _apPersonaservices.UpdatePersonaAsync(persona);
                _logger.LogInformation("Persona con ID {Id} actualizada exitosamente.", id);

                return Ok(persona);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar la persona con ID {Id}.", id);
                throw;
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePersona(int id)
        {
            _logger.LogInformation("Iniciando la eliminación de la persona con ID {Id}.", id);
            try
            {
                await _apPersonaservices.DeletePersonaAsync(id);
                _logger.LogInformation("Persona con ID {Id} eliminada exitosamente.", id);
                return Ok($"Persona con ID {id} eliminada correctamente.");
            }
            catch (KeyNotFoundException)
            {
                _logger.LogWarning("Persona con ID {Id} no encontrada para eliminación.", id);
                return NotFound($"Persona con ID {id} no encontrada.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar la persona con ID {Id}.", id);
                throw;
            }
        }
    }
}
