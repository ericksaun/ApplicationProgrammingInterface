using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Refit;


namespace Infrastructure.AppProgrammingInt.Refit.Client
{
    public class RefitConfig
    {
        private const string ApiNameCuentaMovimiento = "CuentaMovimiento";
        private const string ApiNamePersonaCliente = "PersonaCliente";
        public static void Configure(IServiceCollection services)
        {
            ServiceProvider serviceProvider = services.BuildServiceProvider();
            var configuration = serviceProvider.GetRequiredService<IOptionsMonitor<Appsettings>>();
            string ApiName = configuration.CurrentValue.ApiName;
            // Configura las opciones del serializador
            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull // Ignora propiedades nulas al serializar
            };

            // Crea la configuración de Refit usando System.Text.Json
            var refitSettings = new RefitSettings
            {
                ContentSerializer = new SystemTextJsonContentSerializer(jsonOptions)
            };

            // Aplica la configuración personalizada al cliente Refit
            services.AddRefitClient<IPersonaClienteApi>(refitSettings)
                    .ConfigureHttpClient(c => c.BaseAddress = new Uri(configuration.CurrentValue.Configurations.AgentoConnectPersonaCuenta));

            services.AddRefitClient<ICuentaMovimientoApi>(refitSettings)
                    .ConfigureHttpClient(c => c.BaseAddress = new Uri(configuration.CurrentValue.Configurations.AgentoConnectCuentaMovimiento));
        }
    }
}
