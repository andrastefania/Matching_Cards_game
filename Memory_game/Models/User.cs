using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Memory_game.Models
{
    public class User
    {
        public string Username { get; set; }
        public string ImagePath { get; set; }
        public int GamesPlayed { get; set; } = 0;
        public int GamesLost => GamesPlayed - GamesWon;
        public int GamesWon { get; set; } = 0;
    }
}
