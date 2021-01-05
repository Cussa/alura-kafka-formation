using Ecommerce.Common;

namespace Ecommerce.Service.Users
{
    // using internal, as the by the concept from the course, each service should have its own Order class
    internal class Order
    {
        public string UserId { get; }
        public string OrderId { get; }
        public double Amount { get; }
        public string Email { get; }

        public Order(string userId, string orderId, double amount, string email)
        {
            UserId = userId;
            OrderId = orderId;
            Amount = amount;
            Email = email;
        }

        public override string ToString() => this.ToJsonString();
    }
}
