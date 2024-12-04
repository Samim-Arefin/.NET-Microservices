﻿namespace Cart.API.Services.Interfaces
{
    public interface IEventBusService
    {
        Task PublishAsync<T>(T message, CancellationToken cancellationToken = default) where T : class;
    }
}
