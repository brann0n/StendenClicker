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
			if (state == null)
			{
				Dictionary<string, string> parameters = new Dictionary<string, string>
				{
					{ "device_id", DeviceID }
				};

				var response = await RestHelper.GetRequestAsync("api/player/get", parameters);
				if (response.StatusCode == HttpStatusCode.OK)
				{
					state = RestHelper.ConvertJsonToObject<Models.DatabaseModels.Player>(response.Content);
					LocalPlayerData.SaveLocalPlayerData(state);
				}
				else
				{
					state = LocalPlayerData.LoadLocalPlayerData();
				}
			}

			return state;
		}

		/// <summary>
		/// uploads the current player state to the server
		/// </summary>
		/// <param name="player"></param>
		public async void SetPlayerState(Player player)
		{
			state = player;
			Models.DatabaseModels.Player dbPlayer = player;
			var response = await RestHelper.PostRequestAsync("api/player/set", dbPlayer);
			if (response.StatusCode == HttpStatusCode.OK)
			{
				LocalPlayerData.SaveLocalPlayerData(state);
			}
			else
			{
				throw new Exception($"Couldn't set the player state... Api error: [{response.StatusCode}] {response.ErrorMessage}");
			}
		}

		public async void CreateUser(string username, string connectionId)
		{
			Player player = new Player
			{
				connectionId = connectionId,
				deviceId = Player.GetMachineKey(),
				Username = username,
				UserId = Guid.NewGuid()
			};

			
		}
	}
}
