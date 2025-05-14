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
    public class ApCuentaRepository : GenericAppProgrammingIntRepository<ApCuenta>, IApCuentaRepository
    {
        public ApCuentaRepository(contextAppProgrammingInt context) : base(context)
        {
        }
        public async Task<IEnumerable<ApCuenta>> GetCuentasIdClienteComplete(int id)
        {
            return await  _context.ApCuenta.Where(x => x.CuIdCliente == id).Include(x=>x.ApMovimientos).ToListAsync();


        }
    }
    
}
