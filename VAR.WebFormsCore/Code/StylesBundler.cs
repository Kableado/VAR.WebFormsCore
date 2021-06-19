using Microsoft.AspNetCore.Http;

namespace VAR.WebForms.Common.Code
{
    public class StylesBundler : IHttpHandler
    {
        #region IHttpHandler

        public bool IsReusable { get { return false; } }

        public void ProcessRequest(HttpContext context)
        {
            // TODO: Needs replacement for context.Server.MapPath
            //Bundler bundler = new Bundler(
            //    assembly: Assembly.GetExecutingAssembly(),
            //    assemblyNamespace: "Styles",
            //    absolutePath: context.Server.MapPath("~/Styles/"));
            //context.Response.PrepareCacheableResponse();
            //bundler.WriteResponse(context.Response, "text/css");
        }

        #endregion IHttpHandler
    }
}