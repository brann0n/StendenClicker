using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace StendenClicker.Library.PlayerControls
{
	public class ApiPlayerHandler
	{
		private Player state;

		/// <summary>
		/// does a webrequest to get the current player state
		/// </summary>
		/// <param name="DeviceID"></param>
		/// <returns></returns>
		public async Task<Player> GetPlayerStateAsync(string DeviceID)
		{
			//todo: make webrequest OR signalR code to backup and retrieve this object
			Player player;
			if (state == null)
			{
				Dictionary<string, string> parameters = new Dictionary<string, string>();
				parameters.Add("device_id", DeviceID);

				var response = await RestHelper.GetRequestAsync("api/player/get", parameters);
				if (response.StatusCode == HttpStatusCode.OK)
				{
					player = RestHelper.ConvertJsonToObject<Player>(response.Content);
					LocalPlayerData.SaveLocalPlayerData(player);
				}
				else
				{
					player = LocalPlayerData.LoadLocalPlayerData();
				}
				state = player;
			} else
            {
				player = state;
            }
			return player;
		}
		
		/// <summary>
		/// uploads the current player state to the server
		/// </summary>
		/// <param name="player"></param>
		public async void SetPlayerState(Player player)
		{
			//todo: make webrequest OR signalR code to backup and retrieve this object
			var response = await RestHelper.PostRequestAsync("api/player/set", player);
			if(response.StatusCode == HttpStatusCode.OK)
            {
				LocalPlayerData.SaveLocalPlayerData(player);
				state = player;
            } else
            {
				throw new Exception("Couldn't set the player state... Api error: [" + response.StatusCode + "] " + response.ErrorMessage);
            }
		}
	}
}
