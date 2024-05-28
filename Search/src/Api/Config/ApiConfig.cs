using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using PetStore.Search.Data.Persistence;

namespace PetStore.Search.Api.Config
{
    public static class ApiConfig
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment env)
        {
            #region [ Context ]

            services.AddDbContext<SearchDBContext>(options =>
            {
                options.UseMongoDB(new MongoClient(configuration.GetConnectionString("DefaultConnection")), "animalsDB");
            });

            #endregion [ Context ]

            return services;
        }
    }
}