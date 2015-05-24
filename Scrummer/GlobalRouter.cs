using System;
using System.Web;
using Scrummer.Code;
using Scrummer.Code.JSON;

namespace Scrummer
{
    public class GlobalRouter : IHttpHandler
    {
        public bool IsReusable
        {
            get { return false; }
        }

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                context.Response.Write("Hello world!");
            }
            catch (Exception ex)
            {
                GlobalErrorHandler.HandleError(context, ex);
            }
        }
    }
}