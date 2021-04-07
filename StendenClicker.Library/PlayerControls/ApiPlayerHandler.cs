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
							return null;
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
		{//todo: implement the save player data, also add hero's to the player object.
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

		/// <summary>
		/// Checks if the username is taken in the database via the API.
		/// </summary>
		/// <param name="username"></param>
		public async Task<bool> IsUsernameAvailable(string username)
		{
			Dictionary<string, string> parameters = new Dictionary<string, string>
			{
				{ "username", username }
			};

			var response = await RestHelper.GetRequestAsync("api/player/isusernameavailable", parameters);
			if (response.StatusCode == HttpStatusCode.OK)
			{
				if (response.Content == "false")
				{
					return false;
				}
				return true;
			}
			else
			{
				throw new Exception($"Couldn't search for the player name... Api error: [{response.StatusCode}] {response.ErrorMessage}");
			}
		}

		/// <summary>
		///	Deletes the friendship if it excist
		/// </summary>
		/// <param name="guid1"></param>
		/// <param name="guid2"></param>
		public async Task DeleteFriendship(string guid1, string guid2)
		{
			List<string> PlayerGuids = new List<string>
			{
				{ guid1 },
				{ guid2 }
			};
			
			var response = await RestHelper.PostRequestAsync("api/player/deletefriendship", PlayerGuids);
			if (response.StatusCode != HttpStatusCode.OK)
			{
				throw new Exception($"Couldn't create the player... Api error: [{response.StatusCode}] {response.ErrorMessage}");
			}
		}
	}
}
