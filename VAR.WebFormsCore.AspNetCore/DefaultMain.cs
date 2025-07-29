using Microsoft.AspNetCore.Builder;
using VAR.WebFormsCore.AspNetCore.Code;

namespace VAR.WebFormsCore.AspNetCore;

public static class DefaultMain
{
    public static void WebFormCoreMain(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
        WebApplication app = builder.Build();

        app.UseGlobalRouterMiddleware(builder.Environment);

        app.Run();
    }
}