using System;
using System.Collections.Generic;

namespace Memory_game.Models
{
    public static class GameConfiguration
    {
        public static int Rows { get; set; } = 4;
        public static int Columns { get; set; } = 4;
        public static int TimeLimitSeconds { get; set; } = 60;
        public static string SelectedCategory { get; set; } = "Anime";
        public static List<string> SelectedImages { get; set; } = new List<string>
        {
            "Images/anime1.jpeg", "Images/anime2.jpeg", "Images/anime3.png",
            "Images/anime4.jpeg", "Images/anime5.jpeg", "Images/anime6.jpeg",
            "Images/anime7.jpeg", "Images/anime8.jpeg"
        };

        public static User CurrentUser { get; set; }

    }
}
