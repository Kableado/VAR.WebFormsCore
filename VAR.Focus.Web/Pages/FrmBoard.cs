using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VAR.Focus.BusinessLogic;
using VAR.Focus.BusinessLogic.Entities;
using VAR.Focus.Web.Code;
using VAR.Focus.Web.Controls;

namespace VAR.Focus.Web.Pages
{
    public class FrmBoard : PageCommon
    {
        #region Declarations

        private int _idBoard = 0;

        private CTextBox _txtTitle = new CTextBox { ID = "txtTitle", CssClassExtra = "width100pc", AllowEmpty = false, };
        private CTextBox _txtDescription = new CTextBox { ID = "txtDescription", CssClassExtra = "width100pc", TextMode = TextBoxMode.MultiLine, KeepSize = true, };

        #endregion Declarations

        #region Life cycle

        public FrmBoard()
        {
            Init += FrmBoard_Init;
        }

        private void FrmBoard_Init(object sender, EventArgs e)
        {
            string strIDBoard = Context.GetRequestParm("idBoard");
            if (string.IsNullOrEmpty(strIDBoard) == false)
            {
                _idBoard = Convert.ToInt32(strIDBoard);
            }
            if (_idBoard == 0)
            {
                FrmBoard_InitIndex();
            }
            else
            {
                FrmBoard_InitBoard();
            }
        }

        #endregion Life cycle

        #region UI Events

        private void btnAddBoard_Click(object sender, EventArgs e)
        {
            if (FormUtils.Controls_AreValid(Controls) == false) { return; }

            Board board = Boards.Current.Boards_SetBoard(0, _txtTitle.Text, _txtDescription.Text, CurrentUser.Name);
            _idBoard = board.IDBoard;

            Response.Redirect(GetUrl(_idBoard));
        }

        private void BtnView_Click(object sender, EventArgs e)
        {
            CButton btnView = (CButton)sender;
            int idBoard = Convert.ToInt32(btnView.CommandArgument);
            Response.Redirect(GetUrl(idBoard));
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            CButton btnEdit = (CButton)sender;
            int idBoard = Convert.ToInt32(btnEdit.CommandArgument);
            Response.Redirect(FrmBoardEdit.GetUrl(idBoard, nameof(FrmBoard)));
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            CButton btnEdit = (CButton)sender;
            int idBoard = Convert.ToInt32(btnEdit.CommandArgument);

            if (Boards.Current.Boards_DelBoard(idBoard, CurrentUser.Name))
            {
                Controls.Clear();
                FrmBoard_InitIndex();
            }
        }

        #endregion UI Events

        #region Private methods

        private Panel BoardSelector_Create(Board board)
        {
            var pnlBoardSelector = new Panel { CssClass = "boardBanner" };

            var lnkTitle = new HyperLink
            {
                NavigateUrl = GetUrl(board.IDBoard),
            };
            var lblTitle = new CLabel
            {
                Text = board.Title,
                CssClass = "title",
            };
            lnkTitle.Controls.Add(lblTitle);
            pnlBoardSelector.Controls.Add(lnkTitle);

            var lblDescription = new CLabel
            {
                Text = board.Description.Replace(" ", "&nbsp;").Replace("\n", "<br>"),
                CssClass = "description",
            };
            pnlBoardSelector.Controls.Add(FormUtils.CreatePanel("", lblDescription));

            Panel pnlButtons = (Panel)FormUtils.CreatePanel("formRow");
            var btnView = new CButton
            {
                ID = string.Format("btnView{0}", board.IDBoard),
                Text = "View",
            };
            btnView.CommandArgument = Convert.ToString(board.IDBoard);
            btnView.Click += BtnView_Click;
            pnlButtons.Controls.Add(btnView);
            var btnEdit = new CButton
            {
                ID = string.Format("btnEdit{0}", board.IDBoard),
                Text = "Edit",
            };
            btnEdit.CommandArgument = Convert.ToString(board.IDBoard);
            btnEdit.Click += BtnEdit_Click;
            pnlButtons.Controls.Add(btnEdit);
            var btnDelete = new CButton
            {
                ID = string.Format("btnDelete{0}", board.IDBoard),
                Text = "Delete",
            };
            btnDelete.CommandArgument = Convert.ToString(board.IDBoard);
            btnDelete.Click += BtnDelete_Click;
            btnDelete.Attributes.Add("onclick", string.Format("return confirm('{0}');", "¿Are you sure to delete?"));
            pnlButtons.Controls.Add(btnDelete);
            pnlBoardSelector.Controls.Add(pnlButtons);

            return pnlBoardSelector;
        }

        private void FrmBoard_InitIndex()
        {
            Title = "Boards";

            List<Board> boards = Boards.Current.Boards_GetListForUser(CurrentUser.Name);
            foreach (Board board in boards)
            {
                Panel pnlBoardSelector = BoardSelector_Create(board);
                Controls.Add(pnlBoardSelector);
            }

            // Board creator
            var pnlBoardAdd = new Panel { CssClass = "boardBanner" };
            var btnAddBoard = new CButton { ID = "btnAddBoard", Text = "AddBoard" };
            btnAddBoard.Click += btnAddBoard_Click;
            pnlBoardAdd.Controls.Add(FormUtils.CreatePanel("formRow", _txtTitle));
            _txtTitle.PlaceHolder = "Title";
            pnlBoardAdd.Controls.Add(FormUtils.CreatePanel("formRow", _txtDescription));
            _txtDescription.PlaceHolder = "Description";
            pnlBoardAdd.Controls.Add(FormUtils.CreatePanel("formRow", btnAddBoard));
            Controls.Add(pnlBoardAdd);
        }

        private void FrmBoard_InitBoard()
        {
            Board board = Boards.Current.Board_GetByIDBoard(_idBoard);

            Title = board.Title;

            CtrCardBoard cardBoardControl = new CtrCardBoard
            {
                ID = "ctrCardBoard",
                IDBoard = board.IDBoard,
                UserName = CurrentUser.Name,
            };
            Controls.Add(cardBoardControl);

            CtrChat chatControl = new CtrChat
            {
                ID = "ctrChat",
                IDMessageBoard = string.Format("CardBoard_{0}", board.IDBoard),
                UserName = CurrentUser.Name,
            };
            Controls.Add(chatControl);
        }

        #endregion Private methods

        #region Public methods

        public static string GetUrl(int idBoard, string returnUrl = null)
        {
            if (string.IsNullOrEmpty(returnUrl))
            {
                return string.Format("{0}?idBoard={1}", nameof(FrmBoard), idBoard);
            }
            return string.Format("{0}?idBoard={1}&returnUrl={2}", nameof(FrmBoard), idBoard, HttpUtility.UrlEncode(returnUrl));
        }

        #endregion Public methods
    }
}