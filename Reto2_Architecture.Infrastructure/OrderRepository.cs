using Reto2_Architecture.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Reto2_Architecture.Infrastructure;

public class OrderRepository : IOrderRepository 
{
    private readonly List<Order> _database = new List<Order>();

    public void SaveOrder(Order order) 
    {
        var existingOrder = _database.FirstOrDefault(o => o.Id == order.Id);
        if (existingOrder != null)
        {
            _database.Remove(existingOrder);
        }
        _database.Add(order);
    }

    public Order GetOrderById(Guid id)
    {
        return _database.FirstOrDefault(o => o.Id == id);
    }
}