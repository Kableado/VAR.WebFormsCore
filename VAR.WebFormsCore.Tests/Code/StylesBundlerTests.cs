using Xunit;
using VAR.WebFormsCore.Code;
using VAR.WebFormsCore.Tests.Fakes;

namespace VAR.WebFormsCore.Tests.Code;

public class StylesBundlerTests
{
    [Fact]
    public void ProcessRequest__Base__IntrinsicStyles()
    {
        FakeWebContext fakeWebContext = new();
        StylesBundler stylesBundler = new();
        
        stylesBundler.ProcessRequest(fakeWebContext);
        
        Assert.Equal(200, fakeWebContext.ResponseStatusCode);
        Assert.Single(fakeWebContext.FakeWritePackages);
        
        // TODO: Verify contents of intrinsic styles
    }
}