using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.AppProgrammingInt.Services;
using Microsoft.Extensions.DependencyInjection;
using Domain.AppProgrammingInt.Repositories;
using Infrastructure.AppProgrammingInt.DataBase.AppProgrammingInt.Repositories;
using Domain.AppProgrammingInt.UnitOfWork;
using Infrastructure.AppProgrammingInt.DataBase.AppProgrammingInt.UnitOfWork;
using AspNetCoreRateLimit;
using Infrastructure.AppProgrammingInt.Agent.PersonaCliente;

namespace Infrastructure.AppProgrammingInt.IOC.Dependency
{
    public static class ConfigurationIOC
    {
        public static IServiceCollection? Services { get; set; }
        public static void AddDependency(this IServiceCollection services)
        {
            Services = services;

            #region Services
            services.AddScoped<IApClienteServices, ApClienteServices>();
            services.AddScoped<IApCuentaServices, ApCuentaServices>();
            services.AddScoped<IApPersonasServices, ApPersonasServices>();
            services.AddScoped<IApMovimientoServices, ApMovimientoServices>();
            services.AddScoped<IApClienteServices, ApClienteServices>();
            #endregion Services
            #region Repsositories
            services.AddScoped<IApClienteRepository, ApClienteRepository>();
            services.AddScoped<IApCuentaRepository, ApCuentaRepository>();
            services.AddScoped<IApPersonaRepository, ApPersonaRepository>();
            services.AddScoped<IApMovimientoRepository, ApMovimientoRepository>();
            services.AddScoped<IApClienteRepository, ApClienteRepository>();
            services.AddScoped(typeof(IGenericAppProgrammingIntRepository<>), typeof(GenericAppProgrammingIntRepository<>));
            #endregion Repsositories
            #region UnitOfWork
            services.AddScoped<IAppProgrammingIntUnitOfWork, AppProgrammingIntUnitOfWork>();
            #endregion UnitOfWork
            #region Security
            services.AddTransient<IRateLimitConfiguration, RateLimitConfiguration>();
            #endregion Security
            #region Agents
            services.AddScoped<IPersonaClienteAgents, PersonaClienteAgents>();
            #endregion
           

        }


    }
}
