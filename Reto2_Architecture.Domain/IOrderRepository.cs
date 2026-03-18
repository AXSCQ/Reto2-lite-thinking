using System;

namespace Reto2_Architecture.Domain;

public interface IOrderRepository
{
    void SaveOrder(Order order );
    Order GetOrderById(Guid id);
}