using System;
using System.Reflection;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using VAR.Focus.Web.Code.BusinessLogic;
using VAR.Focus.Web.Code.Entities;
using VAR.Focus.Web.Controls;

namespace VAR.Focus.Web.Pages
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
        private User _currentUser = null;

        #endregion

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

        public User CurrentUser
        {
            get { return _currentUser; }
        }

        #endregion

        #region Life cycle

        public PageCommon()
        {
            PreInit += PageCommon_PreInit;
            Init += PageCommon_Init;
            PreRender += PageCommon_PreRender;
        }

        void PageCommon_PreInit(object sender, EventArgs e)
        {
            Session session = Sessions.Current.Session_GetCurrent(Context);
            if (session != null)
            {
                _currentUser = Users.Current.User_GetByName(session.UserName);
                if (_mustBeAutenticated)
                {
                    Sessions.Current.Session_SetCookie(Context, session);
                }
            }
            if (_currentUser == null && _mustBeAutenticated)
            {
                Response.Redirect("FrmLogin");
            }
        }

        void PageCommon_Init(object sender, EventArgs e)
        {
            CreateControls();
        }

        void PageCommon_PreRender(object sender, EventArgs e)
        {
            _head.Title = string.IsNullOrEmpty(Title) ? Globals.Title : String.Format("{0}{1}{2}", Globals.Title, Globals.TitleSeparator, Title);
            _btnLogout.Visible = (_currentUser != null);
        }

        #endregion

        #region UI Events

        void btnLogout_Click(object sender, EventArgs e)
        {
            Sessions.Current.Session_FinalizeCurrent(Context);
            _currentUser = null;
            if (_mustBeAutenticated)
            {
                Response.Redirect("FrmLogin");
            }
        }

        #endregion

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

            _head.Controls.Add(new HtmlMeta { HttpEquiv = "content-type", Content = "text/html; charset=utf-8" });
            _head.Controls.Add(new HtmlMeta { Name = "author", Content = Globals.Author });
            _head.Controls.Add(new HtmlMeta { Name = "Copyright", Content = Globals.Copyright });
            Assembly asm = Assembly.GetExecutingAssembly();
            string version = asm.GetName().Version.ToString();
            _head.Controls.Add(new LiteralControl(String.Format("<script type=\"text/javascript\" src=\"ScriptsBundler?t={0}\"></script>\n", version)));
            _head.Controls.Add(new LiteralControl(String.Format("<link href=\"StylesBundler?t={0}\" type=\"text/css\" rel=\"stylesheet\"/>\n", version)));


            _body = new HtmlGenericControl("body");
            html.Controls.Add(_body);
            _form = new HtmlForm { ID = "formMain" };
            _body.Controls.Add(_form);

            var pnlHeader = new Panel { CssClass = "divHeader" };
            _form.Controls.Add(pnlHeader);

            HyperLink lnkTitle = new HyperLink();
            lnkTitle.NavigateUrl = ".";
            pnlHeader.Controls.Add(lnkTitle);

            var lblTitle = new CLabel { Text = Globals.Title, Tag = "h1" };
            lnkTitle.Controls.Add(lblTitle);

            _btnPostback.ID = "btnPostback";
            _btnPostback.Text = "Postback";
            pnlHeader.Controls.Add(_btnPostback);
            _btnPostback.Style.Add("display", "none");

            var pnlUserInfo = new Panel { CssClass = "divUserInfo" };
            pnlHeader.Controls.Add(pnlUserInfo);

            _btnLogout.ID = "btnLogout";
            _btnLogout.Text = "Logout";
            _btnLogout.Click += btnLogout_Click;
            _btnLogout.Attributes.Add("onclick", String.Format("return confirm('{0}');", "¿Are you sure to exit?"));
            pnlUserInfo.Controls.Add(_btnLogout);

            _pnlContainer.CssClass = "divContent";
            _form.Controls.Add(_pnlContainer);
        }

        #endregion
    }
}
