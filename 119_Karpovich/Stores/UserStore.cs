using _119_Karpovich.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _119_Karpovich.Stores
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
            set => currentUser = value;
        }
    }
}
