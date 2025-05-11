using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Domain.AppProgrammingInt.Models;
using Domain.AppProgrammingInt.UnitOfWork;

namespace Application.AppProgrammingInt.Services
{
    public class ApMovimientoServices : IApMovimientoServices
    {
        private readonly IAppProgrammingIntUnitOfWork _unitOfWork;
        public ApMovimientoServices(IAppProgrammingIntUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ApMovimiento> GetMovimientoByIdAsync(int id)
        {
            return (await _unitOfWork.ApMovimiento.FindAsync(x => x.MoIdMovimientos.Equals(id))).FirstOrDefault()!;
        }
        public async Task<IEnumerable<ApMovimiento>> GetAllMovimientosAsync()
        {
            return await _unitOfWork.ApMovimiento.GetAllAsync();
        }
        public async Task AddMovimientoAsync(ApMovimiento Movimiento)
        {
            await _unitOfWork.ApMovimiento.AddAsync(Movimiento);
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task UpdateMovimientoAsync(ApMovimiento Movimiento)
        {
            _unitOfWork.ApMovimiento.Update(Movimiento);
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task DeleteMovimientoAsync(int id)
        {
            var Movimiento = await GetMovimientoByIdAsync(id);
            if (Movimiento != null)
            {
                _unitOfWork.ApMovimiento.Remove(Movimiento);
                await _unitOfWork.SaveChangesAsync();
            }
        }
        public async Task<IEnumerable<ApMovimiento>> GetMovimientosByCriteriaAsync(Expression<Func<ApMovimiento, bool>> criteria)
        {
            return await _unitOfWork.ApMovimiento.FindAsync(criteria);
        }
    }
}
