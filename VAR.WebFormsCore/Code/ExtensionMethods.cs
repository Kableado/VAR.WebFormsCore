using Microsoft.AspNetCore.Http;
using VAR.Json;

namespace VAR.WebFormsCore.Code
{
    public static class ExtensionMethods
    {
        #region HttpContext

        public static string GetRequestParm(this HttpContext context, string parm)
        {
            foreach (string key in context.Request.Query.Keys)
            {
                if (string.IsNullOrEmpty(key) == false && key.EndsWith(parm))
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
            // TODO: Implement on asp.net core
            //const int secondsInDay = 86400;
            //response.ExpiresAbsolute = DateTime.Now.AddSeconds(secondsInDay);
            //response.Expires = secondsInDay;
            //response.Cache.SetCacheability(HttpCacheability.Public);
            //response.Cache.SetMaxAge(new TimeSpan(0, 0, secondsInDay));
        }

        public static void PrepareUncacheableResponse(this HttpResponse response)
        {
            // TODO: Implement on asp.net core
            //response.ExpiresAbsolute = DateTime.Now.AddDays(-2d);
            //response.Expires = -1500;
            //response.Headers.Add("Cache-Control", "max-age=0, no-cache, no-store");
            //response.BufferOutput = true;
        }

        #endregion HttpContext
    }
}