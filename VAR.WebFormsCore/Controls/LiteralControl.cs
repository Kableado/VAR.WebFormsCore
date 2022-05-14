using System.IO;

namespace VAR.WebFormsCore.Controls
{
    public class LiteralControl : Control
    {
        private string Content { get; set; }

        public LiteralControl() { }
        public LiteralControl(string content) { Content = content; }

        public override void Render(TextWriter textWriter) { textWriter.Write(Content); }
    }
}