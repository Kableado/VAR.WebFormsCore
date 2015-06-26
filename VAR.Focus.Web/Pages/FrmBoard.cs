using System;
using VAR.Focus.Web.Controls;

namespace VAR.Focus.Web.Pages
{
    public class FrmBoard : PageCommon
    {
        private int _idBoard = 0;

        public FrmBoard()
        {
            Init += FrmBoard_Init;
        }

        void FrmBoard_Init(object sender, EventArgs e)
        {
            Title = "Board";

            CardBoardControl cardBoardControl = new CardBoardControl();
            cardBoardControl.ID = "ctrCardBoard";
            cardBoardControl.IDBoard = _idBoard;
            cardBoardControl.UserName = CurrentUser.Name;
            Controls.Add(cardBoardControl);

            ChatControl chatControl = new ChatControl();
            chatControl.ID = "ctrChat";
            chatControl.IDBoard = _idBoard;
            chatControl.UserName = CurrentUser.Name;
            Controls.Add(chatControl);
        }
    }
}
