using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Memory_game.Models;
using System.Text.Json;


namespace Memory_game.Services
{
    public class UserService
    {
        private readonly string filePath;

        public UserService()
        {
            string appPath = AppDomain.CurrentDomain.BaseDirectory;
            string dataFolder = Path.Combine(appPath, "Data");

            if (!Directory.Exists(dataFolder))
                Directory.CreateDirectory(dataFolder);

            filePath = Path.Combine(dataFolder, "users.json");
        }

        public List<User> LoadUsers()
        {
            if (!File.Exists(filePath))
                return new List<User>();

            string json = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<List<User>>(json) ?? new List<User>();
        }

        public void SaveUsers(List<User> users)
        {
            string json = JsonSerializer.Serialize(users, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, json);
        }
        public void UpdateUser(User updatedUser)
        {
            var users = LoadUsers();
            var userIndex = users.FindIndex(u => u.Username == updatedUser.Username);
            if (userIndex >= 0)
            {
                users[userIndex] = updatedUser;
                SaveUsers(users);
            }
        }

    }
}
