using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VAR.Focus.Web.Code.Entities;

namespace VAR.Focus.Web.Code.BusinessLogic
{
    public class Boards
    {
        #region Declarations

        private static Boards _currentInstance = null;

        private List<Board> _boards = new List<Board>();
        private int _lastIDBoard=0;

        #endregion

        #region Properties

        public static Boards Current
        {
            get
            {
                if (_currentInstance == null)
                {
                    _currentInstance = new Boards();
                }
                return _currentInstance;
            }
            set { _currentInstance = value; }
        }

        #endregion

        #region Life cycle

        public Boards()
        {
            LoadData();
        }

        #endregion

        #region Public methods

        public List<Board> Boards_GetListForUser(string userName)
        {
            // FIXME: filter by permissions
            return _boards;
        }

        public Board Board_GetByIDBoard(int idBoard)
        {
            foreach (Board board in _boards)
            {
                if (board.IDBoard == idBoard)
                {
                    return board;
                }
            }
            return null;
        }

        public Board Boards_SetBoard(int idBoard, string title, string description, string userName)
        {
            DateTime currentDate = DateTime.UtcNow;
            Board board;
            if (idBoard == 0)
            {
                lock (this)
                {
                    _lastIDBoard++;
                    board = new Board();
                    board.IDBoard = _lastIDBoard;
                    board.Active = true;
                    board.CreatedBy = userName;
                    board.CreatedDate = currentDate;
                    _boards.Add(board);
                }
            }
            else
            {
                board = Board_GetByIDBoard(idBoard);
            }

            board.Title = title;
            board.Description = description;
            board.ModifiedBy = userName;
            board.ModifiedDate = currentDate;

            SaveData();

            return board;
        }

        #endregion

        #region Private methods

        #region Persistence

        private const string BoardsPersistenceFile = "boards";
        
        private void LoadData()
        {
            _boards = Persistence.LoadList<Board>(BoardsPersistenceFile);
            _lastIDBoard = 0;
            foreach (Board board in _boards)
            {
                if (board.IDBoard > _lastIDBoard)
                {
                    _lastIDBoard = board.IDBoard;
                }
            }
        }

        private void SaveData()
        {
            Persistence.SaveList(BoardsPersistenceFile, _boards);
        }

        #endregion

        #endregion
    }
}
