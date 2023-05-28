using Xunit;
using VAR.WebFormsCore.Code;

namespace VAR.WebFormsCore.Tests.Code;

public class ObjectActivatorTests
{
    [Fact]
    public void CreateInstance__Object__Object()
    {
        object result = ObjectActivator.CreateInstance(typeof(object));

        Assert.IsType<object>(result);
    }

    private class TestType { }
    
    [Fact]
    public void CreateInstance__TestType__TestType()
    {
        object result = ObjectActivator.CreateInstance(typeof(TestType));

        Assert.IsType<TestType>(result);
        
        object result2 = ObjectActivator.CreateInstance(typeof(TestType));

        Assert.IsType<TestType>(result2);
    }
}