using System.Web;
using VAR.Json;

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
            context.Response.Write("<pre><code>");
            context.Response.Write(JsonWriter.WriteObject(context.Request, indent: true));
            context.Response.Write("</code></pre>");
        }

        #endregion IHttpHandler
    }
}