using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Scrummer.Code.Controls;

namespace Scrummer.Code.Pages
{
    public class FormUtils
    {
        public static Control CreateField(string label, Control fieldControl)
        {
            Panel pnlRow = new Panel();
            pnlRow.CssClass = "formRow";

            Panel pnlLabelContainer = new Panel();
            pnlLabelContainer.CssClass = "formLabel width25pc";
            pnlRow.Controls.Add(pnlLabelContainer);

            if (string.IsNullOrEmpty(label) == false)
            {
                CLabel lblField = new CLabel();
                lblField.Text = label;
                pnlLabelContainer.Controls.Add(lblField);
            }

            Panel pnlFieldContainer = new Panel();
            pnlFieldContainer.CssClass = "formField width75pc";
            pnlRow.Controls.Add(pnlFieldContainer);

            pnlFieldContainer.Controls.Add(fieldControl);

            return pnlRow;
        }

    }
}