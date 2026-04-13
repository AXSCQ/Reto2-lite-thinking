using System.Text;
using System.Text.Json;
using Orders.Contracts;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Orders.Worker;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IConfiguration _configuration;
    private IConnection? _connection;
    private IModel? _channel;

    public Worker(ILogger<Worker> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
        InitRabbitMQ();
    }

    private void InitRabbitMQ()
    {
        var mqHost = _configuration["RabbitMQHost"] ?? "localhost";
        var factory = new ConnectionFactory { HostName = mqHost };
        
        try 
        {
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.QueueDeclare(queue: "orders",
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);
                                 
            _logger.LogInformation("Conectado a RabbitMQ en {HostName}", mqHost);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Could not connect to RabbitMQ");
        }
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        stoppingToken.ThrowIfCancellationRequested();

        if (_channel == null) 
        {
            _logger.LogWarning("Canal de RabbitMQ no disponible.");
            return Task.CompletedTask;
        }

        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += (ch, ea) =>
        {
            var content = Encoding.UTF8.GetString(ea.Body.ToArray());
            var orderCreated = JsonSerializer.Deserialize<OrderCreated>(content);

            if (orderCreated != null)
            {
                _logger.LogInformation($"Orden {orderCreated.OrderId} recibida y procesada exitosamente en el Worker. Email asociado: {orderCreated.Email}");
            }

            _channel.BasicAck(ea.DeliveryTag, multiple: false);
        };

        _channel.BasicConsume("orders", false, consumer);

        return Task.CompletedTask;
    }

    public override void Dispose()
    {
        _channel?.Dispose();
        _connection?.Dispose();
        base.Dispose();
    }
}
