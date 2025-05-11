using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.AppProgrammingInt.Models;
using Infrastructure.AppProgrammingInt.DataBase.Context;
using Domain.AppProgrammingInt.Repositories;

namespace Infrastructure.AppProgrammingInt.DataBase.AppProgrammingInt.Repositories
{
    public class ApClienteRepository : GenericAppProgrammingIntRepository<ApCliente>, IApClienteRepository
    {
        public ApClienteRepository(contextAppProgrammingInt context) : base(context)
        {
        }
    }
    
}
