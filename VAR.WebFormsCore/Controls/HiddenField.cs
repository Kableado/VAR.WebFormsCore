using System.IO;

namespace VAR.WebFormsCore.Controls
{
    // TODO: Implememnt control
    public class HiddenField : Control
    {
        public string Value { get; set; }

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
                textWriter.Write(" value=\"{0}\"", Value);
            }
            textWriter.Write(">");
            textWriter.Write("</input>");
        }
    }
}