using System.Text;
using VAR.Json;

namespace VAR.WebFormsCore.Code;

public static class ExtensionMethods
{
    #region IWebContext

    public static string GetRequestParameter(this IWebContext context, string parameter)
    {
        if (context.RequestMethod == "POST")
        {
            foreach (string key in context.RequestForm.Keys)
            {
                if (string.IsNullOrEmpty(key) == false && key == parameter)
                {
                    return context.RequestForm[key] ?? string.Empty;
                }
            }
        }

        foreach (string key in context.RequestQuery.Keys)
        {
            if (string.IsNullOrEmpty(key) == false && key == parameter)
            {
                return context.RequestQuery[key] ?? string.Empty;
            }
        }

        return string.Empty;
    }

    private static readonly Encoding Utf8Encoding = new UTF8Encoding();

    public static void ResponseObject(this IWebContext context, object obj, string contentType = "text/json")
    {
        context.ResponseContentType = contentType;
        string strObject = JsonWriter.WriteObject(obj);
        byte[] byteObject = Utf8Encoding.GetBytes(strObject);
        context.ResponseWriteBin(byteObject);
    }

    #endregion IWebContext
}