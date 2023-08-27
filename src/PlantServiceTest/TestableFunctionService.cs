using System;
using System.Threading;
using Core.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Logging;

namespace PlantServiceTest;

public class TestableFunctionService : FunctionService
{
    public TestableFunctionService(ILogger log, IMessageService messageService, CancellationTokenSource? cts = null) : base(log, messageService, cts)
    {
    }
}