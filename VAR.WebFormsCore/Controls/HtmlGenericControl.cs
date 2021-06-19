using VAR.WebForms.Common.Controls;

namespace VAR.WebForms.Common.Pages
{
    public class HtmlGenericControl : Control
    {
        private string _tag;

        public HtmlGenericControl(string tag)
        {
            this._tag = tag;
        }
    }
}