using Consumer.Data.Persistence;
using Consumer.Events.Animals;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Consumer.Consumers.Animals
{
    public interface IUpdateConsumer
    {
        void UpdatedListener();
    }

    public class UpdateConsumer : IUpdateConsumer
    {
        private readonly AnimalsContext context;
        private readonly ConnectionFactory Factory;

        public UpdateConsumer(AnimalsContext context)
        {
            this.context = context;

            Factory = new ConnectionFactory
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest"
            };
        }

        public void UpdatedListener()
        {
            var connection = Factory.CreateConnection();

            using var channel = connection.CreateModel();
            channel.QueueDeclare("animal-updated", durable: true, exclusive: false, autoDelete: true);
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, eventArgs) =>
            {
                var body = eventArgs.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var updated = System.Text.Json.JsonSerializer.Deserialize<AnimalUpdated>(message);
                if (updated != null)
                {
                    var animal = context.Animals.FirstOrDefault(x => x.Id == updated.Id);
                    if (animal != null)
                    {
                        animal.Age = updated.Age;
                        animal.Weight = updated.Weight;
                        animal.Description = updated.Description;
                        animal.Status = updated.Status;

                        context.Animals.Update(animal);
                        var success = context.SaveChanges() > 0;
                        if (success)
                            Console.WriteLine($"UPDATED pet received: {animal.Name!.ToUpper()}");
                        else
                            Console.WriteLine($"NOT updated: {animal.Name!.ToUpper()}");
                    }
                    else
                    {
                        Console.WriteLine($"NOT found: {updated.Id!.ToUpper()}");
                    }
                }
                else
                {
                    Console.WriteLine("Error reading the message");
                }
            };
            channel.BasicConsume(queue: "animal-updated", autoAck: true, consumer: consumer);
            Console.WriteLine("UPDATE Listener running...");
        }
    }
}