using System;
using System.Web;

namespace VAR.WebForms.Common
{
    public class GlobalModule : IHttpModule
    {
        public void Dispose() { }

        public void Init(HttpApplication context)
        {
            context.PreSendRequestHeaders += Context_PreSendRequestHeaders;
        }

        private void Context_PreSendRequestHeaders(object sender, EventArgs e)
        {
            HttpContext ctx = HttpContext.Current;
            if (ctx == null) { return; }

            ctx.Response.Headers.Remove("Server");
            ctx.Response.Headers.Remove("X-Powered-By");
            ctx.Response.Headers.Add("X-Content-Type-Options", "nosniff");
            ctx.Response.Headers.Add("X-Frame-Options", "SAMEORIGIN");
            ctx.Response.Headers.Add("X-XSS-Protection", "1; mode=block");
        }
    }
}