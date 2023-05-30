using System.IO;
using VAR.WebFormsCore.Code;

namespace VAR.WebFormsCore.Controls;

public class Label : Control
{
    #region Properties

    public string Tag { get; set; } = "span";

    public string Text { get; set; } = string.Empty;

    #endregion Properties

    #region Life cycle

    protected override void Render(TextWriter textWriter)
    {
        textWriter.Write("<{0} ", Tag);
        RenderAttributes(textWriter);
        textWriter.Write(">");

        textWriter.Write(ServerHelpers.HtmlEncode(Text));

        base.Render(textWriter);

        textWriter.Write("</{0}>", Tag);
    }

    #endregion Life cycle
}