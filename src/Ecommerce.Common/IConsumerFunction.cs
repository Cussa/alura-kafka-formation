﻿using Confluent.Kafka;

namespace Ecommerce.Common
{
    public interface IConsumerFunction<T>
    {
        void Consume(ConsumeResult<string, T> record);
    }
}