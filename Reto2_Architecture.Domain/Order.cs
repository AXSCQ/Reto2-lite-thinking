using System;
using System.Collections.Generic;

namespace Reto2_Architecture.Domain;

public class Order 
{
    public Guid Id { get; private set; }
    public DateTime CreateAt { get; private set; }
    public string Status { get; private set; }
	public List<string> Products { get; private set; }

	public Order()
    {
        Id = Guid.NewGuid();
        CreateAt = DateTime.UtcNow;
        Status = "Creado";
        Products = new List<string>();
    }

	public void AddProduct(string productName)
	{
		Products.Add(productName);
	}

    public void MarkAsPaid()
    {
        if (Status == "Pagado")
        {
            throw new InvalidOperationException("No se puede pagar una orden dos veces.");
        }
        Status = "Pagado";
    }
}
