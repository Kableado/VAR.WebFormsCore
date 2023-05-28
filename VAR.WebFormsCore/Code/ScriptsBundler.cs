using System.Reflection;

namespace VAR.WebFormsCore.Code;

public class ScriptsBundler : IHttpHandler
{
    #region IHttpHandler

    public void ProcessRequest(IWebContext context)
    {
        Bundler bundler = new Bundler(
            assembly: Assembly.GetExecutingAssembly(),
            assemblyNamespace: "Scripts",
            absolutePath: ServerHelpers.MapContentPath("Scripts")
        );
        context.PrepareCacheableResponse();
        bundler.WriteResponse(context, "text/javascript");
    }

    #endregion IHttpHandler
}