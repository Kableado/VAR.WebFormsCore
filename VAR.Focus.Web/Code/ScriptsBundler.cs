using System;
using System.Web;

namespace VAR.Focus.Web.Code
{
    public class ScriptsBundler : IHttpHandler
    {
        #region IHttpHandler

        public bool IsReusable { get { return false; } }

        public void ProcessRequest(HttpContext context)
        {
            Bundler bundler = new Bundler(context.Server.MapPath("~/Scripts/"));
            context.Response.PrepareCacheableResponse();
            bundler.WriteResponse(context.Response, "text/javascript");
        }

        #endregion IHttpHandler
    }
}