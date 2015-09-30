using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using VAR.Focus.Web.Code;
using VAR.Focus.Web.Code.BusinessLogic;
using VAR.Focus.Web.Code.Entities;
using VAR.Focus.Web.Controls;

namespace VAR.Focus.Web.Pages
{
    public class FrmBoardEdit : PageCommon
    {
        #region Declarations

        private int _idBoard = 0;

        private CTextBox _txtTitle = new CTextBox { ID = "txtTitle", CssClassExtra = "width100pc", AllowEmpty = false };
        private CTextBox _txtDescription = new CTextBox { ID = "txtDescription", CssClassExtra = "width100pc", TextMode = TextBoxMode.MultiLine };
        private CButton _btnSave = new CButton { ID = "btnSave" };
        private CButton _btnExit = new CButton { ID = "btnExit" };

        #endregion

        #region Page life cycle

        public FrmBoardEdit()
        {
            Init += FrmBoardEdit_Init;
            Load += FrmBoardEdit_Load;
        }

        private void FrmBoardEdit_Load(object sender, EventArgs e)
        {
            if (IsPostBack == false)
            {
                LoadData();
            }
        }

        void FrmBoardEdit_Init(object sender, EventArgs e)
        {
            string strIDBoard = Context.GetRequestParm("idBoard");
            if (String.IsNullOrEmpty(strIDBoard) == false)
            {
                _idBoard = Convert.ToInt32(strIDBoard);
            }
            if (_idBoard == 0)
            {
                Response.Redirect(typeof(FrmBoard).Name);
            }
            InitializeComponents();
        }

        #endregion

        #region UI Events

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (FormUtils.Controls_AreValid(Controls) == false) { return; }

            Board board = Boards.Current.Boards_SetBoard(_idBoard, _txtTitle.Text, _txtDescription.Text, CurrentUser.Name);

            // FIXME: Notify User of "Save Succesfully"
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            string returnUrl = Context.GetRequestParm("returnUrl");
            if (string.IsNullOrEmpty(returnUrl))
            {
                Response.Redirect(string.Format("{0}?idBoard={1}", typeof(FrmBoard).Name, _idBoard));
            }
            else
            {
                Response.Redirect(returnUrl);
            }
        }

        #endregion

        #region Private methods

        private void InitializeComponents()
        {
            Title = "Register";
            var lblTitle = new CLabel { Text = "Register", Tag = "h2" };
            Controls.Add(lblTitle);
            
            Controls.Add(FormUtils.CreateField("Title", _txtTitle));
            _txtTitle.NextFocusOnEnter = _txtTitle;
            _txtTitle.PlaceHolder = "Title";

            Controls.Add(FormUtils.CreateField("Description", _txtDescription));
            _txtDescription.PlaceHolder = "Description";

            _btnSave.Text = "Save";
            _btnSave.Click += btnSave_Click;

            _btnExit.Text = "Exit";
            _btnExit.Click += btnExit_Click;

            Panel pnlButtons = new Panel();
            pnlButtons.Controls.Add(_btnSave);
            pnlButtons.Controls.Add(_btnExit);
            Controls.Add(FormUtils.CreateField(String.Empty, pnlButtons));
        }

        private void LoadData()
        {
            Board board = Boards.Current.Board_GetByIDBoard(_idBoard);

            _txtTitle.Text = board.Title;
            _txtDescription.Text = board.Description;
        }

        #endregion

    }
}