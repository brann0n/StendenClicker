using StendenClicker.Library.Models;
using System;
using System.Management;

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

        

        public string GetUsername()
        {
            return null;
        }

        

        //public static string GetMachineKey()
        //{
            
        //       ManagementObject os = new ManagementObject("Win32_OperatingSystem=@");
        //    return (string)os["SerialNumber"];
        //}

        public static bool IsPlayerObjectEmpty(Player player)
		{
            if (player == null) return true;

            if (player.UserId == Guid.Empty) return true;

            if (string.IsNullOrEmpty(player.Username)) return true;

            if (string.IsNullOrEmpty(player.deviceId)) return true;

            return false;
		}


        public static implicit operator Player(Models.DatabaseModels.Player player)
		{
			return new Player
			{
				connectionId = player.ConnectionId,
				deviceId = player.DeviceId,
				Username = player.PlayerName,
                UserId = Guid.Parse(player.PlayerGuid),
                State = new PlayerState(),
                Wallet = new PlayerCurrency()
			};
		}

        public int getDamageFactor()
        {
            return 1;
        }
    }
}
