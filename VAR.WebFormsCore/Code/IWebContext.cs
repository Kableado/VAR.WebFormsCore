using System;
using System.Collections.Generic;

namespace VAR.WebFormsCore.Code;

public interface IWebContext
{
    string RequestPath { get; }
    string RequestMethod { get; }
    Dictionary<string, string?> RequestHeader { get; }
    Dictionary<string, string> RequestCookies { get; }
    Dictionary<string, string?> RequestQuery { get; }
    Dictionary<string, string?> RequestForm { get; }

    void ResponseWrite(string text);
    void ResponseWriteBin(byte[] content);
    void ResponseFlush();
    void ResponseRedirect(string url);
    void AddResponseCookie(string cookieName, string value, DateTime? expiration = null);
    void DelResponseCookie(string cookieName);

    bool ResponseHasStarted { get; }
    int ResponseStatusCode { get; set; }
    string? ResponseContentType { get; set; }
    void SetResponseHeader(string key, string value);
    
    void PrepareCacheableResponse();
    void PrepareUncacheableResponse();
}