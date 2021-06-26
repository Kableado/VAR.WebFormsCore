using System.IO;

namespace VAR.WebFormsCore.Controls
{
    public class HiddenField : Control
    {
        private string _value = string.Empty;

        public string Value { get { return _value; } set { _value = value; } }

        protected override void Process()
        {
            if (Page.IsPostBack && Page.Context.Request.Form.ContainsKey(ClientID))
            {
                Value = Page.Context.Request.Form[ClientID];
            }
        }

        public override void Render(TextWriter textWriter)
        {
            textWriter.Write("<input type=\"hidden\" ");
            RenderAttributes(textWriter, forceId: true);
            if (string.IsNullOrEmpty(Value) == false)
            {
                RenderAttribute(textWriter, "value", _value);
            }
            textWriter.Write(">");
            textWriter.Write("</input>");
        }
    }
}