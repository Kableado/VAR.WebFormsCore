using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Web;
using VAR.Focus.Web.Code;

namespace VAR.Focus.Web
{
    public class GlobalRouter : IHttpHandler
    {
        #region Handlers

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

        #endregion

        #region IHttpHandler

        public bool IsReusable
        {
            get { return false; }
        }

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                RouteRequest(context);
            }
            catch (Exception ex)
            {
                if(ex is ThreadAbortException)
                {
                    return;
                }
                GlobalErrorHandler.HandleError(context, ex);
            }
        }

        #endregion

        #region Private methods

        private void RouteRequest(HttpContext context)
        {
            string file = Path.GetFileName(context.Request.FilePath);
            if (string.IsNullOrEmpty(file))
            {
                file = Globals.DefaultHandler;
            }

            // Pass allowed extensions requests
            string extension = Path.GetExtension(context.Request.FilePath).ToLower();
            if(Globals.AllowedExtensions.Contains(extension))
            {
                string filePath = context.Server.MapPath(string.Format("~/{0}", context.Request.FilePath));
                if (File.Exists(filePath))
                {
                    context.Response.Buffer = true;
                    context.Response.WriteFile(filePath);
                    context.Response.Flush();
                    context.Response.Close();
                    context.Response.End();
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
            context.Response.Clear();
            context.Handler = handler;
            handler.ProcessRequest(context);
        }

        #endregion
    }
}
