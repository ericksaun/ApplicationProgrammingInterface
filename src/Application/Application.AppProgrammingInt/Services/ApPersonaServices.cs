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
    public class ApPersonasServices : IApPersonasServices
    {

        private readonly IAppProgrammingIntUnitOfWork _unitOfWork;
        public ApPersonasServices(IAppProgrammingIntUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ApPersona> GetPersonaByIdAsync(int id)
        {
            return (await _unitOfWork.ApPersona.FindAsync(x => x.PsIdPersona.Equals(id))).FirstOrDefault()!;
        }
        public async Task<IEnumerable<ApPersona>> GetAllPersonasAsync()
        {
            return await _unitOfWork.ApPersona.GetAllPersonasAsync();
        }
        public async Task AddPersonaAsync(ApPersona Persona)
        {
            await _unitOfWork.ApPersona.AddAsync(Persona);
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task UpdatePersonaAsync(ApPersona Persona)
        {
            _unitOfWork.ApPersona.Update(Persona);
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task DeletePersonaAsync(int id)
        {
            var Persona = await GetPersonaByIdAsync(id);
            
                _unitOfWork.ApPersona.Remove(Persona);
                await _unitOfWork.SaveChangesAsync();
            
        }
        public async Task<IEnumerable<ApPersona>> GetPersonasByCriteriaAsync(Expression<Func<ApPersona, bool>> criteria)
        {
            return await _unitOfWork.ApPersona.FindAsync(criteria);
        }
        public async Task<ApPersona?> GetPersonaCompleteByNombre(string Nombre)
        {
            return await _unitOfWork.ApPersona.GetPersonaCompleteByNombre(Nombre);
        }

    }
}
