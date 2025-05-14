using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.AppProgrammingInt.Models;

namespace Domain.AppProgrammingInt.Repositories
{
    public interface IApPersonaRepository : IGenericAppProgrammingIntRepository<ApPersona>
    {
        Task<IEnumerable<ApPersona>> GetAllPersonasAsync();
        Task<ApPersona?> GetPersonaCompleteByNombre(string Nombre);
    }
}
