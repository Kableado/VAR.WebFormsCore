using System;

namespace VAR.WebForms.Common.Controls
{
    // TODO: Implememnt control
    public class CButton : Control
    {
        public CButton()
        {
            CssClass = "button";
        }

        public string Text { get; set; }
        public string CommandArgument { get; set; }

        public event EventHandler Click;
    }
}