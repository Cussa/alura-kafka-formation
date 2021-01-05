namespace Ecommerce.Service.HttpWebSite
{
    public class Order
    {
        public string OrderId { get; }
        public double Amount { get; }
        public string Email { get; }

        public Order(string orderId, double amount, string email)
        {
            OrderId = orderId;
            Amount = amount;
            Email = email;
        }
    }
}
