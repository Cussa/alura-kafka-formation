using System;

namespace Ecommerce.Common.Models
{
    public class CorrelationId
    {
        public string Id { get; set; }

        public CorrelationId(string title)
        {
            Id = $"{title}({Guid.NewGuid()})";
        }

        public CorrelationId ContinueWith(string title)
        => new CorrelationId($"{Id}-{title}");
    }
}
