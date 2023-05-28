using VAR.Json;
using VAR.WebFormsCore.Code;

namespace VAR.WebFormsCore.Pages;

public class FrmEcho : IHttpHandler
{
    #region IHttpHandler

    public void ProcessRequest(IWebContext context)
    {
        context.ResponseContentType = "text/html";
        context.ResponseWrite("<pre><code>");
        context.ResponseWrite($"Header:{JsonWriter.WriteObject(context.RequestHeader, indent: true)}\n");
        context.ResponseWrite($"Query:{JsonWriter.WriteObject(context.RequestQuery, indent: true)}\n");
        context.ResponseWrite($"Form:{JsonWriter.WriteObject(context.RequestForm, indent: true)}\n");
        context.ResponseWrite("</code></pre>");
    }

    #endregion IHttpHandler
}