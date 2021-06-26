using System;
using Microsoft.AspNetCore.Http;
using VAR.Json;

namespace VAR.WebFormsCore.Code
{
    public static class ExtensionMethods
    {
        #region HttpContext

        public static string GetRequestParm(this HttpContext context, string parm)
        {
            if (context.Request.Method == "POST")
            {
                foreach (string key in context.Request.Form.Keys)
                {
                    if (string.IsNullOrEmpty(key) == false && key == parm)
                    {
                        return context.Request.Form[key];
                    }
                }
            }
            foreach (string key in context.Request.Query.Keys)
            {
                if (string.IsNullOrEmpty(key) == false && key == parm)
                {
                    return context.Request.Query[key];
                }
            }
            return string.Empty;
        }

        public static void ResponseObject(this HttpContext context, object obj)
        {
            context.Response.ContentType = "text/json";
            string strObject = JsonWriter.WriteObject(obj);
            context.Response.WriteAsync(strObject);
        }

        public static void PrepareCacheableResponse(this HttpResponse response)
        {
            const int secondsInDay = 86400;
            response.Headers.Add("Cache-Control", string.Format("public, max-age={0}", secondsInDay));
            string ExpireDate = DateTime.UtcNow.AddSeconds(secondsInDay).ToString("ddd, dd MMM yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
            response.Headers.Add("Expires", ExpireDate + " GMT");
        }

        public static void PrepareUncacheableResponse(this HttpResponse response)
        {
            response.Headers.Add("Cache-Control", "max-age=0, no-cache, no-store");
            string ExpireDate = DateTime.UtcNow.AddSeconds(-1500).ToString("ddd, dd MMM yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
            response.Headers.Add("Expires", ExpireDate + " GMT");
        }

        #endregion HttpContext
    }
}