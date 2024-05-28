using Microsoft.Extensions.Options;
using PetStore.Services.Application.Settings;
using PetStore.Services.Events;
using RabbitMQ.Client;
using System.Text;

namespace PetStore.Services.Application.Producers
{
    public interface IAnimalProducer
    {
        public void SendCreatedAnimal(AnimalCreated animalCreated);

        public void SendUpdatedAnimal(AnimalUpdated animalUpdated);
    }

    public class AnimalProducer : IAnimalProducer
    {
        private readonly RabbitSettings settings;
        private readonly ConnectionFactory factory;

        public AnimalProducer(IOptions<RabbitSettings> options)
        {
            settings = options.Value;
            factory = new()
            {
                HostName = settings.Host,
                UserName = settings.User,
                Password = settings.Password
            };
        }

        public void SendCreatedAnimal(AnimalCreated animalCreated)
        {
            var connection = factory.CreateConnection();

            using var channel = connection.CreateModel();
            channel.QueueDeclare("animal-created", durable: true, exclusive: false, autoDelete: true);
            var json = System.Text.Json.JsonSerializer.Serialize(animalCreated);
            var body = Encoding.UTF8.GetBytes(json);
            channel.BasicPublish(exchange: "", routingKey: "animal-created", body: body);
        }

        public void SendUpdatedAnimal(AnimalUpdated animalUpdated)
        {
            var connection = factory.CreateConnection();

            using var channel = connection.CreateModel();
            channel.QueueDeclare("animal-updated", durable: true, exclusive: false, autoDelete: true);
            var json = System.Text.Json.JsonSerializer.Serialize(animalUpdated);
            var body = Encoding.UTF8.GetBytes(json);
            channel.BasicPublish(exchange: "", routingKey: "animal-updated", body: body);
        }
    }
}