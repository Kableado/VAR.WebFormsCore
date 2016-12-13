using System.Collections.Generic;
using System.Linq;
using VAR.Focus.Web.Code.Entities;

namespace VAR.Focus.Web.Code.BusinessLogic
{
    public class Groups
    {
        #region declarations

        private static Groups _currentInstance = null;

        private List<Group> _groups = new List<Group>();

        private List<GroupMember> _groupMembers = new List<GroupMember>();

        #endregion

        #region Properties

        public static Groups Current
        {
            get
            {
                if (_currentInstance == null)
                {
                    _currentInstance = new Groups();
                }
                return _currentInstance;
            }
            set { _currentInstance = value; }
        }

        #endregion

        #region Public methods

        public Group Group_GetByName(string name)
        {
            name = name.ToLower();
            foreach (Group groupAux in _groups)
            {
                if (name.CompareTo(groupAux.Name.ToLower()) == 0)
                {
                    return groupAux;
                }
            }
            return null;
        }

        public Group Group_Set(string name, string description)
        {
            Group group = null;
            bool isNew = false;
            lock (_groups)
            {
                group = Group_GetByName(name);
                if (group == null) { group = new Group(); isNew = true; }

                group.Name = name;
                group.Description = description;

                if (isNew) { _groups.Add(group); }

                SaveData();
            }
            return group;
        }

        public List<string> GroupMember_GetGroupNamesByUser(string userName)
        {
            List<string> groupNames = _groupMembers.Select(groupMember => groupMember.GroupName).ToList();
            return groupNames;
        }

        public List<string> GroupMember_GetUserNamesByGroup(string groupName)
        {
            List<string> userNames = _groupMembers.Select(groupMember => groupMember.UserName).ToList();
            return userNames;
        }
        
        public GroupMember GroupMember_Set(string groupName, string userName)
        {
            string groupNameLower = groupName.ToLower();
            string userNameLower = userName.ToLower();
            GroupMember groupMember = null;
            bool isNew = false;
            lock (_groups)
            {
                groupMember = _groupMembers.FirstOrDefault(x => (
                    x.GroupName.ToLower() == groupNameLower &&
                    x.UserName.ToLower() == userNameLower));

                if (groupMember == null) { groupMember = new GroupMember(); isNew = true; }

                groupMember.GroupName = groupName;
                groupMember.UserName = userName;

                if (isNew) { _groupMembers.Add(groupMember); }

                SaveData();
            }
            return groupMember;
        }

        #endregion

        #region Life cycle

        public Groups()
        {
            LoadData();
        }

        #endregion

        #region Private methods

        #region Persistence

        private const string GroupsPersistenceFile = "groups";
        private const string GroupMembersPersistenceFile = "groupMembers";

        private void LoadData()
        {
            _groups = Persistence.LoadList<Group>(GroupsPersistenceFile);
            _groupMembers = Persistence.LoadList<GroupMember>(GroupMembersPersistenceFile);
        }

        private void SaveData()
        {
            Persistence.SaveList(GroupsPersistenceFile, _groups);
            Persistence.SaveList(GroupMembersPersistenceFile, _groupMembers);
        }

        #endregion

        #endregion
    }
}