using Customer.API.Abstractions.Registrations;
using Microsoft.AspNetCore.Http.Json;
using OPS.Infrastructure;
using System.Text.Json.Serialization;

namespace Customer.API.Registrations
{
    public class SystemRegistration : IRegistration
    {
        public void RegisterServices(IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<JsonOptions>(config =>
            {
                config.SerializerOptions.PropertyNamingPolicy = null;
                config.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

            services.AddInfrastructure(configuration);
        }
    }
}
