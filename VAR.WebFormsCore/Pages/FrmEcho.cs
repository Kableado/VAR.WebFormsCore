using Microsoft.AspNetCore.Http;
using VAR.Json;
using VAR.WebFormsCore.Code;

namespace VAR.WebFormsCore.Pages
{
    public class FrmEcho : IHttpHandler
    {
        #region IHttpHandler

        public async void ProcessRequest(HttpContext context)
        {
            await context.Response.WriteAsync("<pre><code>");
            await context.Response.WriteAsync(JsonWriter.WriteObject(context.Request, indent: true));
            await context.Response.WriteAsync("</code></pre>");
        }

        #endregion IHttpHandler
    }
}