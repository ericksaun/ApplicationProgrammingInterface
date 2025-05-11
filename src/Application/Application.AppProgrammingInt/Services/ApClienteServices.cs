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
    public class ApClienteServices : IApClienteServices
    {
        private readonly IAppProgrammingIntUnitOfWork _unitOfWork;
        public ApClienteServices(IAppProgrammingIntUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ApCliente> GetClienteByIdAsync(int id)
        {
            return (await _unitOfWork.ApCliente.FindAsync(x => x.ClIdCliente.Equals(id))).FirstOrDefault()!;
        }
        public async Task<IEnumerable<ApCliente>> GetAllClientesAsync()
        {
            return await _unitOfWork.ApCliente.GetAllAsync();
        }
        public async Task AddClienteAsync(ApCliente cliente)
        {
            await _unitOfWork.ApCliente.AddAsync(cliente);
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task UpdateClienteAsync(ApCliente cliente)
        {
            _unitOfWork.ApCliente.Update(cliente);
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task DeleteClienteAsync(int id)
        {
            var cliente = await GetClienteByIdAsync(id);
            if (cliente != null)
            {
                _unitOfWork.ApCliente.Remove(cliente);
                await _unitOfWork.SaveChangesAsync();
            }
        }
        public async Task<IEnumerable<ApCliente>> GetClientesByCriteriaAsync(Expression<Func<ApCliente, bool>> criteria)
        {
            return await _unitOfWork.ApCliente.FindAsync(criteria);
        }
     


    }
}
