using System.IO;

namespace VAR.WebFormsCore.Controls;

public class HtmlGenericControl : Control
{
    private readonly string _tagName;

    public HtmlGenericControl(string tag) { _tagName = tag; }

    protected override void Render(TextWriter textWriter)
    {
        textWriter.Write("<{0} ", _tagName);
        RenderAttributes(textWriter);
        textWriter.Write(">");

        base.Render(textWriter);

        textWriter.Write("</{0}>", _tagName);
    }
}