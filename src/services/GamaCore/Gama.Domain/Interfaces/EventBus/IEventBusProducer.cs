﻿namespace Gama.Domain.Interfaces.EventBus
{
    public interface IEventBusProducer
    {
        void Publish<T>(T message, string routingKey) where T : class;
    }
}
