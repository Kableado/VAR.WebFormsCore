﻿using System.Web.UI;
using System.Web.UI.WebControls;
using Scrummer.Controls;

namespace Scrummer.Pages
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

        public static bool Control_IsValid(Control control)
        {
            if (control is IValidableControl)
            {
                if (((IValidableControl)control).IsValid() == false)
                {
                    return false;
                }
            }
            return true;
        }

        public static bool Controls_AreValid(ControlCollection controls) {
            bool valid = true;
            foreach (Control control in controls)
            {
                if (Control_IsValid(control))
                {
                    if (Controls_AreValid(control.Controls) == false)
                    {
                        valid = false;
                        break;
                    }
                }
                else
                {
                    valid = false;
                    break;
                }
            }
            return valid;
        }
    }
}