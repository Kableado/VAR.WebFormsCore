using System;
using System.Linq;

namespace VAR.WebFormsCore.Code
{
    public static class GlobalConfig
    {
        private static IGlobalConfig _globalConfig;

        public static IGlobalConfig Get()
        {
            if (_globalConfig != null) { return _globalConfig; }

            Type iGlobalConfig = typeof(IGlobalConfig);
            Type foundGlobalConfig = AppDomain.CurrentDomain
                .GetAssemblies()
                .SelectMany(
                    x =>
                        x.GetTypes()
                )
                .FirstOrDefault(
                    x =>
                        x.IsAbstract == false &&
                        x.IsInterface == false &&
                        iGlobalConfig.IsAssignableFrom(x)
                );
            _globalConfig = ObjectActivator.CreateInstance(foundGlobalConfig) as IGlobalConfig;

            return _globalConfig;
        }
    }
}