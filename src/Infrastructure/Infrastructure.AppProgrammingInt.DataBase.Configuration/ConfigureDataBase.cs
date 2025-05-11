using System.Runtime.CompilerServices;
using Infrastructure.AppProgrammingInt.DataBase.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Infrastructure.AppProgrammingInt.DataBase.Configuration
{
    public static class ConfigureDataBase
    {
        public static void AddConfigureDataBase(this ServiceCollection services)
        {

            IOptionsMonitor<Appsettings> options = services.BuildServiceProvider().GetService<IOptionsMonitor<Appsettings>>()!;
            string connectionString = options.CurrentValue.ConnectionString.AppProgrammingInt;
            services.AddDbContext<contextAppProgrammingInt>(options =>
                options.UseSqlServer(connectionString,
                b => b.MigrationsAssembly(typeof(contextAppProgrammingInt).Assembly.FullName)

                )); 

        }

    }
}
