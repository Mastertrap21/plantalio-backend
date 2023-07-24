using NUnit.Framework;

namespace TestCore;

public class TestCoreTest : TestCore
{
    [Test]
    public void Always_Passes()
    {
        Assert.Pass();
    }
}