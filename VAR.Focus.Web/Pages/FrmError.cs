using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VAR.Focus.Web.Controls;

namespace VAR.Focus.Web.Pages
{
    public class FrmError : PageCommon
    {
        #region Declarations

        private Exception _ex = null;

        #endregion Declarations

        #region Page life cycle

        public FrmError(Exception ex)
        {
            _ex = ex;
            Init += FrmError_Init;
        }

        private void FrmError_Init(object sender, EventArgs e)
        {
            InitializeControls();
        }

        #endregion Page life cycle

        #region Private methods

        private void InitializeControls()
        {
            Title = "Application Error";

            CLabel lblErrorTitle = new CLabel { Text = Title, Tag = "h2" };
            Controls.Add(lblErrorTitle);

            Exception exAux = _ex;
            if (exAux is HttpUnhandledException && exAux.InnerException != null) { exAux = exAux.InnerException; }
            while (exAux != null)
            {
                CLabel lblMessage = new CLabel { Tag = "P" };
                lblMessage.Text = String.Format("<b>{0}:</b> {1}", "Message", HttpUtility.HtmlEncode(exAux.Message));
                Controls.Add(lblMessage);

                CLabel lblStacktraceTitle = new CLabel { Tag = "p" };
                lblStacktraceTitle.Text = String.Format("<b>{0}:</b>", "Stacktrace");
                Controls.Add(lblStacktraceTitle);
                Panel pnlStacktrace = new Panel();
                pnlStacktrace.CssClass = "divCode";
                Controls.Add(pnlStacktrace);
                LiteralControl litStackTrace = new LiteralControl(
                    String.Format("<pre><code>{0}</code></pre>", HttpUtility.HtmlEncode(exAux.StackTrace)));
                pnlStacktrace.Controls.Add(litStackTrace);

                exAux = exAux.InnerException;
            }
        }

        #endregion Private methods
    }
}