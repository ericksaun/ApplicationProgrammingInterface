using Domain.AppProgrammingInt.Models;
using System.Linq.Expressions;

namespace Application.AppProgrammingInt.Services
{
    public interface IApCuentaServices
    {
        Task<ApCuenta> GetCuentaByIdAsync(int id);
        Task<IEnumerable<ApCuenta>> GetAllCuentasAsync();
        Task AddCuentaAsync(ApCuenta cuenta);
        Task UpdateCuentaAsync(ApCuenta cuenta);
        Task DeleteCuentaAsync(int id);
        Task<IEnumerable<ApCuenta>> GetCuentasByCriteriaAsync(Expression<Func<ApCuenta, bool>> criteria);
        Task<ApPersona> GetPersonaClientebyNameAsync(string nombre);
        Task<IEnumerable<ApCuenta>> GetCuentasIdClienteComplete(int id);

    }
}