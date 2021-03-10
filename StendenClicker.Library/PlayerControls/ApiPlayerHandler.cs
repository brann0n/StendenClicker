using System;

namespace StendenClicker.Library.PlayerControls
{
	public class ApiPlayerHandler
	{
		private Player state;

		/// <summary>
		/// does a webrequest to get the current player state
		/// </summary>
		/// <param name="userid"></param>
		/// <returns></returns>
		public Player getPlayerState(Guid userid)
		{
			//todo: make webrequest OR signalR code to backup and retrieve this object
			return null;
		}

		/// <summary>
		/// uploads the current player state to the server
		/// </summary>
		/// <param name="player"></param>
		public void setPlayerState(Player player)
		{
			//todo: make webrequest OR signalR code to backup and retrieve this object
		}

		public Player getPlayer()
		{
			return state;
		}

	}

}
