using Newtonsoft.Json;
using StendenClicker.Library.PlayerControls;
using System;
using System.Threading.Tasks;
using Windows.Storage;

namespace StendenClicker.Library
{
	public class LocalPlayerData
	{
		public static async Task<Player> LoadLocalPlayerDataAsync() => await LoadLocalData<Player>("player_data.json");
		public static async Task SaveLocalPlayerData(Player player) => await SaveLocalData(player, "player_data.json");
		public static async Task RemoveLocalPlayerData() => await RemoveFile("player_data.json");

		public static async Task<T> LoadLocalData<T>(string filename) where T : new()
		{
			StorageFolder installedLocation = Windows.Storage.ApplicationData.Current.LocalFolder;
			StorageFile file;
			if (!await FileExists(filename))
			{
				file = await installedLocation.CreateFileAsync(filename);
				await FileIO.WriteTextAsync(file, JsonConvert.SerializeObject(new T()));
			}

			file = await installedLocation.GetFileAsync(filename);
			return JsonConvert.DeserializeObject<T>(await FileIO.ReadTextAsync(file));
		}

		private static async Task<bool> FileExists(string filename)
		{
			StorageFolder installedLocation = Windows.Storage.ApplicationData.Current.LocalFolder;
			return await installedLocation.TryGetItemAsync(filename) is StorageFile;
		}

		public static async Task SaveLocalData<T>(T data, string filename)
		{
			StorageFolder installedLocation = Windows.Storage.ApplicationData.Current.LocalFolder;
			StorageFile file;
			if (await FileExists(filename))
			{
				file = await installedLocation.GetFileAsync(filename);
			}
			else
			{
				file = await installedLocation.CreateFileAsync(filename);
			}

			await FileIO.WriteTextAsync(file, JsonConvert.SerializeObject(data));
		}

		public static async Task RemoveFile(string filename)
		{
			StorageFolder installedLocation = Windows.Storage.ApplicationData.Current.LocalFolder;
			StorageFile file;
			if (await FileExists(filename))
			{
				file = await installedLocation.GetFileAsync(filename);
				await file.DeleteAsync();
			}
		}
	}
}
