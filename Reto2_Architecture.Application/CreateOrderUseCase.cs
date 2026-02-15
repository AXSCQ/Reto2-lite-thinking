using Reto2_Architecture.Domain;

namespace Reto2_Architecture.Application;

public class CreateOrderUseCase
{
    private readonly IOrderRepository _repository;

    public CreateOrderUseCase(IOrderRepository repository)
    {
        _repository = repository;
    }

    public void Execute(string productName)
    {
        Order miOrden = new Order();
        miOrden.AddProduct(productName);
        _repository.SaveOrder(miOrden);
    }
}