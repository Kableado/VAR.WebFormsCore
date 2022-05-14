using System.Collections.Generic;
using System.IO;
using VAR.Json;

namespace VAR.WebFormsCore.Code
{
    public static class MultiLang
    {
        private static string GetPrivatePath(string baseDir, string fileName)
        {
            string currentDir = Directory.GetCurrentDirectory();
            string privatePath = Path.Combine(currentDir, baseDir);
            while (Directory.Exists(privatePath) == false)
            {
                DirectoryInfo dirInfo = Directory.GetParent(currentDir);
                if (dirInfo == null) { break; }

                currentDir = dirInfo.FullName;
                privatePath = Path.Combine(currentDir, baseDir);
            }

            return Path.Combine(privatePath, fileName);
        }

        private static Dictionary<string, Dictionary<string, object>> _literals;

        private static void InitializeLiterals()
        {
            _literals = new Dictionary<string, Dictionary<string, object>>();

            JsonParser jsonParser = new JsonParser();
            foreach (string lang in new[] {"en", "es"})
            {
                string filePath = GetPrivatePath("Resources", $"Literals.{lang}.json");
                if (File.Exists(filePath) == false) { continue; }

                string strJsonLiteralsLanguage = File.ReadAllText(filePath);
                object result = jsonParser.Parse(strJsonLiteralsLanguage);
                _literals.Add(lang, result as Dictionary<string, object>);
            }
        }

        private const string DefaultLanguage = "en";

        private static string GetUserLanguage()
        {
            // TODO: Needs replacement for ctx.Request.UserLanguages
            //HttpContext ctx = HttpContext.Current;
            //if (ctx != null)
            //{
            //    if (ctx.Items["UserLang"] != null)
            //    {
            //        return (string)ctx.Items["UserLang"];
            //    }

            //    IEnumerable<string> userLanguages = ctx.Request.UserLanguages
            //        .Select(lang =>
            //        {
            //            if (lang.Contains(";"))
            //            {
            //                lang = lang.Split(';')[0];
            //            }
            //            if (lang.Contains("-"))
            //            {
            //                lang = lang.Split('-')[0];
            //            }
            //            return lang.ToLower();
            //        })
            //        .Where(lang => _literals.ContainsKey(lang));
            //    string userLang = userLanguages.FirstOrDefault() ?? _defaultLanguage;

            //    ctx.Items["UserLang"] = userLang;
            //    return userLang;
            //}
            return DefaultLanguage;
        }

        public static string GetLiteral(string resource, string culture = null)
        {
            if (_literals == null) { InitializeLiterals(); }

            culture ??= GetUserLanguage();

            if (_literals == null || _literals.ContainsKey(culture) == false) { return resource; }

            Dictionary<string, object> literalCurrentCulture = _literals[culture];

            if (literalCurrentCulture == null || literalCurrentCulture.ContainsKey(resource) == false)
            {
                return resource;
            }

            return (literalCurrentCulture[resource] as string) ?? resource;
        }
    }
}