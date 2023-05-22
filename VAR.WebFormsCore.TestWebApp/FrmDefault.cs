using VAR.WebFormsCore.Code;
using VAR.WebFormsCore.Controls;
using VAR.WebFormsCore.Pages;

namespace VAR.WebFormsCore.TestWebApp;

public class FrmDefault: PageCommon
{
    private readonly CTextBox _txtText = new() { ID = "txtText", CssClassExtra = "width150px", AllowEmpty = false };
    private readonly Button _btnTest = new() { ID = "btnTest", };
    private readonly Label _lblMessage = new() { ID = "lblMessage", };
    
    public FrmDefault()
    {
        MustBeAuthenticated = false;
        Init += FrmLogin_Init;
    }
    
    private void FrmLogin_Init(object? sender, EventArgs e)
    {
        InitializeControls();
    }

    private void InitializeControls()
    {
        Title = MultiLang.GetLiteral("Default");
        var lblTitle = new Label { Text = Title, Tag = "h2" };
        Controls.Add(lblTitle);

        Controls.Add(FormUtils.CreateField(MultiLang.GetLiteral("Text"), _txtText));
        _txtText.PlaceHolder = MultiLang.GetLiteral("Text");

        Controls.Add(FormUtils.CreateField(string.Empty, _btnTest));
        _btnTest.Text = MultiLang.GetLiteral("Test");
        _btnTest.Click += BtnTest_Click;
        
        Controls.Add(FormUtils.CreateField(string.Empty, _lblMessage));
    }

    private void BtnTest_Click(object? sender, EventArgs e)
    {
        _lblMessage.Text = $"Hello World: {_txtText.Text}";
    }
}