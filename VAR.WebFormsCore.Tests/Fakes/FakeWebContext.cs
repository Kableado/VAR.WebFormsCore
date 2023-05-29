using System.Text;
using VAR.WebFormsCore.Code;

namespace VAR.WebFormsCore.Tests.Fakes;

public class FakeWebContext : IWebContext
{
    public FakeWebContext(string requestMethod = "GET")
    {
        RequestMethod = requestMethod;
    }
    
    public string RequestPath => string.Empty;

    public string RequestMethod { get; }

    public Dictionary<string, string?> RequestHeader { get; } = new();
    
    public Dictionary<string, string> RequestCookies { get; } = new();
    
    public Dictionary<string, string?> RequestQuery { get; } = new();
    
    public Dictionary<string, string?> RequestForm { get; } = new();
    
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
        ResponseStatusCode = 302;
        SetResponseHeader("location", url);
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

    public int ResponseStatusCode { get; set; } = 200;
    
    public string? ResponseContentType { get; set; }

    public Dictionary<string, string> FakeResponseHeaders { get; } = new();

    public void SetResponseHeader(string key, string value)
    {
        FakeResponseHeaders.Add(key, value);
    }

    public void PrepareCacheableResponse()
    {
        // TODO: Mark as Cacheable response
    }

    public void PrepareUncacheableResponse()
    {
        // TODO: Mark as Uncacheable response
    }
}

public struct WritePackage
{
    public string? Text { get; set; }
    public byte[]? Bin { get; set; }

    public override string ToString()
    {
        if (Text != null)
        {
            return Text;
        }

        if (Bin == null)
        {
            return string.Empty;
        }

        string text = Encoding.UTF8.GetString(Bin ?? Array.Empty<byte>());
        return text;
    }
}

public static class WritePackageExtensions
{
    public static string ToString(this List<WritePackage> list, string separator)
    {
        IEnumerable<string> listStrings = list.Select(x => x.ToString());
        string result = string.Join(separator, listStrings);
        return result;
    }
}
