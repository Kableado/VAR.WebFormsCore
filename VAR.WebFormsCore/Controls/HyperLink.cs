using System.IO;

namespace VAR.WebFormsCore.Controls;

public class HyperLink : Control
{
    public string NavigateUrl { get; set; } = string.Empty;
    public string Text { get; set; } = string.Empty;

    protected override void Render(TextWriter textWriter)
    {
        textWriter.Write("<a ");
        RenderAttributes(textWriter);
        if (string.IsNullOrEmpty(NavigateUrl) == false) { textWriter.Write(" href=\"{0}\"", NavigateUrl); }

        textWriter.Write(">");

        if (string.IsNullOrEmpty(Text) == false) { textWriter.Write(Text); }

        base.Render(textWriter);

        textWriter.Write("</a>");
    }
}