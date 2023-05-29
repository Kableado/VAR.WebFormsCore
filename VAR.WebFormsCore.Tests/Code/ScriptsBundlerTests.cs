using Xunit;
using VAR.WebFormsCore.Code;
using VAR.WebFormsCore.Tests.Fakes;

namespace VAR.WebFormsCore.Tests.Code;

public class ScriptsBundlerTests
{
    [Fact]
    public void ProcessRequest__Base__IntrinsicScripts()
    {
        FakeWebContext fakeWebContext = new();
        ScriptsBundler scriptsBundler = new();
        
        scriptsBundler.ProcessRequest(fakeWebContext);
        
        Assert.Equal(200, fakeWebContext.ResponseStatusCode);
        Assert.Single(fakeWebContext.FakeWritePackages);
        
        // TODO: Verify contents of intrinsic scripts
    }
}