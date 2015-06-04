using System;
using System.Collections.Generic;
using Scrummer.Code.Entities;

namespace Scrummer.Code.BusinessLogic
{
    public class Users
    {
        #region declarations

        private static Users _currentInstance = null;

        private List<User> _users = new List<User>();

        #endregion

        #region Properties

        public static Users Current
        {
            get
            {
                if (_currentInstance == null)
                {
                    _currentInstance = new Users();
                }
                return _currentInstance;
            }
            set { _currentInstance = value; }
        }

        #endregion

        #region Public methods

        public User User_GetByName(string name)
        {
            name=name.ToLower();
            foreach (User userAux in _users)
            {
                if (name.CompareTo(userAux.Name.ToLower()) == 0)
                {
                    return userAux;
                }
            }
            return null;
        }

        public User User_GetByEmail(string email)
        {
            email = email.ToLower();
            foreach (User userAux in _users)
            {
                if (email.CompareTo(userAux.Email.ToLower()) == 0)
                {
                    return userAux;
                }
            }
            return null;
        }

        public User User_GetByNameOrEmail(string name, string email)
        {
            name = name.ToLower();
            email = email.ToLower();
            foreach (User userAux in _users)
            {
                if (name.CompareTo(userAux.Name.ToLower()) == 0 ||
                    email.CompareTo(userAux.Email.ToLower()) == 0)
                {
                    return userAux;
                }
            }
            return null;
        }

        public User User_Set(string name, string email, string password)
        {
            User user = null;
            bool isNew = false;
            lock (_users)
            {
                user = User_GetByName(name);
                if (user == null) { user = User_GetByEmail(name); }
                if (user == null) { user = new User(); isNew = true; }

                user.Name = name;
                user.Email = email;
                if (string.IsNullOrEmpty(password) == false)
                {
                    user.PasswordSalt = CryptoUtils.GetCryptoToken();
                    user.PasswordHash = CryptoUtils.GetHashedPassword(password, user.PasswordSalt);
                }

                if (isNew) { _users.Add(user); }
            }
            return user;
        }

        public bool User_Authenticate(string nameOrMail, string password)
        {
            User user = User_GetByNameOrEmail(nameOrMail, nameOrMail);
            if (user == null) { return false; }

            string passwordHash = CryptoUtils.GetHashedPassword(password, user.PasswordSalt);
            if (passwordHash != user.PasswordHash) { return false; }

            return true;
        }

        #endregion
    }
}