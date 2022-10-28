using EWallet.Models;
using System;

namespace EWallet.Stores
{
    /// <summary>
    /// Хранилище пользователей.
    /// </summary>
    public sealed class UserStore
    {
        #region Fields
        private User currentUser;
        #endregion

        #region Events
        /// <summary>
        /// Событие при изменении текущего пользователя.
        /// </summary>
        public event Action CurrentUserChanged;
        #endregion

        #region Properties
        /// <summary>
        /// Текущий пользователь.
        /// </summary>
        public User CurrentUser
        {
            get => currentUser;
            set
            {
                currentUser = value;
                CurrentUserChanged?.Invoke();
            }
        }
        /// <summary>
        /// Указывает, что пользователь авторизован.
        /// </summary>
        public bool IsLoggedIn 
            => currentUser != null;
        /// <summary>
        /// Указывает, что пользователь не авторизован.
        /// </summary>
        public bool IsLoggedOut 
            => currentUser == null;
        #endregion

        #region Methods
        /// <summary>
        /// Сбрасывает текущего пользователя.
        /// </summary>
        public void Logout() 
            => CurrentUser = null;
        #endregion
    }
}
