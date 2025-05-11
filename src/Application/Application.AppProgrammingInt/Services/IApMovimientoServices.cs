using Domain.AppProgrammingInt.Models;
using System.Linq.Expressions;

namespace Application.AppProgrammingInt.Services
{
    public interface IApMovimientoServices
    {
        Task<ApMovimiento> GetMovimientoByIdAsync(int id);
        Task<IEnumerable<ApMovimiento>> GetAllMovimientosAsync();
        Task AddMovimientoAsync(ApMovimiento movimiento);
        Task UpdateMovimientoAsync(ApMovimiento movimiento);
        Task DeleteMovimientoAsync(int id);
        Task<IEnumerable<ApMovimiento>> GetMovimientosByCriteriaAsync(Expression<Func<ApMovimiento, bool>> criteria);
    }
}