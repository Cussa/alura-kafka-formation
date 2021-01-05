using Ecommerce.Common;

namespace Ecommerce.Service.Users
{
    // using internal, as the by the concept from the course, each service should have its own Order class
    internal class Order
    {
        public string UserId { get; }
        public string OrderId { get; }
        public double Amount { get; }
        public string Email { get; } = "email";

        public Order(string userId, string orderId, double amount)
        {
            UserId = userId;
            OrderId = orderId;
            Amount = amount;
        }

        public override string ToString() => this.ToJsonString();
    }
}
