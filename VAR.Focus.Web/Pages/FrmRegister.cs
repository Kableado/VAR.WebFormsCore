using System;
using System.Web.UI.WebControls;
using VAR.Focus.BusinessLogic;
using VAR.Focus.BusinessLogic.Entities;
using VAR.Focus.Web.Controls;

namespace VAR.Focus.Web.Pages
{
    public class FrmRegister : PageCommon
    {
        #region Declarations

        private Panel _pnlRegister = new Panel { ID = "pnlRegister" };
        private CTextBox _txtName = new CTextBox { ID = "txtName", CssClassExtra = "width150px", AllowEmpty = false };
        private CTextBox _txtEmail = new CTextBox { ID = "txtEmail", CssClassExtra = "width150px", AllowEmpty = false };
        private CTextBox _txtPassword1 = new CTextBox { ID = "txtPassword1", CssClass = "width150px", AllowEmpty = false, TextMode = TextBoxMode.Password };
        private CTextBox _txtPassword2 = new CTextBox { ID = "txtPassword2", CssClass = "width150px", AllowEmpty = false, TextMode = TextBoxMode.Password };
        private CButton _btnRegister = new CButton { ID = "btnRegister" };
        private CButton _btnExit = new CButton { ID = "btnExit" };
        private Panel _pnlSuccess = new Panel { ID = "pnlSuccess" };
        private CLabel _lblSuccess = new CLabel { ID = "lblSuccess" };
        private CButton _btnExitSuccess = new CButton { ID = "btnExitSuccess" };

        #endregion Declarations

        #region Page life cycle

        public FrmRegister()
        {
            MustBeAutenticated = false;
            Init += FrmRegister_Init;
        }

        private void FrmRegister_Init(object sender, EventArgs e)
        {
            InitializeComponents();
        }

        #endregion Page life cycle

        #region UI Events

        private void btnRegister_Click(object sender, EventArgs e)
        {
            if (FormUtils.Controls_AreValid(Controls) == false) { return; }

            // FIXME: Check Email

            // Check password
            if (_txtPassword1.Text != _txtPassword2.Text)
            {
                _txtPassword1.MarkedInvalid = true;
                _txtPassword2.MarkedInvalid = true;
                _txtPassword1.Text = String.Empty;
                _txtPassword2.Text = String.Empty;
                return;
            }

            User user = Users.Current.User_Set(_txtName.Text, _txtEmail.Text, _txtPassword1.Text);

            _pnlRegister.Visible = false;
            _pnlSuccess.Visible = true;
            _lblSuccess.Text = String.Format("User {0} created sucessfully", user.Name);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect(Globals.DefaultHandler);
        }

        #endregion UI Events

        #region Private methods

        private void InitializeComponents()
        {
            Title = "Register";
            var lblTitle = new CLabel { Text = "Register", Tag = "h2" };
            Controls.Add(lblTitle);

            Controls.Add(_pnlRegister);

            _pnlRegister.Controls.Add(FormUtils.CreateField("Name", _txtName));
            _txtName.NextFocusOnEnter = _txtEmail;
            _txtName.PlaceHolder = "Name";

            _pnlRegister.Controls.Add(FormUtils.CreateField("Email", _txtEmail));
            _txtEmail.NextFocusOnEnter = _txtPassword1;
            _txtEmail.PlaceHolder = "Email";

            _pnlRegister.Controls.Add(FormUtils.CreateField("Password", _txtPassword1));
            _txtPassword1.NextFocusOnEnter = _txtPassword2;
            _txtPassword1.PlaceHolder = "Password";

            _pnlRegister.Controls.Add(FormUtils.CreateField(String.Empty, _txtPassword2));
            _txtPassword2.NextFocusOnEnter = _btnRegister;
            _txtPassword2.PlaceHolder = "Password";

            _btnRegister.Text = "Register";
            _btnRegister.Click += btnRegister_Click;

            _btnExit.Text = "Exit";
            _btnExit.Click += btnExit_Click;

            Panel pnlButtons = new Panel();
            pnlButtons.Controls.Add(_btnRegister);
            pnlButtons.Controls.Add(_btnExit);
            _pnlRegister.Controls.Add(FormUtils.CreateField(String.Empty, pnlButtons));

            Controls.Add(_pnlSuccess);
            _pnlSuccess.Visible = false;

            _pnlSuccess.Controls.Add(_lblSuccess);

            _btnExitSuccess.Text = "Exit";
            _btnExitSuccess.Click += btnExit_Click;
            _pnlSuccess.Controls.Add(FormUtils.CreateField(String.Empty, _btnExitSuccess));
        }

        #endregion Private methods
    }
}