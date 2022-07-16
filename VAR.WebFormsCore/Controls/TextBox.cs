using System.IO;
using VAR.WebFormsCore.Code;

namespace VAR.WebFormsCore.Controls
{
    public class TextBox : Control
    {
        public string Text { get; set; } = string.Empty;

        public TextBoxMode TextMode { get; set; } = TextBoxMode.Normal;

        protected override void Process()
        {
            if (Page.IsPostBack && Page.Context.Request.Form.ContainsKey(ClientID))
            {
                Text = Page.Context.Request.Form[ClientID];
            }
        }

        protected override void Render(TextWriter textWriter)
        {
            if (TextMode == TextBoxMode.MultiLine)
            {
                textWriter.Write("<textarea ");
                RenderAttributes(textWriter, forceId: true);
                textWriter.Write(">");
                textWriter.Write(ServerHelpers.HtmlEncode(Text));
                textWriter.Write("</textarea>");
            }
            else if (TextMode == TextBoxMode.Normal)
            {
                textWriter.Write("<input type=\"text\" ");
                RenderAttributes(textWriter, forceId: true);
                if (string.IsNullOrEmpty(Text) == false) { RenderAttribute(textWriter, "value", Text); }

                textWriter.Write(">");
                textWriter.Write("</input>");
            }
            else if (TextMode == TextBoxMode.Password)
            {
                textWriter.Write("<input type=\"password\" ");
                RenderAttributes(textWriter, forceId: true);
                if (string.IsNullOrEmpty(Text) == false) { RenderAttribute(textWriter, "value", Text); }

                textWriter.Write(">");
                textWriter.Write("</input>");
            }
        }
    }

    public enum TextBoxMode
    {
        Normal, Password, MultiLine,
    }
}