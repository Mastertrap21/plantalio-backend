using System;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace TestCore;

public class TestCoreTest : TestCore
{
    [Test]
    public void Always_Passes()
    {
        Assert.Pass();
    }

    protected void VerifyLogger(LogLevel verifyLogLevel, Times expectedTimes)
    {
        LoggerMock.Verify(
            logger => logger.Log(
                It.Is<LogLevel>(logLevel => logLevel == verifyLogLevel),
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((@object, @type) => true),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            expectedTimes);
    }
}