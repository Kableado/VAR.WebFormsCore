using System;
using System.Text;
using System.Web;
using VAR.Focus.Web.Pages;

namespace VAR.Focus.Web.Code
{
    public static class GlobalErrorHandler
    {
        #region Private methods

        private static void ShowInternalError(HttpContext context, Exception ex)
        {
            context.Response.StatusCode = 500;
            context.Response.Clear();

            StringBuilder sbOutput = new StringBuilder();
            sbOutput.Append("<h2>Internal error</h2>");
            Exception exAux = ex;
            if (exAux is HttpUnhandledException && exAux.InnerException != null) { exAux = exAux.InnerException; }
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
                for (int i = 0; i < fillResponse; i++)
                {
                    sbOutput.Append("A");
                }
                sbOutput.Append("-->");
            }

            context.Response.Write(sbOutput.ToString());
        }

        #endregion

        #region Public methods

        public static void HandleError(HttpContext context, Exception ex)
        {
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
