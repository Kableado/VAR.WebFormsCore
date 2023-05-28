using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using VAR.WebFormsCore.Code;

namespace VAR.WebFormsCore.AspNetCore.Code;

public class GlobalRouterMiddleware
{
    private readonly GlobalRouter _globalRouter = new();
        
    public GlobalRouterMiddleware(RequestDelegate next, IWebHostEnvironment env)
    {
        ServerHelpers.SetContentRoot(env.ContentRootPath);
    }

    public async Task Invoke(HttpContext httpContext)
    {
        httpContext.Response.Headers.SafeDel("Server");
        httpContext.Response.Headers.SafeDel("X-Powered-By");
        httpContext.Response.Headers.SafeSet("X-Content-Type-Options", "nosniff");
        httpContext.Response.Headers.SafeSet("X-Frame-Options", "SAMEORIGIN");
        httpContext.Response.Headers.SafeSet("X-XSS-Protection", "1; mode=block");

        IWebContext webContext = new AspnetCoreWebContext(httpContext);
            
        try
        {
            _globalRouter.RouteRequest(webContext);
            await httpContext.Response.Body.FlushAsync();
        }
        catch (Exception ex)
        {
            if (IsIgnoreException(ex) == false)
            {
                // TODO: Implement better error logging
                Console.WriteLine("!!!!!!!!!!");
                Console.Write("Message: {0}\nStacktrace: {1}\n", ex.Message, ex.StackTrace);

                GlobalErrorHandler.HandleError(webContext, ex);
            }
        }
    }

    private static bool IsIgnoreException(Exception ex) { return ex is ThreadAbortException; }

}

public static class GlobalRouterMiddlewareExtensions
{
    public static IApplicationBuilder UseGlobalRouterMiddleware(
        this IApplicationBuilder builder,
        IWebHostEnvironment env
    )
    {
        return builder.UseMiddleware<GlobalRouterMiddleware>(env);
    }
}