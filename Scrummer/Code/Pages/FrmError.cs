using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Scrummer.Code.Controls;

namespace Scrummer.Code.Pages
{
    public class FrmError : PageCommon
    {
        Exception _ex = null;

        public FrmError(Exception ex)
        {
            _ex = ex;
            Init += FrmError_Init;
        }

        void FrmError_Init(object sender, EventArgs e)
        {
            Title = "Error";

            CLabel lblErrorTitle = new CLabel { Text = "Error", Tag = "h2" };
            Controls.Add(lblErrorTitle);

            CLabel lblMessage = new CLabel { Tag = "P" };
            lblMessage.Text = String.Format("<b>{0}:</b> {1}", "Message",HttpUtility.HtmlEncode(_ex.Message));
            Controls.Add(lblMessage);

            CLabel lblStacktraceTitle = new CLabel { Tag = "p" };
            lblStacktraceTitle.Text = String.Format("<b>{0}:</b>", "Stacktrace");
            Controls.Add(lblStacktraceTitle);
            Panel pnlStacktrace = new Panel();
            pnlStacktrace.CssClass = "divCode";
            Controls.Add(pnlStacktrace);
            LiteralControl litStackTrace = new LiteralControl(
                String.Format("<pre><code>{0}</code></pre>", HttpUtility.HtmlEncode(_ex.StackTrace)));
            pnlStacktrace.Controls.Add(litStackTrace);
        }
    }
}