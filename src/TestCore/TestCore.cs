using System;
using System.Collections.Generic;
using System.Linq;
using Core.Messaging;
using Core.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace TestCore;

public class TestCore
{
    
    protected IProducer Producer;
    protected Mock<ILogger> LoggerMock;
    protected ILogger Logger;
    
    protected IFunctionPayload LastRequest;
    protected IFunctionPayload LastResponse;
    
    [SetUp]
    public void SetupCore()
    {
        SetupLogger();
        SetupProducer();
    }

    protected static DbSet<T> GetQueryableMockDbSet<T>(List<T> sourceList) where T : class
    {
        var queryable = sourceList.AsQueryable();

        var dbSet = new Mock<DbSet<T>>();
        dbSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
        dbSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
        dbSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
        dbSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(() => queryable.GetEnumerator());
        dbSet.Setup(d => d.Add(It.IsAny<T>())).Callback<T>(sourceList.Add);

        return dbSet.Object;
    }

    private void SetupLogger()
    {
        LoggerMock = new Mock<ILogger>();
        LoggerMock.Setup(logger => logger.Log(
            It.Is<LogLevel>(logLevel => logLevel == LogLevel.Error),
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((@object, @type) => true),
            It.IsAny<Exception>(),
            It.IsAny<Func<It.IsAnyType, Exception, string>>()
        ));
        Logger = LoggerMock.Object;
    }

    private void SetupProducer()
    {
        var mock = new Mock<IProducer>();
        mock.Setup(p => p.Respond(It.IsAny<IFunctionPayload>(), It.IsAny<IFunctionPayload>()))
            .Callback((IFunctionPayload request, IFunctionPayload response) =>
            {
                LastRequest = request;
                LastResponse = response;
            });
        Producer = mock.Object;
    }
}