using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Domain.AppProgrammingInt.Models;
using Domain.AppProgrammingInt.UnitOfWork;
using Infrastructure.AppProgrammingInt.Agent.PersonaCliente;

namespace Application.AppProgrammingInt.Services
{
    public class ApClienteServices : IApClienteServices
    {
        private readonly IAppProgrammingIntUnitOfWork _unitOfWork;
       
        public ApClienteServices(IAppProgrammingIntUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            
        }

        public async Task<ApCliente> GetClienteByIdAsync(int id)
        {
            var cliente = (await _unitOfWork.ApCliente.FindAsync(x => x.ClIdCliente == id)).FirstOrDefault();
            if (cliente == null)
            {
                throw new KeyNotFoundException($"Cliente con ID {id} no encontrado.");
            }
            return cliente;
        }

        public async Task<IEnumerable<ApCliente>> GetAllClientesAsync()
        {
            return await _unitOfWork.ApCliente.GetAllAsync();
        }

        public async Task AddClienteAsync(ApCliente cliente)
        {
            if (cliente == null)
            {
                throw new ArgumentNullException(nameof(cliente), "El cliente no puede ser nulo.");
            }

            await _unitOfWork.ApCliente.AddAsync(cliente);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateClienteAsync(ApCliente cliente)
        {
            if (cliente == null)
            {
                throw new ArgumentNullException(nameof(cliente), "El cliente no puede ser nulo.");
            }

            _unitOfWork.ApCliente.Update(cliente);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteClienteAsync(int id)
        {
            var cliente = await GetClienteByIdAsync(id);
            _unitOfWork.ApCliente.Remove(cliente);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<ApCliente>> GetClientesByCriteriaAsync(Expression<Func<ApCliente, bool>> criteria)
        {
            if (criteria == null)
            {
                throw new ArgumentNullException(nameof(criteria), "El criterio no puede ser nulo.");
            }

            return await _unitOfWork.ApCliente.FindAsync(criteria);
        }


    }
}
