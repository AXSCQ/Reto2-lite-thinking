using System.Text;
using System.Text.Json;
using Orders.Contracts;
using RabbitMQ.Client;

namespace Reto2_Architecture.Application;

public class EventPublisher
{
    private readonly string _hostName;

    public EventPublisher(string hostName)
    {
        _hostName = hostName;
    }

    public void PublishOrderCreated(OrderCreated orderEvent)
    {
        var factory = new ConnectionFactory { HostName = _hostName };
        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();

        channel.QueueDeclare(queue: "orders",
                             durable: false,
                             exclusive: false,
                             autoDelete: false,
                             arguments: null);

        string message = JsonSerializer.Serialize(orderEvent);
        var body = Encoding.UTF8.GetBytes(message);

        channel.BasicPublish(exchange: string.Empty,
                             routingKey: "orders",
                             basicProperties: null,
                             body: body);
    }
}
