using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Memory_game.Models;
using Memory_game.Services;

namespace Memory_game.ViewModels
{
    public class StatisticsViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private User _currentUser;
        public string CurrentPlayerStats { get; set; }

        public ObservableCollection<UserStats> Leaderboard { get; set; }

        public StatisticsViewModel(User currentUser)
        {
            _currentUser = currentUser;

            var userService = new UserService();
            var users = userService.LoadUsers();

            Leaderboard = new ObservableCollection<UserStats>(
                users.Select(u => new UserStats
                {
                    Username = u.Username,
                    GamesPlayed = u.GamesPlayed,
                    GamesWon = u.GamesWon,
                    GamesLost = u.GamesPlayed - u.GamesWon
                })
                .OrderByDescending(u => u.GamesWon)
            );

            CurrentPlayerStats = $"You played {_currentUser.GamesPlayed} games, " +
                                 $"won {_currentUser.GamesWon}, " +
                                 $"lost {_currentUser.GamesPlayed - _currentUser.GamesWon}";
        }

        protected void OnPropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public class UserStats
    {
        public string Username { get; set; }
        public int GamesPlayed { get; set; }
        public int GamesWon { get; set; }
        public int GamesLost { get; set; }
    }
}
