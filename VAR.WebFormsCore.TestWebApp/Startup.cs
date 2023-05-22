using Microsoft.AspNetCore.Server.Kestrel.Core;
using VAR.WebFormsCore.Code;

namespace VAR.WebFormsCore.TestWebApp
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            // If using Kestrel:
            services.Configure<KestrelServerOptions>(options =>
            {
                options.AddServerHeader = false;
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseGlobalRouterMiddleware(env);
        }
    }
}