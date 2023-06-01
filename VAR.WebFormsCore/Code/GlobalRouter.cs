using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace VAR.WebFormsCore.Code;

public class GlobalRouter
{
    public void RouteRequest(IWebContext context)
    {
        string path = context.RequestPath;
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
                    if (Handlers.ContainsKey(typeName) == false) { Handlers.Add(typeName, type); }
                }
            }

            return handler;
        }

        return null;
    }
}