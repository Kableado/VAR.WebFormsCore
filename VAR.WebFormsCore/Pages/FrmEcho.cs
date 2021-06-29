using Microsoft.AspNetCore.Http;
using VAR.Json;
using VAR.WebFormsCore.Code;

namespace VAR.WebFormsCore.Pages
{
    public class FrmEcho : IHttpHandler
    {
        #region IHttpHandler

        public void ProcessRequest(HttpContext context)
        {
            context.Response.WriteAsync("<pre><code>");
            context.Response.WriteAsync(JsonWriter.WriteObject(context.Request, indent: true));
            context.Response.WriteAsync("</code></pre>");
        }

        #endregion IHttpHandler
    }
}