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
    public class ApCuentaServices : IApCuentaServices
    {
        private readonly IAppProgrammingIntUnitOfWork _unitOfWork;
        public ApCuentaServices(IAppProgrammingIntUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ApCuenta> GetCuentaByIdAsync(int id)
        {
            return (await _unitOfWork.ApCuenta.FindAsync(x => x.CuIdCuenta.Equals(id))).FirstOrDefault()!;
        }
        public async Task<IEnumerable<ApCuenta>> GetAllCuentasAsync()
        {
            return await _unitOfWork.ApCuenta.GetAllAsync();
        }
        public async Task AddCuentaAsync(ApCuenta Cuenta)
        {
            await _unitOfWork.ApCuenta.AddAsync(Cuenta);
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task UpdateCuentaAsync(ApCuenta Cuenta)
        {
            _unitOfWork.ApCuenta.Update(Cuenta);
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task DeleteCuentaAsync(int id)
        {
            var Cuenta = await GetCuentaByIdAsync(id);
            if (Cuenta != null)
            {
                _unitOfWork.ApCuenta.Remove(Cuenta);
                await _unitOfWork.SaveChangesAsync();
            }
        }
        public async Task<IEnumerable<ApCuenta>> GetCuentasByCriteriaAsync(Expression<Func<ApCuenta, bool>> criteria)
        {
            return await _unitOfWork.ApCuenta.FindAsync(criteria);
        }

    }
}
