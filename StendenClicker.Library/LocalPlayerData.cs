using Newtonsoft.Json;
using StendenClicker.Library.PlayerControls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace StendenClicker.Library
{
    public class LocalPlayerData
    {
        public static Player LoadLocalPlayerData() => LoadPlayerData<Player>("player_data.json");
        public static void SaveLocalPlayerData(Player player) => SavePlayerData("player_data.json", player);

        private static T LoadPlayerData<T>(string filename) where T : new()
        {
            var path = Path.Combine(Environment.CurrentDirectory, filename);
            if (!File.Exists(path))
            {
                using (var fs = File.Create(path))
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.Write(JsonConvert.SerializeObject(new T()));
                }
            }
            return JsonConvert.DeserializeObject<T>(File.ReadAllText(path));
        }

        private static void SavePlayerData(string filename, Player data)
        {
            var path = Path.Combine(Environment.CurrentDirectory, filename);
            File.WriteAllText(path, JsonConvert.SerializeObject(data));
        }
}
