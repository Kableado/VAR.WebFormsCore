using System;
using System.IO;

namespace VAR.WebFormsCore.Controls;

public class Button : Control, IReceivePostbackEvent
{
    public Button() { CssClass = "button"; }

    public string Text { get; set; } = string.Empty;

    public string OnClientClick { get; set; } = string.Empty;

    public string CommandArgument { get; set; } = string.Empty;

    public event EventHandler? Click;

    protected override void Render(TextWriter textWriter)
    {
        textWriter.Write("<input type=\"submit\" ");
        RenderAttributes(textWriter);
        RenderAttribute(textWriter, "value", Text);
        if (string.IsNullOrEmpty(OnClientClick) == false) { RenderAttribute(textWriter, "onclick", OnClientClick); }

        textWriter.Write(">");

        base.Render(textWriter);

        textWriter.Write("</input>");
    }

    public void ReceivePostBack() { Click?.Invoke(this, EventArgs.Empty); }
}