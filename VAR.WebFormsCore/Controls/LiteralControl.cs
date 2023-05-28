using System.IO;

namespace VAR.WebFormsCore.Controls;

public class LiteralControl : Control
{
    public string Content { get; set; } = string.Empty;

    public LiteralControl() { }
    public LiteralControl(string content) { Content = content; }

    protected override void Render(TextWriter textWriter) { textWriter.Write(Content); }
}