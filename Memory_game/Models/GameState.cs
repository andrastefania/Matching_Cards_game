using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Memory_game.Models
{
    public class GameState
    {
        public List<Card> Cards { get; set; }
        public string Category { get; set; }
        public TimeSpan TimeElapsed { get; set; }
        public int Rows { get; set; }
        public int Columns { get; set; }
        public double RemainingSeconds { get; set; }
    }
}
