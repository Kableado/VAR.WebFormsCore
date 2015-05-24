using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Scrummer.Code.Controls;

namespace Scrummer.Code.Pages
{
    public class FrmError : Page
    {
        Exception _ex = null;

        public FrmError(Exception ex)
        {
            _ex = ex;
            Init += FrmError_Init;
        }

        void FrmError_Init(object sender, EventArgs e)
        {
            CLabel lblMessage = new CLabel { ID = "lblMessage", Text = _ex.Message, Tag = "P" };
            Controls.Add(lblMessage);

            CLabel lblStackTrace = new CLabel { ID = "lblStackTrace", Text = _ex.StackTrace, Tag = "P" };
            Controls.Add(lblStackTrace);
        }
    }
}