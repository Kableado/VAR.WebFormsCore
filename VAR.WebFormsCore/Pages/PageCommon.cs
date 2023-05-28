using System;
using System.Reflection;
using VAR.WebFormsCore.Code;
using VAR.WebFormsCore.Controls;

namespace VAR.WebFormsCore.Pages;

public class PageCommon : Page
{
    #region Declarations

    private readonly HtmlHead _head = new();
    private readonly HtmlBody _body = new();
    private readonly HtmlForm _form = new() {ID = "formMain"};
    private readonly Panel _pnlContainer = new();
    private readonly Button _btnPostback = new();
    private readonly Button _btnLogout = new();

    private bool _isAuthenticated;

    #endregion Declarations

    #region Properties

    public new ControlCollection Controls => _pnlContainer.Controls;

    public bool MustBeAuthenticated { get; set; } = true;

    #endregion Properties

    #region Life cycle

    public PageCommon()
    {
        PreInit += PageCommon_PreInit;
        Init += PageCommon_Init;
        PreRender += PageCommon_PreRender;
    }

    private void PageCommon_PreInit(object? sender, EventArgs e)
    {
        Context?.PrepareUncacheableResponse();

        if (Context != null)
        {
            _isAuthenticated = GlobalConfig.Get().IsUserAuthenticated(Context);
        }

        if (MustBeAuthenticated && _isAuthenticated == false)
        {
            Context?.ResponseRedirect(GlobalConfig.Get().LoginHandler);
        }
    }

    private void PageCommon_Init(object? sender, EventArgs e) { CreateControls(); }

    private void PageCommon_PreRender(object? sender, EventArgs e)
    {
        _head.Title = string.IsNullOrEmpty(Title)
            ? GlobalConfig.Get().Title
            : string.Concat(Title, GlobalConfig.Get().TitleSeparator, GlobalConfig.Get().Title);
        _btnLogout.Visible = _isAuthenticated;
    }

    #endregion Life cycle

    #region UI Events

    private void btnLogout_Click(object? sender, EventArgs e)
    {
        if(Context != null)
        {
            GlobalConfig.Get().UserDeauthenticate(Context);
        }
        if (MustBeAuthenticated) { Context?.ResponseRedirect(GlobalConfig.Get().LoginHandler); }
    }

    #endregion UI Events

    #region Private methods

    private void CreateControls()
    {
        //Context.Response.Charset = Encoding.UTF8.WebName;

        var doctype = new LiteralControl("<!DOCTYPE html>\n");
        base.Controls.Add(doctype);

        var html = new HtmlGenericControl("html");
        base.Controls.Add(html);

        html.Controls.Add(_head);

        _head.Controls.Add(new HtmlMeta {HttpEquiv = "X-UA-Compatible", Content = "IE=Edge"});
        _head.Controls.Add(new HtmlMeta {HttpEquiv = "content-type", Content = "text/html; charset=utf-8"});
        _head.Controls.Add(new HtmlMeta {Name = "author", Content = GlobalConfig.Get().Author});
        _head.Controls.Add(new HtmlMeta {Name = "Copyright", Content = GlobalConfig.Get().Copyright});
        _head.Controls.Add(
            new HtmlMeta
            {
                Name = "viewport",
                Content = "width=device-width, initial-scale=1, maximum-scale=4, user-scalable=1"
            }
        );

        string? version = Assembly.GetExecutingAssembly().GetName().Version?.ToString();
        _head.Controls.Add(
            new LiteralControl($"<script type=\"text/javascript\" src=\"ScriptsBundler?v={version}\"></script>\n")
        );
        _head.Controls.Add(
            new LiteralControl($"<link href=\"StylesBundler?v={version}\" type=\"text/css\" rel=\"stylesheet\"/>\n")
        );

        html.Controls.Add(_body);
        _body.Controls.Add(_form);

        var pnlHeader = new Panel {CssClass = "divHeader"};
        _form.Controls.Add(pnlHeader);

        HyperLink lnkTitle = new HyperLink {NavigateUrl = "."};
        pnlHeader.Controls.Add(lnkTitle);

        var lblTitle = new Label {Text = GlobalConfig.Get().Title, Tag = "h1"};
        lnkTitle.Controls.Add(lblTitle);

        _btnPostback.ID = "btnPostback";
        _btnPostback.Text = "Postback";
        pnlHeader.Controls.Add(_btnPostback);
        _btnPostback.Style.Add("display", "none");

        var pnlUserInfo = new Panel {CssClass = "divUserInfo"};
        pnlHeader.Controls.Add(pnlUserInfo);

        _btnLogout.ID = "btnLogout";
        _btnLogout.Text = MultiLang.GetLiteral("Logout");
        _btnLogout.Click += btnLogout_Click;
        _btnLogout.Attributes.Add(
            "onclick",
            $"return confirm('{MultiLang.GetLiteral("ConfirmExit")}');"
        );
        pnlUserInfo.Controls.Add(_btnLogout);

        _pnlContainer.CssClass = "divContent";
        _form.Controls.Add(_pnlContainer);
    }

    #endregion Private methods
}