using System.Collections.Generic;

namespace VAR.WebFormsCore.Code;

public interface IWebContext
{
    string RequestPath { get; }
    string RequestMethod { get; }
    Dictionary<string, string?> RequestForm { get; }
    Dictionary<string, string?> RequestQuery { get; }
    Dictionary<string, string?> RequestHeader { get; }

    void ResponseWrite(string text);
    void ResponseWriteBin(byte[] content);
    void ResponseFlush();
    void ResponseRedirect(string url);

    bool ResponseHasStarted { get; }
    int ResponseStatusCode { get; set; }
    string? ResponseContentType { get; set; }
    void SetResponseHeader(string key, string value);
    
    void PrepareCacheableResponse();
    void PrepareUncacheableResponse();
}