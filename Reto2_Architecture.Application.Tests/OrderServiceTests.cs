using System;
using Moq;
using Xunit;
using Reto2_Architecture.Domain;
using Reto2_Architecture.Application;

namespace Reto2_Architecture.Application.Tests;

public class OrderServiceTests
{
    [Fact]
    public void PayOrder_Should_ThrowException_When_Order_Is_Already_Paid()
    {
        var mockRepo = new Mock<IOrderRepository>();
        var order = new Order();
        order.MarkAsPaid(); 
        
        mockRepo.Setup(repo => repo.GetOrderById(order.Id)).Returns(order);

        var service = new OrderService(mockRepo.Object);

        var exception = Assert.Throws<InvalidOperationException>(() => service.PayOrder(order.Id));
        Assert.Equal("No se puede pagar una orden dos veces.", exception.Message);
    }
}
