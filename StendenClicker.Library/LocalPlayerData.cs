using Newtonsoft.Json;
using StendenClicker.Library.PlayerControls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Windows.Storage;

namespace StendenClicker.Library
{
    public class LocalPlayerData
    {
        public static Player LoadLocalPlayerData() => LoadLocalData<Player>("player_data.json");
        public static void SaveLocalPlayerData(Player player) => SaveLocalData(player, "player_data.json");     

        public static T LoadLocalData<T>(string filename) where T : new()
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

        public static void SaveLocalData<T>(T data, string filename)
        {
            var path = Path.Combine(Environment.CurrentDirectory, filename);
            File.WriteAllText(path, JsonConvert.SerializeObject(data));
        }
    }
}
