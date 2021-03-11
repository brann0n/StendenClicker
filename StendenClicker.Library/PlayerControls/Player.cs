using StendenClicker.Library.Models;
using System;

namespace StendenClicker.Library.PlayerControls
{
    public class Player
    {
        /// <summary>
        /// Generated in the database
        /// </summary>
        public Guid UserId { get; set; }

        public string Username { get; set; }

        public PlayerCurrency Wallet { get; set; }

        /// <summary>
        /// This is the physical ID of your device (not MAC)
        /// </summary>
        public string deviceId { get; set; }

        public string connectionId { get; set; }

        public PlayerState State { get; set; }


        public string getUsername()
        {
            return null;
        }
    }
}
