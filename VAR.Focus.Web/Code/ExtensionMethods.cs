using System.Web;
using VAR.Focus.BusinessLogic.JSON;

namespace VAR.Focus.Web.Code
{
    public static class ExtensionMethods
    {
        #region HttpContext

        public static string GetRequestParm(this HttpContext context, string parm)
        {
            foreach (string key in context.Request.Params.AllKeys)
            {
                if (string.IsNullOrEmpty(key) == false && key.EndsWith(parm))
                {
                    return context.Request.Params[key];
                }
            }
            return string.Empty;
        }

        public static void ResponseObject(this HttpContext context, object obj)
        {
            var jsonWritter = new JsonWriter(true);
            context.Response.ContentType = "text/json";
            string strObject = jsonWritter.Write(obj);
            context.Response.Write(strObject);
        }

        #endregion

    }
}