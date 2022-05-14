using System.Reflection;
using Microsoft.AspNetCore.Http;

namespace VAR.WebFormsCore.Code
{
    public class ScriptsBundler : IHttpHandler
    {
        #region IHttpHandler

        public void ProcessRequest(HttpContext context)
        {
            Bundler bundler = new Bundler(
                assembly: Assembly.GetExecutingAssembly(),
                assemblyNamespace: "Scripts",
                absolutePath: ServerHelpers.MapContentPath("Scripts")
            );
            context.Response.PrepareCacheableResponse();
            bundler.WriteResponse(context.Response, "text/javascript");
        }

        #endregion IHttpHandler
    }
}