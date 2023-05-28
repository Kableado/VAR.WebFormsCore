using Microsoft.AspNetCore.Http;

namespace VAR.WebFormsCore.AspNetCore.Code
{
    public static class ExtensionMethods
    {
        #region IHeaderDictionary

        public static void SafeSet(this IHeaderDictionary header, string key, string value) { header[key] = value; }

        public static void SafeDel(this IHeaderDictionary header, string key)
        {
            if (header.ContainsKey(key)) { header.Remove(key); }
        }

        #endregion IHeaderDictionary
    }
}