using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using VAR.Focus.Web.Code;
using VAR.Focus.Web.Code.BusinessLogic;
using VAR.Focus.Web.Code.Entities;
using VAR.Focus.Web.Controls;

namespace VAR.Focus.Web.Pages
{
    public class FrmBoard : PageCommon
    {
        #region Declarations

        private int _idBoard = 0;

        private CTextBox _txtTitle = new CTextBox { ID = "txtTitle", CssClassExtra="width100pc", AllowEmpty = false };
        private CTextBox _txtDescription = new CTextBox { ID = "txtDescription", CssClassExtra = "width100pc", TextMode = TextBoxMode.MultiLine };

        #endregion

        #region Life cycle

        public FrmBoard()
        {
            Init += FrmBoard_Init;
        }

        void FrmBoard_Init(object sender, EventArgs e)
        {
            string strIDBoard = Context.GetRequestParm("idBoard");
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
            if (FormUtils.Controls_AreValid(Controls) == false) { return; }

            Board board = Boards.Current.Boards_SetBoard(0, _txtTitle.Text, _txtDescription.Text, CurrentUser.Name);
            _idBoard = board.IDBoard;

            Response.Redirect(string.Format("{0}?idBoard={1}", "FrmBoard", _idBoard));
        }

        #endregion

        #region Private methods

        private Panel BoardSelector_Create(Board board)
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

            CardBoardControl cardBoardControl = new CardBoardControl
            {
                ID = "ctrCardBoard",
                IDBoard = board.IDBoard,
                UserName = CurrentUser.Name,
            };
            Controls.Add(cardBoardControl);

            ChatControl chatControl = new ChatControl
            {
                ID = "ctrChat",
                IDMessageBoard = string.Format("CardBoard_{0}", board.IDBoard),
                UserName = CurrentUser.Name,
            };
            Controls.Add(chatControl);
        }
        
        #endregion
    }
}
