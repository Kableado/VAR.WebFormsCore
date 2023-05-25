using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace VAR.WebFormsCore.Code
{
    public class GlobalRouterMiddleware
    {
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

            try
            {
                RouteRequest(httpContext);
                await httpContext.Response.Body.FlushAsync();
            }
            catch (Exception ex)
            {
                if (IsIgnoreException(ex) == false)
                {
                    // TODO: Implement better error logging
                    Console.WriteLine("!!!!!!!!!!");
                    Console.Write("Message: {0}\nStacktrace: {1}\n", ex.Message, ex.StackTrace);

                    GlobalErrorHandler.HandleError(httpContext, ex);
                }
            }
        }

        private static bool IsIgnoreException(Exception ex) { return ex is ThreadAbortException; }

        private void RouteRequest(HttpContext context)
        {
            string path = context.Request.Path;
            string file = Path.GetFileName(path);
            if (string.IsNullOrEmpty(file)) { file = GlobalConfig.Get().DefaultHandler; }

            // Pass allowed extensions requests
            string extension = Path.GetExtension(path).ToLower();
            if (GlobalConfig.Get().AllowedExtensions.Contains(extension))
            {
                string filePath = ServerHelpers.MapContentPath(path);
                if (File.Exists(filePath))
                {
                    StaticFileHelper.ResponseStaticFile(context, filePath);
                    return;
                }
                else
                {
                    // TODO: FrmNotFound
                    throw new Exception($"NotFound: {path}");
                }
            }

            IHttpHandler? handler = GetHandler(file);
            if (handler == null)
            {
                // TODO: FrmNotFound
                throw new Exception($"NotFound: {path}");
            }

            // Use handler
            handler.ProcessRequest(context);
        }

        private static readonly Dictionary<string, Type> Handlers = new();

        private static IHttpHandler? GetHandler(string typeName)
        {
            if (string.IsNullOrEmpty(typeName)) { return null; }

            Type? type;
            lock (Handlers)
            {
                if (Handlers.TryGetValue(typeName, out type))
                {
                    IHttpHandler? handler = ObjectActivator.CreateInstance(type) as IHttpHandler;
                    return handler;
                }
            }

            // Search type on executing assembly
            Assembly asm = Assembly.GetExecutingAssembly();
            Type[] types = asm.GetTypes();
            foreach (Type typeAux in types)
            {
                if (typeAux.FullName?.EndsWith(typeName) == true)
                {
                    type = typeAux;
                    break;
                }
            }

            // Search type on all loaded assemblies
            if (type == null)
            {
                Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
                foreach (Assembly asmAux in assemblies)
                {
                    types = asmAux.GetTypes();
                    foreach (Type typeAux in types)
                    {
                        if (typeAux.FullName?.EndsWith(typeName) != true) { continue; }

                        type = typeAux;
                        break;
                    }

                    if (type != null) { break; }
                }
            }

            // Use found type
            if (type != null)
            {
                IHttpHandler? handler = ObjectActivator.CreateInstance(type) as IHttpHandler;
                if (handler != null)
                {
                    lock (Handlers)
                    {
                        Handlers.TryAdd(typeName, type);
                    }
                }

                return handler;
            }

            return null;
        }
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
}