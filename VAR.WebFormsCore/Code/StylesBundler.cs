using System.Reflection;

namespace VAR.WebFormsCore.Code;

public class StylesBundler : IHttpHandler
{
    #region IHttpHandler

    public void ProcessRequest(IWebContext context)
    {
        Bundler bundler = new Bundler(
            assembly: Assembly.GetExecutingAssembly(),
            assemblyNamespace: "Styles",
            absolutePath: ServerHelpers.MapContentPath("Styles")
        );
        context.PrepareCacheableResponse();
        bundler.WriteResponse(context, "text/css");
    }

    #endregion IHttpHandler
}