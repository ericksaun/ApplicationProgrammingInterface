using Application.AppProgrammingInt.Services;
using Domain.AppProgrammingInt.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Presentation.ApplicationProgrammingInterface.PersonaCliente.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        private readonly IApPersonasServices _apPersonaservices;
        private readonly ILogger<ClienteController> _logger;

        public ClienteController(IApPersonasServices apPersonaservices, ILogger<ClienteController> logger)
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

        [HttpGet]
        public async Task<IActionResult> GetPersonaById([FromQuery] int id)
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
        [HttpGet]
        public async Task<IActionResult> GetPersonaByIdName([FromQuery] string Nombre)
        {
            _logger.LogInformation("Iniciando la búsqueda de la persona con Nombre {Nombre}.", Nombre);
            try
            {
                var persona = await _apPersonaservices.GetPersonaCompleteByNombre(Nombre);

                if (persona == null)
                {
                    _logger.LogWarning("Persona con Nombre {Nombre} no encontrada.", Nombre);
                    return NotFound($"Persona con Nombre {Nombre} no encontrada.");
                }

                _logger.LogInformation("Persona con Nombre {Nombre} encontrada.", Nombre);
                return Ok(persona);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al buscar la persona con Nombre {Nombre}.", Nombre);
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddPersona([FromBody] VMApPersona vmpersona)
        {
            if (vmpersona == null)
            {
                _logger.LogWarning("Intento de agregar una persona nula.");
                return BadRequest("La persona no puede ser nula.");
            }

            _logger.LogInformation("Iniciando la creación de una nueva persona.");
            try
            {
                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                };

                var persona = JsonConvert.DeserializeObject<ApPersona>(JsonConvert.SerializeObject(vmpersona), settings);
                if (persona == null)
                {
                    _logger.LogWarning("Error al deserializar la persona.");
                    return BadRequest("Error al procesar los datos de la persona.");
                }

                await _apPersonaservices.AddPersonaAsync(persona);
                _logger.LogInformation("Persona con ID {Id} creada exitosamente.", persona.PsIdPersona);
                return CreatedAtAction(nameof(GetPersonaById), new { id = persona.PsIdPersona }, persona);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear una nueva persona.");
                throw;
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdatePersona([FromQuery] int id, [FromBody] VMApPersona modificacion)
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

        [HttpDelete]
        public async Task<IActionResult> DeletePersona([FromQuery] int id)
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
