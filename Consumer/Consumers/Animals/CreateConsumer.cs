using AutoMapper;
using Consumer.Data.Domain;
using Consumer.Data.Persistence;
using Consumer.Events.Animals;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Consumer.Consumers.Animals
{
    public interface ICreateConsumer
    {
        void CreatedListener();
    }

    public class CreateConsumer : ICreateConsumer
    {
        private readonly AnimalsContext context;
        private readonly IMapper mapper;
        private readonly ConnectionFactory Factory;

        public CreateConsumer(AnimalsContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;

            Factory = new ConnectionFactory
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest"
            };
        }

        public void CreatedListener()
        {
            var connection = Factory.CreateConnection();

            using var channel = connection.CreateModel();
            channel.QueueDeclare("animal-created", durable: true, exclusive: false, autoDelete: true);
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, eventArgs) =>
            {
                var body = eventArgs.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var created = System.Text.Json.JsonSerializer.Deserialize<AnimalCreated>(message);
                if (created != null)
                {
                    var animal = mapper.Map<Animal>(created);
                    context.Animals.Add(animal);
                    var success = context.SaveChanges() > 0;
                    if (success)
                        Console.WriteLine($"NEW pet received: {created.Name!.ToUpper()}");
                    else
                        Console.WriteLine($"NOT created: {created.Name!.ToUpper()}");
                }
                else
                {
                    Console.WriteLine("Error reading the message");
                }
            };
            channel.BasicConsume(queue: "animal-created", autoAck: true, consumer: consumer);
            Console.WriteLine("CREATE Listener running...");
        }
    }
}