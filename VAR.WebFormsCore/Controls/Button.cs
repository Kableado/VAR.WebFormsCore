using System;
using System.IO;

namespace VAR.WebFormsCore.Controls
{
    // TODO: Implememnt control
    public class Button : Control, IReceivePostbackEvent
    {
        public Button()
        {
            CssClass = "button";
        }

        public string Text { get; set; }
        public string CommandArgument { get; set; }

        public event EventHandler Click;


        public override void Render(TextWriter textWriter)
        {
            textWriter.Write("<input type=\"submit\" ");
            RenderAttributes(textWriter);
            RenderAttribute(textWriter, "value", Text);
            textWriter.Write(">");

            base.Render(textWriter);

            textWriter.Write("</input>");
        }

        public void ReceivePostBack()
        {
            Click?.Invoke(this, null);
        }
    }
}