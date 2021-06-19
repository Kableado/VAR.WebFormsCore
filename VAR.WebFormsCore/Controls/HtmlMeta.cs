using VAR.WebForms.Common.Controls;

namespace VAR.WebForms.Common.Pages
{
    public class HtmlMeta : Control
    {
        public string Name { get; set; }
        public string Content { get; set; }
        public string HttpEquiv { get; internal set; }
    }
}