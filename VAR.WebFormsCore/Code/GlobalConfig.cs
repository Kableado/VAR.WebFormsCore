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
                        x is { IsAbstract: false, IsInterface: false, IsPublic: true } &&
                        iGlobalConfig.IsAssignableFrom(x)
                );
            if(foundGlobalConfig != null)
            {
                _globalConfig = ObjectActivator.CreateInstance(foundGlobalConfig) as IGlobalConfig;
            }

            return _globalConfig ??= new DefaultGlobalConfig();
        }

        // TODO: Better default global config
        private class DefaultGlobalConfig : IGlobalConfig
        {
            public string Title => string.Empty;
            public string TitleSeparator => string.Empty;
            public string Author => string.Empty;
            public string Copyright => string.Empty;
            public string DefaultHandler => string.Empty;
            public string LoginHandler => string.Empty;
            public List<string> AllowedExtensions { get; } = new();
            
            public bool IsUserAuthenticated(HttpContext context)
            {
                return false;
            }

            public void UserDeauthenticate(HttpContext context)
            {
            }
        }
    }
}