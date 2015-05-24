using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Scrummer.Code.Pages;

namespace Scrummer.Code
{
    public static class GlobalErrorHandler
    {
        #region Private methods

        private static void ShowInternalError(HttpContext context, Exception ex)
        {
            context.Response.StatusCode = 500;
            context.Response.Clear();
            context.Response.Write("<h2>Internal error</h2>");
            context.Response.Write(String.Format("<p><b>Message:</b> {0}</p>", ex.Message));
            context.Response.Write(String.Format("<p><b>StackTrace:</b></p> <pre><code>{0}</code></pre>", ex.StackTrace));

            // Fill response to 512 bytes to avoid browser "beauty" response of errors.
            long fillResponse = 512 - (ex.Message.Length + ex.StackTrace.Length); ;
            if (fillResponse > 0)
            {
                context.Response.Write("<!--");
                for (int i = 0; i < fillResponse; i++)
                {
                    context.Response.Write("A");
                }
                context.Response.Write("-->");
            }
        }

        #endregion

        #region Public methods

        public static void HandleError(HttpContext context,Exception ex){
            try
            {
                IHttpHandler frmError = new FrmError(ex);
                context.Response.Clear();
                context.Handler = frmError;
                frmError.ProcessRequest(context);
            }
            catch
            {
                ShowInternalError(context, ex);
            }
        }

        #endregion
    }
}