namespace Ecommerce.Service.FraudDetector
{
    // using internal, as the by the concept from the course, each service should have its own Order class
    internal class Order
    {
        public string UserId { get; }
        public string OrderId { get; }
        public double Amount { get; }

        public Order(string userId, string orderId, double amount)
        {
            UserId = userId;
            OrderId = orderId;
            Amount = amount;
        }
    }
}
