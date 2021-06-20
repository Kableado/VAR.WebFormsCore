using System.Reflection;
using Microsoft.AspNetCore.Http;

namespace VAR.WebFormsCore.Code
{
    public class StylesBundler : IHttpHandler
    {
        #region IHttpHandler

        public void ProcessRequest(HttpContext context)
        {
            Bundler bundler = new Bundler(
                assembly: Assembly.GetExecutingAssembly(),
                assemblyNamespace: "Styles",
                absolutePath: ServerHelpers.MapContentPath("Styles"));
            context.Response.PrepareCacheableResponse();
            bundler.WriteResponse(context.Response, "text/css");
        }

        #endregion IHttpHandler
    }
}