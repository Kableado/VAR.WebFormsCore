using Microsoft.AspNetCore.Http;
using VAR.Json;
using VAR.WebForms.Common.Code;

namespace VAR.WebForms.Common.Pages
{
    public class FrmEcho : IHttpHandler
    {
        #region IHttpHandler

        public bool IsReusable
        {
            get { return false; }
        }

        public void ProcessRequest(HttpContext context)
        {
            context.Response.WriteAsync("<pre><code>");
            context.Response.WriteAsync(JsonWriter.WriteObject(context.Request, indent: true));
            context.Response.WriteAsync("</code></pre>");
        }

        #endregion IHttpHandler
    }
}