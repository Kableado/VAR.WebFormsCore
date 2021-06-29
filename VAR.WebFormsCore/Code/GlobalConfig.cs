using System;
using System.Linq;

namespace VAR.WebFormsCore.Code
{
    public static class GlobalConfig
    {
        private static IGlobalConfig _globalConfig = null;

        public static IGlobalConfig Get()
        {
            if (_globalConfig == null)
            {
                Type iGlobalConfig = typeof(IGlobalConfig);
                Type foundGlobalConfig = AppDomain.CurrentDomain
                    .GetAssemblies()
                    .SelectMany(x => x.GetTypes())
                    .Where(x =>
                        x.IsAbstract == false &&
                        x.IsInterface == false &&
                        iGlobalConfig.IsAssignableFrom(x) &&
                        true)
                    .FirstOrDefault();
                _globalConfig = ObjectActivator.CreateInstance(foundGlobalConfig) as IGlobalConfig;
            }
            return _globalConfig;
        }
    }
}
