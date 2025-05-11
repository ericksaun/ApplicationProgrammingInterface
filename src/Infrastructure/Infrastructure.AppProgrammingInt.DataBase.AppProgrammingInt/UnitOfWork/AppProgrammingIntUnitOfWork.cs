using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.AppProgrammingInt.Repositories;
using Domain.AppProgrammingInt.UnitOfWork;
using Infrastructure.AppProgrammingInt.DataBase.Context;

namespace Infrastructure.AppProgrammingInt.DataBase.AppProgrammingInt.UnitOfWork
{
    public class AppProgrammingIntUnitOfWork: IAppProgrammingIntUnitOfWork
    {
        private readonly contextAppProgrammingInt _context;
       
        public AppProgrammingIntUnitOfWork(contextAppProgrammingInt context, IApPersonaRepository apPersonaRepository,IApMovimientoRepository apMovimientoRepository, IApCuentaRepository apCuentaRepository,IApClienteRepository apClienteRepository)
        {
            _context = context;
            ApPersona = apPersonaRepository;
            ApMovimiento = apMovimientoRepository;
            ApCuenta = apCuentaRepository;
            ApCliente = apClienteRepository;
        }
        
        public IApPersonaRepository ApPersona { get; }
        public IApMovimientoRepository ApMovimiento { get; }
        public IApCuentaRepository ApCuenta { get; }
        public IApClienteRepository ApCliente { get; }
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
