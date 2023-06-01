using Xunit;
using VAR.WebFormsCore.Code;
using VAR.WebFormsCore.Tests.Fakes;

namespace VAR.WebFormsCore.Tests.Code;

public class ExtensionMethodsTests
{
    #region GetRequestParameter

    [Fact]
    public void GetRequestParameter__EmptyGet__Empty()
    {
        FakeWebContext fakeWebContext = new();
        string key = "Key";

        string result = fakeWebContext.GetRequestParameter(key);

        Assert.Equal(200, fakeWebContext.ResponseStatusCode);
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void GetRequestParameter__EmptyPost__Empty()
    {
        FakeWebContext fakeWebContext = new(requestMethod: "POST");
        string key = "Key";

        string result = fakeWebContext.GetRequestParameter(key);

        Assert.Equal(200, fakeWebContext.ResponseStatusCode);
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void GetRequestParameter__QueryKeyGet__CorrectValue()
    {
        FakeWebContext fakeWebContext = new();
        string key = "Key";
        string value = "Value";
        fakeWebContext.RequestQuery.Add(key, value);

        string result = fakeWebContext.GetRequestParameter(key);

        Assert.Equal(200, fakeWebContext.ResponseStatusCode);
        Assert.Equal(value, result);
    }

    [Fact]
    public void GetRequestParameter__FormKeyPost__CorrectValue()
    {
        FakeWebContext fakeWebContext = new(requestMethod: "POST");
        string key = "Key";
        string value = "Value";
        fakeWebContext.RequestForm.Add(key, value);

        string result = fakeWebContext.GetRequestParameter(key);

        Assert.Equal(200, fakeWebContext.ResponseStatusCode);
        Assert.Equal(value, result);
    }

    [Fact]
    public void GetRequestParameter__OtherQueryKeyGet__CorrectValue()
    {
        FakeWebContext fakeWebContext = new();
        string keyInvalid = "KeyInvalid";
        string key = "Key";
        string value = "Value";
        fakeWebContext.RequestQuery.Add(keyInvalid, value);

        string result = fakeWebContext.GetRequestParameter(key);

        Assert.Equal(200, fakeWebContext.ResponseStatusCode);
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void GetRequestParameter__OtherFormKeyPost__Empty()
    {
        FakeWebContext fakeWebContext = new(requestMethod: "POST");
        string keyInvalid = "KeyInvalid";
        string key = "Key";
        string value = "Value";
        fakeWebContext.RequestForm.Add(keyInvalid, value);

        string result = fakeWebContext.GetRequestParameter(key);

        Assert.Equal(200, fakeWebContext.ResponseStatusCode);
        Assert.Equal(string.Empty, result);
    }

    #endregion GetRequestParameter

    #region ResponseObject

    [Fact]
    public void ResponseObject__EmptyObject__NullString()
    {
        FakeWebContext fakeWebContext = new();

        fakeWebContext.ResponseObject(new object());

        Assert.Equal(200, fakeWebContext.ResponseStatusCode);
        Assert.Single(fakeWebContext.FakeWritePackages);
        Assert.Equal("{ }", fakeWebContext.FakeWritePackages[0].ToString());
    }

    #endregion ResponseObject
}