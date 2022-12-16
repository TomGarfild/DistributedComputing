using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Lab7_b;

public class RabbitMqServer
{
    public void Start()
    {
        var studio = new Studio();
        studio.Start(true);
        var factory = new ConnectionFactory() { HostName = "localhost" };
        using var connection = factory.CreateConnection();
        using (var channel = connection.CreateModel())
        {
            channel.QueueDeclare(queue: "studio",
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                var @params = message.Split('|');
                var command = @params[0].ToUpper();

                switch (command)
                {
                    case "ADD_ARTIST":
                        studio.AddArtist(Guid.Parse(@params[1]), @params[2]);
                        break;
                    case "ADD_ALBUM":
                        studio.AddAlbum(Guid.Parse(@params[1]), @params[2], @params[3], int.Parse(@params[4]), Guid.Parse(@params[5]));
                        break;
                    case "DELETE_ARTIST":
                        studio.DeleteArtist(Guid.Parse(@params[1]));
                        break;
                    case "DELETE_ALBUM":
                        studio.DeleteAlbum(Guid.Parse(@params[1]));
                        break;

                }
            };
            channel.BasicConsume(queue: "studio",
                autoAck: true,
                consumer: consumer);
            Console.ReadKey();
        }
    }
}