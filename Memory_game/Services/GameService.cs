using Memory_game.Models;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Newtonsoft.Json;

namespace Memory_game.Services
{
    public class GameService
    {
        
        public static bool HasSavedGame(string username)
        {
            string path = $"SavedGames/{username}_game.json";
            return File.Exists(path);
        }

        public GameState LoadGame(string username)
        {
            string path = $"SavedGames/{username}_game.json";
            if (!File.Exists(path)) return null;

            var json = File.ReadAllText(path);
            return JsonConvert.DeserializeObject<GameState>(json);
        }

        public void SaveGame(GameState state, string username)
        {
            Directory.CreateDirectory("SavedGames");
            string path = $"SavedGames/{username}_game.json";
            var json = JsonConvert.SerializeObject(state);
            File.WriteAllText(path, json);
        }


    }
}
