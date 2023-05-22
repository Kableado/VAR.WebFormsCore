using System;
using System.Text;
using Microsoft.AspNetCore.Http;
using VAR.Json;

namespace VAR.WebFormsCore.Code
{
    public static class ExtensionMethods
    {
        #region HttpContext

        public static string? GetRequestParm(this HttpContext context, string parm)
        {
            if (context.Request.Method == "POST")
            {
                foreach (string key in context.Request.Form.Keys)
                {
                    if (string.IsNullOrEmpty(key) == false && key == parm) { return context.Request.Form[key]; }
                }
            }

            foreach (string key in context.Request.Query.Keys)
            {
                if (string.IsNullOrEmpty(key) == false && key == parm) { return context.Request.Query[key]; }
            }

            return string.Empty;
        }

        private static readonly Encoding Utf8Encoding = new UTF8Encoding();

        public static void ResponseObject(this HttpContext context, object obj, string contentType = "text/json")
        {
            context.Response.ContentType = contentType;
            string strObject = JsonWriter.WriteObject(obj);
            byte[] byteObject = Utf8Encoding.GetBytes(strObject);
            context.Response.Body.WriteAsync(byteObject).GetAwaiter().GetResult();
        }

        public static void SafeSet(this IHeaderDictionary header, string key, string value) { header[key] = value; }

        public static void SafeDel(this IHeaderDictionary header, string key)
        {
            if (header.ContainsKey(key)) { header.Remove(key); }
        }

        public static void PrepareCacheableResponse(this HttpResponse response)
        {
            const int secondsInDay = 86400;
            response.Headers.SafeSet("Cache-Control", $"public, max-age={secondsInDay}");
            string expireDate = DateTime.UtcNow.AddSeconds(secondsInDay)
                .ToString("ddd, dd MMM yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
            response.Headers.SafeSet("Expires", expireDate + " GMT");
        }

        public static void PrepareUncacheableResponse(this HttpResponse response)
        {
            response.Headers.SafeSet("Cache-Control", "max-age=0, no-cache, no-store");
            string expireDate = DateTime.UtcNow.AddSeconds(-1500)
                .ToString("ddd, dd MMM yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
            response.Headers.SafeSet("Expires", expireDate + " GMT");
        }

        #endregion HttpContext
    }
}