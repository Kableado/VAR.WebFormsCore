using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI.WebControls;
using VAR.Focus.Web.Code.BusinessLogic;
using VAR.Focus.Web.Code.Entities;
using VAR.Focus.Web.Controls;

namespace VAR.Focus.Web.Pages
{
    public class FrmBoard : PageCommon
    {
        #region Declarations

        private int _idBoard = 0;

        private CTextBox _txtTitle = new CTextBox { ID = "txtTitle", CssClassExtra="width100pc" };
        private CTextBox _txtDescription = new CTextBox { ID = "txtDescription", CssClassExtra = "width100pc", TextMode = TextBoxMode.MultiLine };

        #endregion

        #region Life cycle

        public FrmBoard()
        {
            Init += FrmBoard_Init;
        }

        void FrmBoard_Init(object sender, EventArgs e)
        {

            string strIDBoard = GetRequestParm(Context, "idBoard");
            if (String.IsNullOrEmpty(strIDBoard) == false)
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

        #endregion

        #region UI Events

        void btnAddBoard_Click(object sender, EventArgs e)
        {
            Board board = Boards.Current.Boards_SetBoard(0, _txtTitle.Text, _txtDescription.Text, CurrentUser.Name);
            _idBoard = board.IDBoard;

            Response.Redirect(string.Format("{0}?idBoard={1}", "FrmBoard", _idBoard));
        }

        #endregion

        #region Private methods

        private void FrmBoard_InitIndex()
        {
            Title = "Boards";

            List<Board> boards = Boards.Current.Boards_GetListForUser(CurrentUser.Name);

            foreach (Board board in boards)
            {
                var pnlBoardSelector = new Panel { CssClass = "boardBanner" };
                var lnkTitle = new HyperLink
                {
                    NavigateUrl = string.Format("{0}?idBoard={1}", "FrmBoard", board.IDBoard),
                };
                var lblTitle = new CLabel
                {
                    Text = board.Title,
                    CssClass = "title",
                };
                lnkTitle.Controls.Add(lblTitle);
                var pnlDescription = new Panel();
                var lblDescription = new CLabel
                {
                    Text = board.Description,
                    CssClass = "description",
                };
                pnlDescription.Controls.Add(lblDescription);
                pnlBoardSelector.Controls.Add(lnkTitle);
                pnlBoardSelector.Controls.Add(pnlDescription);
                Controls.Add(pnlBoardSelector);
            }

            var pnlBoardAdd = new Panel { CssClass = "boardBanner" };
            var btnAddBoard = new CButton { ID = "btnAddBoard", Text = "AddBoard" };
            btnAddBoard.Click += btnAddBoard_Click;
            pnlBoardAdd.Controls.Add(FormUtils.CreatePanel(_txtTitle, "formRow"));
            _txtTitle.PlaceHolder = "Title";
            pnlBoardAdd.Controls.Add(FormUtils.CreatePanel(_txtDescription, "formRow"));
            _txtDescription.PlaceHolder = "Description";
            pnlBoardAdd.Controls.Add(FormUtils.CreatePanel(btnAddBoard, "formRow"));
            Controls.Add(pnlBoardAdd);

        }

        private void FrmBoard_InitBoard()
        {
            Board board = Boards.Current.Board_GetByIDBoard(_idBoard);

            Title = board.Title;

            CardBoardControl cardBoardControl = new CardBoardControl();
            cardBoardControl.ID = "ctrCardBoard";
            cardBoardControl.IDBoard = board.IDBoard;
            cardBoardControl.UserName = CurrentUser.Name;
            Controls.Add(cardBoardControl);

            ChatControl chatControl = new ChatControl();
            chatControl.ID = "ctrChat";
            chatControl.IDMessageBoard = string.Format("CardBoard_{0}", board.IDBoard);
            chatControl.UserName = CurrentUser.Name;
            Controls.Add(chatControl);
        }

        private string GetRequestParm(HttpContext context, string parm)
        {
            foreach (string key in context.Request.Params.AllKeys)
            {
                if (string.IsNullOrEmpty(key) == false && key.EndsWith(parm))
                {
                    return context.Request.Params[key];
                }
            }
            return string.Empty;
        }

        #endregion
    }
}
