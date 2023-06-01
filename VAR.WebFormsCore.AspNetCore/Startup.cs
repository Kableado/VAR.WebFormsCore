using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.DependencyInjection;
using VAR.WebFormsCore.AspNetCore.Code;

namespace VAR.WebFormsCore.AspNetCore;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        // If using Kestrel:
        services.Configure<KestrelServerOptions>(options => { options.AddServerHeader = false; });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env) { app.UseGlobalRouterMiddleware(env); }
}