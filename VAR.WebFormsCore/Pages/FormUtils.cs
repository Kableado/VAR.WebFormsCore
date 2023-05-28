using VAR.WebFormsCore.Controls;

namespace VAR.WebFormsCore.Pages;

public static class FormUtils
{
    public static Control CreatePanel(string cssClass, Control? ctrl = null)
    {
        Panel pnl = new Panel();
        if (ctrl != null) { pnl.Controls.Add(ctrl); }

        if (string.IsNullOrEmpty(cssClass) == false) { pnl.CssClass = cssClass; }

        return pnl;
    }

    public static Control CreateField(string label, Control fieldControl)
    {
        Panel pnlRow = new Panel {CssClass = "formRow"};

        Panel pnlLabelContainer = new Panel {CssClass = "formLabel width25pc"};
        pnlRow.Controls.Add(pnlLabelContainer);

        if (string.IsNullOrEmpty(label) == false)
        {
            Label lblField = new Label {Text = label};
            pnlLabelContainer.Controls.Add(lblField);
        }

        Panel pnlFieldContainer = new Panel {CssClass = "formField width75pc"};
        pnlRow.Controls.Add(pnlFieldContainer);

        pnlFieldContainer.Controls.Add(fieldControl);

        return pnlRow;
    }

    public static bool Control_IsValid(Control control)
    {
        return (control as IValidableControl)?.IsValid() != false;
    }

    public static bool Controls_AreValid(ControlCollection controls)
    {
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