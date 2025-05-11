using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.AppProgrammingInt.Models;
using Domain.AppProgrammingInt.Repositories;
using Infrastructure.AppProgrammingInt.DataBase.Context;

namespace Infrastructure.AppProgrammingInt.DataBase.AppProgrammingInt.Repositories
{
    public class ApMovimientoRepository : GenericAppProgrammingIntRepository<ApMovimiento>, IApMovimientoRepository
    {
        public ApMovimientoRepository(contextAppProgrammingInt context) : base(context)
        {
        }
    }
    
}
