using EWallet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EWallet.Stores
{
    /// <summary>
    /// Хранилище пользователей.
    /// </summary>
    public class UserStore
    {
        private User currentUser;

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

        public bool IsLoggedIn => currentUser != null;
        public bool IsLoggedOut => currentUser == null;

        public event Action CurrentUserChanged;

        public void Logout() => CurrentUser = null;
    }
}
