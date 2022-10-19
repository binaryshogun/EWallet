using EWallet.Models;
using System;

namespace EWallet.Stores
{
    /// <summary>
    /// Хранилище пользователей.
    /// </summary>
    public class UserStore
    {
        #region Fields
        private User currentUser;
        #endregion

        #region Events
        public event Action CurrentUserChanged;
        #endregion

        #region Properties
        /// <summary>
        /// Текущий пользователь.
        /// </summary>
        /// <value>
        /// Хранит текущего пользователя в системе.
        /// </value>
        public User CurrentUser
        {
            get => currentUser;
            set
            {
                currentUser = value;
                CurrentUserChanged?.Invoke();
            }
        }

        public bool IsLoggedIn 
            => currentUser != null;
        public bool IsLoggedOut 
            => currentUser == null;
        #endregion

        #region Methods
        public void Logout() 
            => CurrentUser = null;
        #endregion
    }
}
