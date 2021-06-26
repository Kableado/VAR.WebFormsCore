using System;
using System.IO;

namespace VAR.WebFormsCore.Controls
{
    public class Button : Control, IReceivePostbackEvent
    {
        public Button()
        {
            CssClass = "button";
        }

        private string _text = string.Empty;

        public string Text { get { return _text; } set { _text = value; } }

        private string _commandArgument = string.Empty;

        public string CommandArgument { get { return _commandArgument; } set { _commandArgument = value; } }

        public event EventHandler Click;

        public override void Render(TextWriter textWriter)
        {
            textWriter.Write("<input type=\"submit\" ");
            RenderAttributes(textWriter);
            RenderAttribute(textWriter, "value", _text);
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