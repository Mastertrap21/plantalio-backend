using System.Collections.Generic;
using Core.Model;

namespace Core.Messaging;

public interface IProducer
{
    void Produce(ServiceName service, List<ProcessPayload> payload, bool prioritized = true);
    void Produce(string service, List<ProcessPayload> payload, bool prioritized = true);
    void Produce<T>(ServiceName service, T payload) where T : IFunctionPayload;
    void Produce<T>(ServiceName service, string command, T payload) where T : IFunctionPayload;
    void Produce<T>(string service, T payload) where T : IFunctionPayload;
    void Produce<T>(string service, string command, T payload) where T : IFunctionPayload;
    void Respond(IFunctionPayload request, IFunctionPayload response);
    void Publish<T>(string service, T command) where T : IFunctionPayload;
}