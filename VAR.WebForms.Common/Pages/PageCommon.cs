using System;
using System.Reflection;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using VAR.WebForms.Common.Code;
using VAR.WebForms.Common.Controls;

namespace VAR.WebForms.Common.Pages
{
    public class PageCommon : Page
    {
        #region Declarations

        private HtmlHead _head;
        private HtmlGenericControl _body;
        private HtmlForm _form;
        private Panel _pnlContainer = new Panel();
        private CButton _btnPostback = new CButton();
        private CButton _btnLogout = new CButton();

        private bool _mustBeAutenticated = true;
        private bool _isAuthenticated = false;

        #endregion Declarations

        #region Properties

        public new ControlCollection Controls
        {
            get { return _pnlContainer.Controls; }
        }

        public bool MustBeAutenticated
        {
            get { return _mustBeAutenticated; }
            set { _mustBeAutenticated = value; }
        }

        #endregion Properties

        #region Life cycle

        public PageCommon()
        {
            PreInit += PageCommon_PreInit;
            Init += PageCommon_Init;
            PreRender += PageCommon_PreRender;
        }

        private void PageCommon_PreInit(object sender, EventArgs e)
        {
            Context.Response.PrepareUncacheableResponse();

            _isAuthenticated = GlobalConfig.Get().IsUserAuthenticated(Context);
            if (_mustBeAutenticated && _isAuthenticated == false)
            {
                Response.Redirect(GlobalConfig.Get().LoginHandler);
            }
        }

        private void PageCommon_Init(object sender, EventArgs e)
        {
            CreateControls();
        }

        private void PageCommon_PreRender(object sender, EventArgs e)
        {
            _head.Title = string.IsNullOrEmpty(Title) ? GlobalConfig.Get().Title : string.Concat(Title, GlobalConfig.Get().TitleSeparator, GlobalConfig.Get().Title);
            _btnLogout.Visible = _isAuthenticated;
        }

        #endregion Life cycle

        #region UI Events

        private void btnLogout_Click(object sender, EventArgs e)
        {
            GlobalConfig.Get().UserUnauthenticate(Context);
            if (_mustBeAutenticated)
            {
                Response.Redirect(GlobalConfig.Get().LoginHandler);
            }
        }

        #endregion UI Events

        #region Private methods

        private void CreateControls()
        {
            Context.Response.Charset = Encoding.UTF8.WebName;

            var doctype = new LiteralControl("<!DOCTYPE html>\n");
            base.Controls.Add(doctype);

            var html = new HtmlGenericControl("html");
            base.Controls.Add(html);

            _head = new HtmlHead();
            html.Controls.Add(_head);

            _head.Controls.Add(new HtmlMeta { HttpEquiv = "X-UA-Compatible", Content = "IE=Edge" });
            _head.Controls.Add(new HtmlMeta { HttpEquiv = "content-type", Content = "text/html; charset=utf-8" });
            _head.Controls.Add(new HtmlMeta { Name = "author", Content = GlobalConfig.Get().Author });
            _head.Controls.Add(new HtmlMeta { Name = "Copyright", Content = GlobalConfig.Get().Copyright });
            _head.Controls.Add(new HtmlMeta { Name = "viewport", Content = "width=device-width, initial-scale=1, maximum-scale=4, user-scalable=1" });

            string version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            _head.Controls.Add(new LiteralControl(string.Format("<script type=\"text/javascript\" src=\"ScriptsBundler?v={0}\"></script>\n", version)));
            _head.Controls.Add(new LiteralControl(string.Format("<link href=\"StylesBundler?v={0}\" type=\"text/css\" rel=\"stylesheet\"/>\n", version)));

            _body = new HtmlGenericControl("body");
            html.Controls.Add(_body);
            _form = new HtmlForm { ID = "formMain" };
            _body.Controls.Add(_form);

            var pnlHeader = new Panel { CssClass = "divHeader" };
            _form.Controls.Add(pnlHeader);

            HyperLink lnkTitle = new HyperLink();
            lnkTitle.NavigateUrl = ".";
            pnlHeader.Controls.Add(lnkTitle);

            var lblTitle = new CLabel { Text = GlobalConfig.Get().Title, Tag = "h1" };
            lnkTitle.Controls.Add(lblTitle);

            _btnPostback.ID = "btnPostback";
            _btnPostback.Text = "Postback";
            pnlHeader.Controls.Add(_btnPostback);
            _btnPostback.Style.Add("display", "none");

            var pnlUserInfo = new Panel { CssClass = "divUserInfo" };
            pnlHeader.Controls.Add(pnlUserInfo);

            _btnLogout.ID = "btnLogout";
            _btnLogout.Text = MultiLang.GetLiteral("Logout");
            _btnLogout.Click += btnLogout_Click;
            _btnLogout.Attributes.Add("onclick", string.Format("return confirm('{0}');", MultiLang.GetLiteral("ConfirmExit")));
            pnlUserInfo.Controls.Add(_btnLogout);

            _pnlContainer.CssClass = "divContent";
            _form.Controls.Add(_pnlContainer);
        }

        #endregion Private methods
    }
}