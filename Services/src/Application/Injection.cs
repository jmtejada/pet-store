using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetStore.Services.Application.Producers;
using PetStore.Services.Application.Settings;
using System.Reflection;

namespace PetStore.Services.Application
{
    public static class Injection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<RabbitSettings>(opts => configuration
            .GetSection("RabbitMq")
            .Bind(opts));

            #region [ Dependencies ]

            Assembly assembly = Assembly.GetExecutingAssembly();
            services.AddAutoMapper(assembly);
            services.AddValidatorsFromAssembly(assembly);
            services.AddFluentValidationAutoValidation(); // the same old MVC pipeline behavior
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(assembly));

            #endregion [ Dependencies ]

            services.AddScoped<IAnimalProducer, AnimalProducer>();

            return services;
        }
    }
}