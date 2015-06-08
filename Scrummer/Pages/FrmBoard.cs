using System;
using Scrummer.Controls;

namespace Scrummer.Pages
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
            var lblTest = new CLabel { Text = "Hello World", Tag = "h2" };
            Controls.Add(lblTest);

            ChatControl chatControl = new ChatControl();
            chatControl.ID = "ctrChat";
            chatControl.IDBoard = _idBoard;
            chatControl.UserName = CurrentUser.Name;
            Controls.Add(chatControl);
        }
    }
}
