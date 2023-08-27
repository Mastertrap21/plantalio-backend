using System;
using System.Threading;
using Core.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Logging;

namespace PlantServiceTest;

public class TestableContextTokenSource : CancellationTokenSource
{
    public new virtual void Cancel()
    {
        
    }
}