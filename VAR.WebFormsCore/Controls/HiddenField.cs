using System.IO;

namespace VAR.WebFormsCore.Controls;

public class HiddenField : Control
{
    private string _value = string.Empty;

    public string Value
    {
        get => _value;
        set => _value = value;
    }

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
        if (string.IsNullOrEmpty(Value) == false) { RenderAttribute(textWriter, "value", _value); }

        textWriter.Write(">");
        textWriter.Write("</input>");
    }
}