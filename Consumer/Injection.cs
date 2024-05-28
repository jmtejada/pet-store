using Consumer.Consumers.Animals;
using Consumer.Data.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using System.Reflection;

namespace Consumer
{
    public static class Injection
    {
        public static void AddConsumers(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddDbContext<AnimalsContext>(options =>
            {
                options.UseMongoDB(new MongoClient("mongodb://localhost"), "animalsDB");
            });

            services.AddTransient<ICreateConsumer, CreateConsumer>();
            services.AddTransient<IUpdateConsumer, UpdateConsumer>();
        }
    }
}