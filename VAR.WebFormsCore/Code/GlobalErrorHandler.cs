using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using VAR.WebFormsCore.Pages;

namespace VAR.WebFormsCore.Code
{
    public static class GlobalErrorHandler
    {
        #region Private methods

        private static void ShowInternalError(HttpContext context, Exception ex)
        {
            try
            {
                context.Response.StatusCode = 500;

                StringBuilder sbOutput = new StringBuilder();
                sbOutput.Append("<h2>Internal error</h2>");
                Exception exAux = ex;
                while (exAux != null)
                {
                    sbOutput.AppendFormat("<p><b>Message:</b> {0}</p>", exAux.Message);
                    sbOutput.AppendFormat("<p><b>StackTrace:</b></p> <pre><code>{0}</code></pre>", exAux.StackTrace);
                    exAux = exAux.InnerException;
                }

                // Fill response to 512 bytes to avoid browser "beauty" response of errors.
                long fillResponse = 512 - sbOutput.Length;
                if (fillResponse > 0)
                {
                    sbOutput.Append("<!--");
                    for (int i = 0; i < fillResponse; i++) { sbOutput.Append("A"); }

                    sbOutput.Append("-->");
                }

                context.Response.WriteAsync(sbOutput.ToString()).GetAwaiter().GetResult();
                context.Response.Body.FlushAsync().GetAwaiter().GetResult();
            }
            catch
            {
                /* Nom nom nom */
            }
        }

        #endregion Private methods

        #region Public methods

        public static void HandleError(HttpContext context, Exception ex)
        {
            try
            {
                IHttpHandler frmError = new FrmError(ex);
                //context.Response.Clear();
                //context.Handler = frmError;
                frmError.ProcessRequest(context);
                context.Response.Body.FlushAsync().GetAwaiter().GetResult();
            }
            catch { ShowInternalError(context, ex); }
        }

        #endregion Public methods
    }
}