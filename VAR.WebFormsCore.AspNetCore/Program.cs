using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace VAR.WebFormsCore.AspNetCore;

public static class DefaultMain
{
    public static void WebFormCoreMain(string[] args) { CreateHostBuilder(args).Build().Run(); }

    private static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
}