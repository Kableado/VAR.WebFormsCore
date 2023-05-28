using System;
using System.Text;
using VAR.WebFormsCore.Pages;

namespace VAR.WebFormsCore.Code;

public static class GlobalErrorHandler
{
    #region Private methods

    private static void ShowInternalError(IWebContext context, Exception ex)
    {
        try
        {
            context.ResponseStatusCode = 500;

            StringBuilder sbOutput = new StringBuilder();
            sbOutput.Append("<h2>Internal error</h2>");
            Exception? exAux = ex;
            while (exAux != null)
            {
                sbOutput.Append($"<p><b>Message:</b> {exAux.Message}</p>");
                sbOutput.Append($"<p><b>StackTrace:</b></p> <pre><code>{exAux.StackTrace}</code></pre>");
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

            context.ResponseWrite(sbOutput.ToString());
            context.ResponseFlush();
        }
        catch
        {
            /* Nom nom nom */
        }
    }

    #endregion Private methods

    #region Public methods

    public static void HandleError(IWebContext context, Exception ex)
    {
        try
        {
            IHttpHandler frmError = new FrmError(ex);
            //context.Response.Clear();
            //context.Handler = frmError;
            frmError.ProcessRequest(context);
            context.ResponseFlush();
        }
        catch { ShowInternalError(context, ex); }
    }

    #endregion Public methods
}