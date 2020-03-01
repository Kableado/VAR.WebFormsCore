using System;
using System.Web;
using System.Web.UI.WebControls;
using VAR.Focus.BusinessLogic;
using VAR.Focus.BusinessLogic.Entities;
using VAR.Focus.Web.Code;
using VAR.WebForms.Common.Code;
using VAR.WebForms.Common.Controls;
using VAR.WebForms.Common.Pages;

namespace VAR.Focus.Web.Pages
{
    public class FrmBoardEdit : PageCommon
    {
        #region Declarations

        private int _idBoard = 0;

        private CTextBox _txtTitle = new CTextBox { ID = "txtTitle", CssClassExtra = "width100pc", AllowEmpty = false };
        private CTextBox _txtDescription = new CTextBox { ID = "txtDescription", CssClassExtra = "width100pc", TextMode = TextBoxMode.MultiLine, KeepSize = true, };
        private CButton _btnSave = new CButton { ID = "btnSave" };
        private CButton _btnExit = new CButton { ID = "btnExit" };

        #endregion Declarations

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

        private void FrmBoardEdit_Init(object sender, EventArgs e)
        {
            string strIDBoard = Context.GetRequestParm("idBoard");
            if (string.IsNullOrEmpty(strIDBoard) == false)
            {
                _idBoard = Convert.ToInt32(strIDBoard);
            }
            if (_idBoard == 0)
            {
                Response.Redirect(nameof(FrmBoard));
            }
            InitializeComponents();
        }

        #endregion Page life cycle

        #region UI Events

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (FormUtils.Controls_AreValid(Controls) == false) { return; }

            User user = WebSessions.Current.Session_GetCurrentUser(Context);
            Board board = Boards.Current.Boards_SetBoard(
                _idBoard,
                _txtTitle.Text,
                _txtDescription.Text,
                _txtDescription.GetClientsideHeight(),
                user.Name);

            // FIXME: Notify User of "Save Succesfully"
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            string returnUrl = Context.GetRequestParm("returnUrl");
            if (string.IsNullOrEmpty(returnUrl))
            {
                Response.Redirect(FrmBoard.GetUrl(_idBoard));
            }
            else
            {
                Response.Redirect(returnUrl);
            }
        }

        #endregion UI Events

        #region Private methods

        private void InitializeComponents()
        {
            Title = MultiLang.GetLiteral("EditBoard");
            var lblTitle = new CLabel { Text = Title, Tag = "h2" };
            Controls.Add(lblTitle);

            Controls.Add(FormUtils.CreateField(MultiLang.GetLiteral("Title"), _txtTitle));
            _txtTitle.NextFocusOnEnter = _txtTitle;
            _txtTitle.PlaceHolder = MultiLang.GetLiteral("Title");

            Controls.Add(FormUtils.CreateField(MultiLang.GetLiteral("Description"), _txtDescription));
            _txtDescription.PlaceHolder = MultiLang.GetLiteral("Description");

            _btnSave.Text = MultiLang.GetLiteral("Save");
            _btnSave.Click += btnSave_Click;

            _btnExit.Text = MultiLang.GetLiteral("Exit");
            _btnExit.Click += btnExit_Click;

            Panel pnlButtons = new Panel();
            pnlButtons.Controls.Add(_btnSave);
            pnlButtons.Controls.Add(_btnExit);
            Controls.Add(FormUtils.CreateField(string.Empty, pnlButtons));
        }

        private void LoadData()
        {
            Board board = Boards.Current.Board_GetByIDBoard(_idBoard);

            _txtTitle.Text = board.Title;
            _txtDescription.Text = board.Description;
            _txtDescription.SetClientsideHeight(board.DescriptionHeight);
        }

        #endregion Private methods

        #region Public methods

        public static string GetUrl(int idBoard, string returnUrl = null)
        {
            if (string.IsNullOrEmpty(returnUrl))
            {
                return string.Format("{0}?idBoard={1}", nameof(FrmBoardEdit), idBoard);
            }
            return string.Format("{0}?idBoard={1}&returnUrl={2}", nameof(FrmBoardEdit), idBoard, HttpUtility.UrlEncode(returnUrl));
        }

        #endregion Public methods
    }
}