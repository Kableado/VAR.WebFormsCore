using System.IO;

namespace VAR.WebFormsCore.Controls
{
    // TODO: Implememnt control
    public class TextBox : Control
    {
        public string Text { get; set; }

        public TextBoxMode TextMode { get; set; } = TextBoxMode.Normal;

        public override void Render(TextWriter textWriter)
        {
            if (TextMode == TextBoxMode.MultiLine)
            {
                textWriter.Write("<textare ");
                RenderAttributes(textWriter);
                textWriter.Write(">");
                textWriter.Write(Text);
                textWriter.Write("</textare>");
            }
            else if (TextMode == TextBoxMode.Normal)
            {
                textWriter.Write("<input type=\"text\" ");
                RenderAttributes(textWriter);
                if (string.IsNullOrEmpty(Text) == false)
                {
                    RenderAttribute(textWriter, "value", Text);
                }
                textWriter.Write(">");
                textWriter.Write("</input>");
            }
            else if (TextMode == TextBoxMode.Password)
            {
                textWriter.Write("<input type=\"password\" ");
                RenderAttributes(textWriter);
                if (string.IsNullOrEmpty(Text) == false)
                {
                    RenderAttribute(textWriter, "value", Text);
                }
                textWriter.Write(">");
                textWriter.Write("</input>");
            }
        }
    }

    public enum TextBoxMode
    {
        Normal,
        Password,
        MultiLine,
    }
}