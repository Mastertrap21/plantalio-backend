using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using Core.Model;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Core.Service;

public class FunctionService : IFunctionService
    {
        private const string AnyFunction = "##ANY##";
        
        private readonly ILogger _log;
        private readonly CancellationTokenSource _cts;
        private readonly IDictionary<string, Tuple<Type, Action<FunctionPayload>>> _listeners = new ConcurrentDictionary<string, Tuple<Type, Action<FunctionPayload>>>();
        private readonly IMessageService _messageService;

        public FunctionService(ILogger log, IMessageService messageService)
        {
            _log = log;
            _cts = new CancellationTokenSource();
            _messageService = messageService;
        }
        
        public void Start(string? serviceName)
        {
            _messageService.Subscribe<FunctionMessage>(serviceName, HandleFunction);
        }

        private void HandleFunction(FunctionMessage functionMessage)
        {
            var function = functionMessage.Function;
            _log.LogDebug("Consumed function: {Function}", function);
            
            try
            {
                if (_listeners.TryGetValue(AnyFunction, out var listener))
                {
                    _log.LogInformation("Handling ANY function");
                    listener.Item2(new AnyFunctionPayload(functionMessage.Payload));
                    _log.LogInformation("Processed ANY function");
                    return;
                }

                if (function != null && !_listeners.ContainsKey(function))
                {
                    _log.LogDebug("Function ignored due to not attached: {Function}", function);
                    return;
                }

                var payload = functionMessage.Payload;
                if (function != null)
                {
                    var (type, action) = _listeners[function];

                    _log.LogInformation("Handling function: {Function}, Payload: {Payload}", function, payload);

                    if (!type.IsAssignableTo(typeof(FunctionPayload)))
                    {
                        _log.LogError("Function ignored due to invalid function type: {Function}", function);
                        return;
                    }

                    if (JsonConvert.DeserializeObject(payload ?? string.Empty, type) is not FunctionPayload functionModel)
                    {
                        _log.LogError("Function '{Function}' ignored due to invalid function payload: {Payload}",
                            function, payload);
                        return;
                    }

                    action(functionModel);
                    _log.LogInformation("Function processed: {Function}", function);
                }
            }
            catch (Exception e)
            {
                _log.LogError(e, "Unable to handle function: {Function}", function);
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

        public void Register<T>(Action<T> action) where T : FunctionPayload
        {
            var func = typeof(T).Name;
            _log.LogDebug("Registering function: {Function}", func);
            if (_listeners.ContainsKey(func))
            {
                throw new Exception($"Function {func} is already handled");
            }
            
            if (_listeners.TryAdd(func, new Tuple<Type, Action<FunctionPayload>>(typeof(T), payload => action((T)payload)))) {
                _log.LogDebug("Function registered: {Function}", func);
            }
            else
            {
                _log.LogError("Failed to add function to listeners: {Function}", func);
            }
        }

        public void RegisterAnyHandler(Action<IAnyFunctionPayload> action)
        {
            if (_listeners.ContainsKey(AnyFunction))
            {
                throw new Exception("Any function is already handled");
            }

            var added = _listeners.TryAdd(AnyFunction, new Tuple<Type, Action<FunctionPayload>>(typeof(AnyFunctionPayload), 
                payload => action((AnyFunctionPayload)payload)));
            
            if (!added)
            {
                throw new Exception("Failed to register Any function");
            }
            
            _log.LogInformation("Function registered: {Function}", AnyFunction);
        }


        public void StartSubscriber(string? service)
        {
            _messageService.Subscribe<FunctionMessage>(service, HandleFunction, false);
        }
    }