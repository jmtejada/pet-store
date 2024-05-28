using Microsoft.EntityFrameworkCore;
using Persistence;

namespace PetStore.Services.Api.Config
{
    public static class ApiConfig
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment env)
        {
            #region [ Context ]

            services.AddDbContext<ServicesDBContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });

            #endregion [ Context ]

            return services;
        }
    }
}