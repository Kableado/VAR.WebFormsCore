using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace VAR.WebFormsCore.Code
{
    public static class GlobalConfig
    {
        private static IGlobalConfig? _globalConfig;

        public static IGlobalConfig Get()
        {
            if (_globalConfig != null) { return _globalConfig; }

            Type iGlobalConfig = typeof(IGlobalConfig);
            Type? foundGlobalConfig = AppDomain.CurrentDomain
                .GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .FirstOrDefault(
                    x =>
                        x.IsAbstract == false &&
                        x.IsInterface == false &&
                        x.IsPublic &&
                        iGlobalConfig.IsAssignableFrom(x)
                );
            if(foundGlobalConfig != null)
            {
                _globalConfig = ObjectActivator.CreateInstance(foundGlobalConfig) as IGlobalConfig;
            }
            
            if(_globalConfig == null)
            {
                _globalConfig = new DefaultGlobalConfig();
            }

            return _globalConfig;
        }

        // TODO: Better default global config
        private class DefaultGlobalConfig : IGlobalConfig
        {
            public string Title { get; } = string.Empty;
            public string TitleSeparator { get; } = string.Empty;
            public string Author { get; } = string.Empty;
            public string Copyright { get; } = string.Empty;
            public string DefaultHandler { get; } = string.Empty;
            public string LoginHandler { get; } = string.Empty;
            public List<string> AllowedExtensions { get; } = new List<string>();
            
            public bool IsUserAuthenticated(HttpContext context)
            {
                return false;
            }

            public void UserUnauthenticate(HttpContext context)
            {
            }
        }
    }
}