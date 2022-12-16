using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Lab8_c;

public class Client
{
    public void Start()
    {
        var factory = new ConnectionFactory() { HostName = "localhost" };
        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();
        channel.QueueDeclare(queue: "studio",
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null);

        var id = Guid.NewGuid();
        string message = $"ADD_ARTIST|{id}|EMINEM";
        var body = Encoding.UTF8.GetBytes(message);

        channel.BasicPublish(exchange: "",
            routingKey: "studio",
            basicProperties: null,
            body: body);


        var id2 = Guid.NewGuid();
        string message2 = $"ADD_ARTIST|{id2}|NF";
        var body2 = Encoding.UTF8.GetBytes(message2);

        channel.BasicPublish(exchange: "",
            routingKey: "studio",
            basicProperties: null,
            body: body2);


        string message3 = $"DELETE_ARTIST|{id2}";
        var body3 = Encoding.UTF8.GetBytes(message3);
        channel.BasicPublish(exchange: "",
            routingKey: "studio",
            basicProperties: null,
            body: body3);

        var albumId = Guid.NewGuid();
        string messageAlbum = $"ADD_ALBUM|{albumId}|Kamikaze|Rap|2018|{id}";
        var bodyAlbum = Encoding.UTF8.GetBytes(messageAlbum);

        channel.BasicPublish(exchange: "",
            routingKey: "studio",
            basicProperties: null,
            body: bodyAlbum);
        
        string messageAlbum2 = $"DELETE_ALBUM|{albumId}";
        var bodyAlbum2 = Encoding.UTF8.GetBytes(messageAlbum2);

        channel.BasicPublish(exchange: "",
            routingKey: "studio",
            basicProperties: null,
            body: bodyAlbum2);

        Console.ReadKey();
    }
}