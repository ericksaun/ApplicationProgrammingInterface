using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.AppProgrammingInt.Models;
using Domain.AppProgrammingInt.Repositories;
using Infrastructure.AppProgrammingInt.DataBase.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.AppProgrammingInt.DataBase.AppProgrammingInt.Repositories
{
    public class ApPersonaRepository : GenericAppProgrammingIntRepository<ApPersona>, IApPersonaRepository
    {
        public ApPersonaRepository(contextAppProgrammingInt context) : base(context)
        {
        }

        public async Task<IEnumerable<ApPersona>> GetAllPersonasAsync()
        {
            return await _context.ApPersonas.Include(x=> x.ApCliente).AsNoTracking().ToListAsync();
              
        }
    }
    
}
