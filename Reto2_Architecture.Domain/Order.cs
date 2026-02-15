using System;
using System.Collections.Generic;

namespace Reto2_Architecture.Domain;


public class Order 
{
    public Guid Id { get; private set; }
    public DateTime CreateAt { get; private set; }
	public List<string> Products { get; private set; }

	public Order()
    {
        Id = Guid.NewGuid();
        CreateAt = DateTime.UtcNow;
        Products = new List<string>();
    }

	public void AddProduct(string productName)
	{
		Products.Add(productName);
	}

}