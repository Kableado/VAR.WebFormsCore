using VAR.WebForms.Common.Controls;

namespace VAR.WebForms.Common.Pages
{
    public class LiteralControl : Control
    {
        public string Content { get; set; }

        public LiteralControl() { }
        public LiteralControl(string content) { Content = content; }

    }
}