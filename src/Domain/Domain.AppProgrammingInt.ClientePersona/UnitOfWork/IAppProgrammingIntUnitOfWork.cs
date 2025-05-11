using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.AppProgrammingInt.Repositories;

namespace Domain.AppProgrammingInt.UnitOfWork
{
    public interface IAppProgrammingIntUnitOfWork: IDisposable
    {
        IApPersonaRepository ApPersona { get; }
        IApMovimientoRepository ApMovimiento { get; }
        IApCuentaRepository ApCuenta { get; }
        IApClienteRepository ApCliente { get; }
        Task SaveChangesAsync();
    }
    
}
