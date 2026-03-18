public class OrderTests { [Xunit.Fact] public void Order_Should_Have_Status_Creado_When_Created() { var order = new Reto2_Architecture.Domain.Order(); Xunit.Assert.Equal("Creado", order.Status); } }
