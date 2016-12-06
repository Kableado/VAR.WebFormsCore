using System.Web;
using VAR.Focus.Web.Code.JSON;

namespace VAR.Focus.Web.Pages
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
            var jsonWritter = new JsonWriter(true);
            context.Response.Write("<pre><code>");
            context.Response.Write(jsonWritter.Write(context.Request));
            context.Response.Write("</code></pre>");
        }

        #endregion
    }
}
