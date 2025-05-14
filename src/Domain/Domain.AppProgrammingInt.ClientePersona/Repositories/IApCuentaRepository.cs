using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.AppProgrammingInt.Models;

namespace Domain.AppProgrammingInt.Repositories
{
    public interface IApCuentaRepository : IGenericAppProgrammingIntRepository<ApCuenta>
    {
        Task<IEnumerable<ApCuenta>> GetCuentasIdClienteComplete(int id);
    }
}
