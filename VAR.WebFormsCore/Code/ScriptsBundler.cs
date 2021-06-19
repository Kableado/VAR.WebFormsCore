using Microsoft.AspNetCore.Http;

namespace VAR.WebForms.Common.Code
{
    public class ScriptsBundler : IHttpHandler
    {
        #region IHttpHandler

        public bool IsReusable { get { return false; } }

        public void ProcessRequest(HttpContext context)
        {
            // TODO: Needs replacement for context.Server.MapPath
            //Bundler bundler = new Bundler(
            //    assembly: Assembly.GetExecutingAssembly(),
            //    assemblyNamespace: "Scripts",
            //    absolutePath: context.Server.MapPath("~/Scripts/"));
            //context.Response.PrepareCacheableResponse();
            //bundler.WriteResponse(context.Response, "text/javascript");
        }

        #endregion IHttpHandler
    }
}