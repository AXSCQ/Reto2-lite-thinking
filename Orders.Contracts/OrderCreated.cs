namespace Orders.Contracts
{
    public class OrderCreated
    {
        public Guid OrderId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Email { get; set; } = string.Empty;
    }
}
