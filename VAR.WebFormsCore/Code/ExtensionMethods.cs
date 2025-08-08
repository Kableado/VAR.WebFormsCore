using System.Collections.Generic;
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
            if (context.RequestForm.ContainsKey(parameter))
            {
                return context.RequestForm.SafeGet(parameter, null) ?? string.Empty;
            }
        }
        
        if (context.RequestQuery.ContainsKey(parameter))
        {
            return context.RequestQuery.SafeGet(parameter, null) ?? string.Empty;
        }
        
        return string.Empty;
    }

    public static void ResponseObject(this IWebContext context, object obj, string contentType = "application/json")
    {
        context.ResponseContentType = contentType;
        string strObject = JsonWriter.WriteObject(obj);
        byte[] byteObject = Encoding.UTF8.GetBytes(strObject);
        context.ResponseWriteBin(byteObject);
    }

    #endregion IWebContext
    
    #region Dictionary

    public static TValue? SafeGet<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue defaultValue)
    {
        return dictionary.TryGetValue(key, out TValue? value) ? value : defaultValue;
    }

    public static void SafeSet<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue value)
    {
        dictionary[key] = value;
    }

    public static void SafeRemove<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
    {
        dictionary.Remove(key);
    }
    
    #endregion Dictionary
}