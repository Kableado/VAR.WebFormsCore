using System;

namespace VAR.Focus.Web.Code.Entities
{
    public class GroupMember
    {
        public string UserName { get; set; }
        public string GroupName { get; set; }

        private bool _active = true;
        public bool Active { get { return _active; } set { _active = value; } }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
