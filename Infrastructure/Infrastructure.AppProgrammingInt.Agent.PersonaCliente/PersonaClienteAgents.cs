using System.Text.Json;
using Domain.AppProgrammingInt.Models;
using Infrastructure.AppProgrammingInt.Refit.Client;
using Microsoft.Extensions.Logging;


namespace Infrastructure.AppProgrammingInt.Agent.PersonaCliente
{
    public class PersonaClienteAgents : IPersonaClienteAgents
    {
        private readonly IPersonaClienteApi _personaClienteApi;
        private readonly ILogger<PersonaClienteAgents> _logger;

        public PersonaClienteAgents(IPersonaClienteApi personaClienteApi, ILogger<PersonaClienteAgents> logger)
        {
            _personaClienteApi = personaClienteApi ?? throw new ArgumentNullException(nameof(personaClienteApi));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<ApPersona> GetPersonaClientebyNameAsync(string nombre)
        {
            _logger.LogInformation("Iniciando búsqueda de persona cliente por nombre: {Nombre}", nombre);

            try
            {
                var valor = await _personaClienteApi.GetPersonaByIdName(nombre);
                string strPersona = ((JsonElement)valor).GetRawText();

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                ApPersona? persona = JsonSerializer.Deserialize<ApPersona>(strPersona, options);
                if (persona == null)
                {
                    _logger.LogWarning("No se encontró ninguna persona cliente con el nombre: {Nombre}", nombre);
                    throw new Exception($"No se encontró la persona cliente con el nombre '{nombre}'.");
                }


                _logger.LogInformation("Persona cliente encontrada correctamente: {Id} - {Nombre}", persona.PsIdPersona, persona.PsNombre);
                return persona;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la persona cliente por nombre: {Nombre}", nombre);
                throw new Exception($"Error al obtener la persona cliente por nombre '{nombre}': {ex.Message}", ex);
            }
        }
    }
}
