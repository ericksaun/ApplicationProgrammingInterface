using System.Linq.Expressions;
using Domain.AppProgrammingInt.Models;

namespace Application.AppProgrammingInt.Services
{
    public interface IApPersonasServices
    {
        Task<ApPersona> GetPersonaByIdAsync(int id);
        Task<IEnumerable<ApPersona>> GetAllPersonasAsync();
        Task AddPersonaAsync(ApPersona persona);
        Task UpdatePersonaAsync(ApPersona persona);
        Task DeletePersonaAsync(int id);
        Task<IEnumerable<ApPersona>> GetPersonasByCriteriaAsync(Expression<Func<ApPersona, bool>> criteria);
    }
}