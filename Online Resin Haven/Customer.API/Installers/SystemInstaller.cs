using Customer.API.Abstraction.Installers;
using Microsoft.AspNetCore.Http.Json;
using ORH.Infrastructure;
using System.Text.Json.Serialization;

namespace Customer.API.Installers
{
    public class SystemInstaller : IInstaller
    {
        public void RegisterService(IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<JsonOptions>(options =>
            {
                options.SerializerOptions.PropertyNamingPolicy = null;
                options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

            services.AddInfrastructure(configuration);
        }
    }
}
