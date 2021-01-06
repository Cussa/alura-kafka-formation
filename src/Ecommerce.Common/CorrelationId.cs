using System;

namespace Ecommerce.Common
{
    public class CorrelationId
    {
        public string Id { get; }

        public CorrelationId()
        {
            Id = Guid.NewGuid().ToString();
        }

        public CorrelationId(string id)
        {
            Id = id;
        }
    }
}
