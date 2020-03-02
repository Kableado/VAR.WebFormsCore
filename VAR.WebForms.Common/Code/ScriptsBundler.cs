using System.Reflection;
using System.Web;

namespace VAR.WebForms.Common.Code
{
    public class ScriptsBundler : IHttpHandler
    {
        #region IHttpHandler

        public bool IsReusable { get { return false; } }

        public void ProcessRequest(HttpContext context)
        {
            Bundler bundler = new Bundler(
                assembly: Assembly.GetExecutingAssembly(),
                assemblyNamespace: "Scripts",
                absolutePath: context.Server.MapPath("~/Scripts/"));
            context.Response.PrepareCacheableResponse();
            bundler.WriteResponse(context.Response, "text/javascript");
        }

        #endregion IHttpHandler
    }
}