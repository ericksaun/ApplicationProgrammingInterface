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
    public class ApCuentaServices : IApCuentaServices
    {
        private readonly IAppProgrammingIntUnitOfWork _unitOfWork;
        private readonly IPersonaClienteAgents _personaClienteAgents;

        public ApCuentaServices(IAppProgrammingIntUnitOfWork unitOfWork, IPersonaClienteAgents personaClienteAgents)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _personaClienteAgents = personaClienteAgents ?? throw new ArgumentNullException(nameof(personaClienteAgents));
        }

        public async Task<ApCuenta> GetCuentaByIdAsync(int id)
        {
            var cuenta = (await _unitOfWork.ApCuenta.FindAsync(x => x.CuIdCuenta == id)).FirstOrDefault();
            if (cuenta == null)
            {
                throw new KeyNotFoundException($"Cuenta con ID {id} no encontrada.");
            }
            return cuenta;
        }

        public async Task<IEnumerable<ApCuenta>> GetAllCuentasAsync()
        {
            return await _unitOfWork.ApCuenta.GetAllAsync();
        }

        public async Task AddCuentaAsync(ApCuenta cuenta)
        {
            if (cuenta == null)
            {
                throw new ArgumentNullException(nameof(cuenta), "La cuenta no puede ser nula.");
            }

            await _unitOfWork.ApCuenta.AddAsync(cuenta);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateCuentaAsync(ApCuenta cuenta)
        {
            if (cuenta == null)
            {
                throw new ArgumentNullException(nameof(cuenta), "La cuenta no puede ser nula.");
            }

            _unitOfWork.ApCuenta.Update(cuenta);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteCuentaAsync(int id)
        {
            var cuenta = await GetCuentaByIdAsync(id);
            _unitOfWork.ApCuenta.Remove(cuenta);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<ApCuenta>> GetCuentasByCriteriaAsync(Expression<Func<ApCuenta, bool>> criteria)
        {
            if (criteria == null)
            {
                throw new ArgumentNullException(nameof(criteria), "El criterio no puede ser nulo.");
            }

            return await _unitOfWork.ApCuenta.FindAsync(criteria);
        }


        public async Task<ApPersona> GetPersonaClientebyNameAsync(string nombre)
        {
            ApPersona persona = await _personaClienteAgents.GetPersonaClientebyNameAsync(nombre);
            return persona;
        }

       public async Task<IEnumerable<ApCuenta>> GetCuentasIdClienteComplete(int id)
        {
            var cuentas = await _unitOfWork.ApCuenta.GetCuentasIdClienteComplete(id);
            if (cuentas == null || !cuentas.Any())
            {
                throw new KeyNotFoundException($"No se encontraron cuentas para el cliente con ID {id}.");
            }
            return cuentas;
        }
    }
}
