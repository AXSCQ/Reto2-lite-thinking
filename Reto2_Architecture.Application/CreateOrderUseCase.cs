using Reto2_Architecture.Domain;

namespace Reto2_Architecture.Application;

public class CreateOrderUseCase
{
    private readonly IOrderRepository _repository;
    private readonly EventPublisher _publisher;

    public CreateOrderUseCase(IOrderRepository repository, EventPublisher publisher)
    {
        _repository = repository;
        _publisher = publisher;
    }

    public void Execute(string productName)
    {
        Order miOrden = new Order();
        miOrden.AddProduct(productName);
        _repository.SaveOrder(miOrden);
        
        // Publicar evento
        _publisher.PublishOrderCreated(new Orders.Contracts.OrderCreated 
        { 
            OrderId = miOrden.Id, 
            CreatedAt = DateTime.UtcNow,
            Email = "customer@example.com"
        });
    }
}