using Reto2_Architecture.Domain;
using System.Collections.Generic;

namespace Reto2_Architecture.Infrastructure;

public class OrderRepository : IOrderRepository 
{
    private readonly List<Order> _database = new List<Order>();

    public void SaveOrder(Order order) 
    {
        _database.Add(order);
    }
}