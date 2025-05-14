using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.AppProgrammingInt.Models;

namespace Infrastructure.AppProgrammingInt.Agent.PersonaCliente
{
    public interface IPersonaClienteAgents
    {
        Task<ApPersona> GetPersonaClientebyNameAsync(string nombre);
    }
}
