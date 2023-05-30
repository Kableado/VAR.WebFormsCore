using System.IO;

namespace VAR.WebFormsCore.Controls;

public class HiddenField : Control
{
    public string Value { get; set; } = string.Empty;

    protected override void Process()
    {
        if (Page?.IsPostBack == true && Page?.Context?.RequestForm.ContainsKey(ClientID) == true)
        {
            Value = Page?.Context.RequestForm[ClientID] ?? string.Empty;
        }
    }

    protected override void Render(TextWriter textWriter)
    {
        textWriter.Write("<input type=\"hidden\" ");
        RenderAttributes(textWriter, forceId: true);
        if (string.IsNullOrEmpty(Value) == false) { RenderAttribute(textWriter, "value", Value); }

        textWriter.Write(">");
        textWriter.Write("</input>");
    }
}