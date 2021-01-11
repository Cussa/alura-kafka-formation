using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Service.FraudDetector.Database
{
    internal class DbOrder
    {
        [Key]
        [StringLength(200)]
        public string OrderId { get; private set; }
        public bool IsFraud { get; private set; }

        internal DbOrder(Order order, bool isFraud) : this(order.OrderId, isFraud) { }

        internal DbOrder() { }

        internal DbOrder(string orderId, bool isFraud)
        {
            OrderId = orderId;
            IsFraud = isFraud;
        }
    }
}
