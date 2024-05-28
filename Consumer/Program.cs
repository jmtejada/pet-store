using Consumer;
using Consumer.Consumers.Animals;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

Console.WriteLine("Ready");
var configuration = new ConfigurationBuilder().Build();
var services = new ServiceCollection();
services.AddConsumers(configuration);

var servicesProvider = services.BuildServiceProvider();
var createConsumer = servicesProvider.GetService<ICreateConsumer>();
createConsumer?.CreatedListener();

//var updateConsumer = servicesProvider.GetService<IUpdateConsumer>();
//updateConsumer?.UpdatedListener();

Console.ReadKey();