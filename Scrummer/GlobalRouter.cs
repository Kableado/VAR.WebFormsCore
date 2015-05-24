using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Web;
using Scrummer.Code;

namespace Scrummer
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
                IHttpHandler handler = Activator.CreateInstance(type) as IHttpHandler;
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
                IHttpHandler handler = Activator.CreateInstance(type) as IHttpHandler;
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
                string file = Path.GetFileName(context.Request.FilePath);
                if (string.IsNullOrEmpty(file))
                {
                    file = Globals.DefaultHandler;
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
            catch (Exception ex)
            {
                GlobalErrorHandler.HandleError(context, ex);
            }
        }

        #endregion
    }
}