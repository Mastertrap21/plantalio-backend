using System;
using System.Threading.Tasks;
using Core.Model;

namespace Core.Service;

public interface IMessageService
{
    void Start()
    {
    }

    void Stop()
    {
    }

    void SendMessage(string channel, MessageBase message, bool persistent = true)
        => SendMessageAsync(channel, message, persistent).Wait();

    Task SendMessageAsync(string channel, MessageBase message, bool persistent = true);

    void Subscribe<T>(string? channel, Action<T> handler, bool persistent = true) where T : MessageBase
        => SubscribeAsync(channel, handler, persistent).Wait();
    
    Task SubscribeAsync<T> (string? channel, Action<T> handler, bool persistent = true) where T : MessageBase;

    void Unsubscribe(string channel, bool persistent = true)
        => UnsubscribeAsync(channel, persistent).Wait();

    Task UnsubscribeAsync(string channel, bool persistent = true);
}