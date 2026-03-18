using System;
using Reto2_Architecture.Domain;

namespace Reto2_Architecture.Application;

public class OrderService
{
    private readonly IOrderRepository _repository;

    public OrderService(IOrderRepository repository)
    {
        _repository = repository;
    }

    public void PayOrder(Guid orderId)
    {
        var order = _repository.GetOrderById(orderId);
        if (order == null)
            throw new Exception("Order not found");

        order.MarkAsPaid();
        _repository.SaveOrder(order);
    }
}
