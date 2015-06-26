using System.Web;

namespace VAR.Focus.Web.Code
{
    public class StylesBundler : IHttpHandler
    {
        #region IHttpHandler

        public bool IsReusable { get { return false; } }

        public void ProcessRequest(HttpContext context)
        {
            Bundler bundler = new Bundler(context.Server.MapPath("~/Styles/"));
            context.Response.ContentType = "text/css";
            bundler.WriteResponse(context.Response.OutputStream);
        }

        #endregion
    }
}
