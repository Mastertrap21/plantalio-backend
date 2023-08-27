using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using Core.Constants;
using Core.Model;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Core.Service;

public class FunctionService : IFunctionService
    {
        private const string AnyFunction = "##ANY##";
        
        private readonly ILogger _log;
        private readonly CancellationTokenSource _cts;
        private readonly IDictionary<string, Tuple<Type, Action<IFunctionPayload>>> _listeners = new ConcurrentDictionary<string, Tuple<Type, Action<IFunctionPayload>>>();
        private readonly IMessageService _messageService;

        public FunctionService(ILogger log, IMessageService messageService, CancellationTokenSource? cts = null)
        {
            _log = log;
            _messageService = messageService;
            _cts = cts ?? new CancellationTokenSource();
        }
        
        public void Start(string? serviceName)
        {
            _messageService.Subscribe<FunctionMessage>(serviceName, HandleFunction);
        }

        private void HandleFunction(FunctionMessage functionMessage)
        {
            var function = functionMessage.Function;
            _log.LogDebug(LoggingMessageTemplates.FunctionServiceHandleFunctionConsumedFunction, function);
            
            try
            {
                if (_listeners.TryGetValue(AnyFunction, out var listener))
                {
                    _log.LogInformation(LoggingMessageTemplates.FunctionServiceHandleFunctionHandlingAnyFunction);
                    listener.Item2(new AnyFunctionPayload(functionMessage.Payload));
                    _log.LogInformation(LoggingMessageTemplates.FunctionServiceHandleFunctionProcessedAnyFunction);
                    return;
                }

                if (function != null && !_listeners.ContainsKey(function))
                {
                    _log.LogDebug(LoggingMessageTemplates.FunctionServiceHandleFunctionIgnoreNotAttachedFunction, function);
                    return;
                }

                var payload = functionMessage.Payload;
                if (function != null)
                {
                    var (type, action) = _listeners[function];

                    _log.LogInformation(LoggingMessageTemplates.FunctionServiceHandleFunctionHandlingFunctionAndPayload, function, payload);

                    if (!type.IsAssignableTo(typeof(FunctionPayload)))
                    {
                        _log.LogError(LoggingMessageTemplates.FunctionServiceHandleFunctionHandlingInvalidFunctionTypeIgnored, function);
                        return;
                    }

                    if (JsonConvert.DeserializeObject(payload ?? string.Empty, type) is not FunctionPayload functionModel)
                    {
                        _log.LogError(LoggingMessageTemplates.FunctionServiceHandleFunctionHandlingInvalidFunctionPayloadIgnored,
                            function, payload);
                        return;
                    }

                    action(functionModel);
                    _log.LogInformation(LoggingMessageTemplates.FunctionServiceHandleFunctionHandlingProcessedFunction, function);
                }
            }
            catch (Exception e)
            {
                _log.LogError(e, LoggingMessageTemplates.FunctionServiceHandleFunctionHandlingUnableHandleFunction, function);
            }
        }
        
        public void Stop()
        {
            if (_cts.IsCancellationRequested)
            {
                return;
            }

            // _messageService.Unsubscribe(); TODO
            _cts.Cancel();
        }

        public void Register<T>(Action<T> action) where T : IFunctionPayload
        {
            var func = typeof(T).Name;
            _log.LogDebug(LoggingMessageTemplates.FunctionServiceRegisterFunctionRegistering, func);
            if (_listeners.ContainsKey(func))
            {
                // TODO: exception constant
                throw new Exception($"Function {func} is already handled");
            }
            
            if (_listeners.TryAdd(func, new Tuple<Type, Action<IFunctionPayload>>(typeof(T), payload => action((T)payload)))) {
                _log.LogDebug(LoggingMessageTemplates.FunctionServiceRegisterFunctionRegistered, func);
            }
            else
            {
                _log.LogError(LoggingMessageTemplates.FunctionServiceRegisterAddToListenersFailed, func);
            }
        }

        public void RegisterAnyHandler(Action<IAnyFunctionPayload> action)
        {
            if (_listeners.ContainsKey(AnyFunction))
            {
                throw new Exception(ExceptionMessageTemplates.FunctionServiceRegisterAnyHandlerAnyFunctionAlreadyHandled);
            }

            var added = _listeners.TryAdd(AnyFunction, new Tuple<Type, Action<IFunctionPayload>>(typeof(AnyFunctionPayload), 
                payload => action((AnyFunctionPayload)payload)));
            
            if (!added)
            {
                throw new Exception(ExceptionMessageTemplates.FunctionServiceRegisterAnyHandlerAnyFunctionRegisterFailed);
            }
            
            _log.LogInformation(LoggingMessageTemplates.FunctionServiceRegisterFunctionRegistered, AnyFunction);
        }


        public void StartSubscriber(string? service)
        {
            _messageService.Subscribe<FunctionMessage>(service, HandleFunction, false);
        }
    }