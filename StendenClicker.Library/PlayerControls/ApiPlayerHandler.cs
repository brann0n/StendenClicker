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
					await LocalPlayerData.SaveLocalPlayerData(state);
				}
				else
				{
					state = await LocalPlayerData.LoadLocalPlayerDataAsync();
					if (response.StatusCode == HttpStatusCode.InternalServerError)
					{
						if (response.StatusDescription == "No player found with this deviceid")
						{
							//remove the local player state
							await LocalPlayerData.RemoveLocalPlayerData();
						}
					}				
				}
			}

			return state;
		}

		/// <summary>
		/// May not work in UWP because of the awaiters
		/// </summary>
		/// <param name="DeviceId"></param>
		/// <returns></returns>
		public Player GetPlayer(string DeviceId)
		{
			return GetPlayerStateAsync(DeviceId).GetAwaiter().GetResult();
		}

		/// <summary>
		/// uploads the current player state to the server
		/// </summary>
		/// <param name="player"></param>
		public async void SetPlayerStateAsync(Player player)
		{
			state = player;
			Models.DatabaseModels.Player dbPlayer = player;
			var response = await RestHelper.PostRequestAsync("api/player/set", dbPlayer);
			if (response.StatusCode == HttpStatusCode.OK)
			{
				await LocalPlayerData.SaveLocalPlayerData(state);
			}
			else
			{
				//save the current player state, and then throw an exception
				await LocalPlayerData.SaveLocalPlayerData(state);
				throw new Exception($"Couldn't set the player state... Api error: [{response.StatusCode}] {response.ErrorMessage}");
			}
		}

		/// <summary>
		/// Creates a new user instance and posts the data to the API, which then is saved in the database.
		/// </summary>
		/// <param name="username"></param>
		/// <param name="connectionId"></param>
		public async Task CreateUser(string username, string DeviceId)
		{
			Models.DatabaseModels.Player player = new Models.DatabaseModels.Player
			{
				DeviceId = DeviceId,
				PlayerName = username,
				PlayerGuid = Guid.NewGuid().ToString()
			};

			var response = await RestHelper.PostRequestAsync("api/player/create", player);
			if (response.StatusCode == HttpStatusCode.OK)
			{
				//after creating a new user, also store that new player
				await LocalPlayerData.SaveLocalPlayerData(player);
			}
			else
			{
				await LocalPlayerData.SaveLocalPlayerData(player);
				throw new Exception($"Couldn't create the player... Api error: [{response.StatusCode}] {response.ErrorMessage}");
			}
		}
	}
}
