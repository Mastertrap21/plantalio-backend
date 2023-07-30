using System;
using System.Collections.Generic;
using Core.Model;
using Core.Service;
using Newtonsoft.Json;

namespace Core.Messaging;

public class Producer : IProducer
    {
        private readonly IMessageService _messageService;

        public Producer(IMessageService messageService)
        {
            _messageService = messageService;
        }

        public void Produce(ServiceName service, List<ProcessPayload> payloads, bool prioritized = true)
        {
            Produce(service.ToString(), payloads, prioritized);
        }

        public void Produce(string service, List<ProcessPayload> payloads, bool prioritized = true)
        {
            DoProduce(service, new Message
            {
                Payloads = payloads,
                Prioritized = prioritized
            });
        }

        public void Produce<T>(ServiceName service, T payload) where T : IFunctionPayload
        {
            Produce(service.ToString(), typeof(T).Name, payload);
        }

        public void Produce<T>(ServiceName service, string function, T payload) where T : IFunctionPayload
        {
            Produce(service.ToString(), function, payload);
        }

        public void Produce<T>(string service, T payload) where T : IFunctionPayload
        {
            Produce(service, typeof(T).Name, payload);
        }

        public void Produce<T>(string service, string function, T payload) where T : IFunctionPayload
        {
            DoProduce(service, new FunctionMessage
            {
                Function = function,
                Payload = JsonConvert.SerializeObject(payload)
            });
        }

        private void DoProduce(string service, MessageBase message)
        {
            if (message is Message { Payloads: not null } msg)
                foreach (var p in msg.Payloads)
                {
                    if (p.Id == Guid.Empty)
                    {
                        p.Id = Guid.NewGuid();
                    }
                }

            _messageService.SendMessage(service, message);
        }
        
        public void Respond(IFunctionPayload request, IFunctionPayload response)
        {
            if (!request.ResponseService.HasValue)
            {
                return;
            }

            response.RequestId = request.RequestId;
            Produce(request.ResponseService.Value, response);
        }

        public void Publish<T>(string service, T function) where T : IFunctionPayload
        {
            _messageService.SendMessage(service, new FunctionMessage
            {
                Function = typeof(T).Name,
                Payload = JsonConvert.SerializeObject(function),
            }, false);
        }
    }