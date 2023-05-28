using VAR.WebFormsCore.Code;

namespace VAR.WebFormsCore.Tests.Fakes;

public class FakeWebContext : IWebContext
{
    public string RequestPath => string.Empty;
    
    public string RequestMethod => string.Empty;
    
    public Dictionary<string, string?> RequestHeader { get; } = new();
    
    public Dictionary<string, string> RequestCookies { get; } = new();
    
    public Dictionary<string, string?> RequestQuery { get; } = new();
    
    public Dictionary<string, string?> RequestForm { get; } = new();
    
    public struct WritePackage
    {
        public string? Text { get; set; }
        public byte[]? Bin { get; set; }
    }

    public List<WritePackage> FakeWritePackages { get; } = new();

    public void ResponseWrite(string text)
    {
        FakeWritePackages.Add(new WritePackage { Text = text, });
    }

    public void ResponseWriteBin(byte[] content)
    {
        FakeWritePackages.Add(new WritePackage { Bin = content, });
    }

    public void ResponseFlush()
    {
        // NOTE: Nothing to do
    }

    public void ResponseRedirect(string url)
    {
        throw new NotImplementedException();
    }

    public void AddResponseCookie(string cookieName, string value, DateTime? expiration = null)
    {
        throw new NotImplementedException();
    }

    public void DelResponseCookie(string cookieName)
    {
        throw new NotImplementedException();
    }

    public bool ResponseHasStarted => false;
    
    public int ResponseStatusCode { get; set; }
    
    public string? ResponseContentType { get; set; }
    
    public void SetResponseHeader(string key, string value)
    {
        throw new NotImplementedException();
    }

    public void PrepareCacheableResponse()
    {
        // TODO: Mark as Cacheable response
    }

    public void PrepareUncacheableResponse()
    {
        throw new NotImplementedException();
    }
}