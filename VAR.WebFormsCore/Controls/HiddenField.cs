using System.IO;

namespace VAR.WebFormsCore.Controls
{
    // TODO: Implememnt control
    public class HiddenField : Control
    {
        public string Value { get; set; }
        public override void Render(TextWriter textWriter)
        {
            textWriter.Write("<input type=\"hidden\" ");
            RenderAttributes(textWriter);
            if (string.IsNullOrEmpty(Value) == false)
            {
                textWriter.Write(" value=\"{0}\"", Value);
            }
            textWriter.Write(">");
            textWriter.Write("</input>");
        }
    }
}