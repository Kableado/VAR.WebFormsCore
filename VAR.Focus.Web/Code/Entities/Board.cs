using System;

namespace VAR.Focus.Web.Code.Entities
{
    public class Board
    {
        public int IDBoard { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }

        private bool _active = true;
        public bool Active { get { return _active; } set { _active = value; } }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; } 
    }
}
