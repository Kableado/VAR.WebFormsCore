using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using VAR.WebForms.Common.Code;

namespace VAR.Focus.WebApp
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class GlobalRouterMiddleware
    {
        private readonly RequestDelegate _next;

        public GlobalRouterMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext httpContext)
        {


            httpContext.Response.Headers.Remove("Server");
            httpContext.Response.Headers.Remove("X-Powered-By");
            httpContext.Response.Headers.Add("X-Content-Type-Options", "nosniff");
            httpContext.Response.Headers.Add("X-Frame-Options", "SAMEORIGIN");
            httpContext.Response.Headers.Add("X-XSS-Protection", "1; mode=block");

            try
            {
                RouteRequest(httpContext);
            }
            catch (Exception ex)
            {
                if (ex is ThreadAbortException)
                {
                    return null;
                }
                GlobalErrorHandler.HandleError(httpContext, ex);
            }

            //httpContext.Response.WriteAsync("Hello World!");

            //return _next(httpContext);

            return null;
        }


        private void RouteRequest(HttpContext context)
        {
            // TODO: need a replacement for context.Request.FilePath, using context.Request.Path for now
            string path = context.Request.Path;
            string file = Path.GetFileName(path);
            if (string.IsNullOrEmpty(file))
            {
                file = GlobalConfig.Get().DefaultHandler;
            }

            // Pass allowed extensions requests
            string extension = Path.GetExtension(path).ToLower();
            if (GlobalConfig.Get().AllowedExtensions.Contains(extension))
            {
                // TODO: need a replacement for context.Request.PhysicalPath, using context.Request.Path for now
                string filePath = context.Request.Path;
                if (File.Exists(filePath))
                {
                    StaticFileHelper.ResponseStaticFile(context, filePath);
                    return;
                }
            }

            IHttpHandler handler = GetHandler(file);
            if (handler == null)
            {
                // TODO: FrmNotFound
                throw new Exception("NotFound");
            }

            // Use handler
            //context.Response.Clear();
            //context.Handler = handler;
            handler.ProcessRequest(context);
        }

        private static Dictionary<string, Type> _handlers = new Dictionary<string, Type>();

        private static IHttpHandler GetHandler(string typeName)
        {
            if (string.IsNullOrEmpty(typeName)) { return null; }
            Type type = null;
            if (_handlers.ContainsKey(typeName))
            {
                type = _handlers[typeName];
                IHttpHandler handler = ObjectActivator.CreateInstance(type) as IHttpHandler;
                return handler;
            }

            // Search type on executing assembly
            Type[] types;
            Assembly asm = Assembly.GetExecutingAssembly();
            types = asm.GetTypes();
            foreach (Type typeAux in types)
            {
                if (typeAux.FullName.EndsWith(typeName))
                {
                    type = typeAux;
                    break;
                }
            }

            // Search type on all loaded assemblies
            if (type == null)
            {
                Assembly[] asms = AppDomain.CurrentDomain.GetAssemblies();
                foreach (Assembly asmAux in asms)
                {
                    types = asmAux.GetTypes();
                    foreach (Type typeAux in types)
                    {
                        if (typeAux.FullName.EndsWith(typeName))
                        {
                            type = typeAux;
                            break;
                        }
                    }
                    if (type != null) { break; }
                }
            }

            // Use found type
            if (type != null)
            {
                IHttpHandler handler = ObjectActivator.CreateInstance(type) as IHttpHandler;
                if (handler != null)
                {
                    lock (_handlers)
                    {
                        if (_handlers.ContainsKey(typeName) == false)
                        {
                            _handlers.Add(typeName, type);
                        }
                    }
                }
                return handler;
            }

            return null;
        }

    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class GlobalRouterMiddlewareExtensions
    {
        public static IApplicationBuilder UseGlobalRouterMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<GlobalRouterMiddleware>();
        }
    }
}
