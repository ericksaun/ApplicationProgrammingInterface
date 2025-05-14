using System.Linq.Expressions;
using Domain.AppProgrammingInt.Models;

namespace Application.AppProgrammingInt.Services
{
    public interface IApClienteServices
    {
        Task<ApCliente> GetClienteByIdAsync(int id);
        Task<IEnumerable<ApCliente>> GetAllClientesAsync();
        Task AddClienteAsync(ApCliente cliente);
        Task UpdateClienteAsync(ApCliente cliente);
        Task DeleteClienteAsync(int id);
        Task<IEnumerable<ApCliente>> GetClientesByCriteriaAsync(Expression<Func<ApCliente, bool>> criteria);
        
    }
    
}