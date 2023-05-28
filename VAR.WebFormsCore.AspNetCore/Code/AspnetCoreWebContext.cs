using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using VAR.WebFormsCore.Code;

namespace VAR.WebFormsCore.AspNetCore.Code;

public class AspnetCoreWebContext : IWebContext
{
    private readonly HttpContext _context;
    
    public AspnetCoreWebContext(HttpContext context)
    {
        _context = context;
    }
    
    public string RequestPath => _context.Request.Path;

    public string RequestMethod => _context.Request.Method;


    private Dictionary<string, string?>? _requestHeader;
    
    public Dictionary<string, string?> RequestHeader
    {
        get
        {
            if (_requestHeader == null)
            {
                _requestHeader = _context.Request.Headers
                    .ToDictionary(p => p.Key, p => p.Value[0]);
            }

            return _requestHeader;
        }
    }

    private Dictionary<string, string?>? _requestQuery;
    
    public Dictionary<string, string?> RequestQuery
    {
        get
        {
            if (_requestQuery == null)
            {
                _requestQuery = _context.Request.Query
                    .ToDictionary(p => p.Key, p => p.Value[0]);
            }

            return _requestQuery;
        }
    }
    
    private Dictionary<string, string?>? _requestForm;
    
    public Dictionary<string, string?> RequestForm
    {
        get
        {
            if (_requestForm == null)
            {
                if (_context.Request.Method == "POST")
                {
                    _requestForm = _context.Request.Form
                        .ToDictionary(p => p.Key, p => p.Value[0]);
                }
                else
                {
                    _requestForm = new Dictionary<string, string?>();
                }
            }

            return _requestForm;
        }
    }

    public void ResponseWrite(string text)
    {
        _context.Response.WriteAsync(text).GetAwaiter().GetResult();
    }
    
    public void ResponseWriteBin(byte[] content)
    {
        _context.Response.Body.WriteAsync(content).GetAwaiter().GetResult();
    }

    public void ResponseFlush()
    {
        _context.Response.Body.FlushAsync().GetAwaiter().GetResult();
    }

    public void ResponseRedirect(string url)
    {
        _context.Response.Redirect(url);
    }

    public bool ResponseHasStarted => _context.Response.HasStarted;
    
    public int ResponseStatusCode
    {
        get => _context.Response.StatusCode;
        set => _context.Response.StatusCode = value;
    }
    
    public string? ResponseContentType
    {
        get => _context.Response.ContentType;
        set => _context.Response.ContentType = value;
    }

    public void SetResponseHeader(string key, string value)
    {
        _context.Response.Headers.SafeSet(key, value);
    }
    
    public void PrepareCacheableResponse()
    {
        const int secondsInDay = 86400;
        _context.Response.Headers.SafeSet("Cache-Control", $"public, max-age={secondsInDay}");
        string expireDate = DateTime.UtcNow.AddSeconds(secondsInDay)
            .ToString("ddd, dd MMM yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
        _context.Response.Headers.SafeSet("Expires", $"{expireDate} GMT");
    }

    public void PrepareUncacheableResponse()
    {
        _context.Response.Headers.SafeSet("Cache-Control", "max-age=0, no-cache, no-store");
        string expireDate = DateTime.UtcNow.AddSeconds(-1500)
            .ToString("ddd, dd MMM yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
        _context.Response.Headers.SafeSet("Expires", $"{expireDate} GMT");
    }

}