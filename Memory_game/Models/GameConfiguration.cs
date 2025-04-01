using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Memory_game.Models
{
    public static class GameConfiguration
    {
        public static int Rows { get; set; } = 4;
        public static int Columns { get; set; } = 4;
        public static int TimeLimitSeconds { get; set; } = 60;
        public static string SelectedCategory { get; set; } = "Vikings";
        public static string SelectedCategoryName { get; set; } = "Vikings";
        public static List<string> SelectedImages { get; set; } = new List<string>
        {
            "Images/skzoo1.jpeg","Images/skzoo2.jpeg","Images/skzoo3.jpeg",
                "Images/skzoo4.jpeg","Images/skzoo5.jpeg","Images/skzoo6.jpeg",
                "Images/skzoo7.jpeg","Images/skzoo8.jpeg"
        };

        public static User CurrentUser { get; set; }

    }
}
