using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Memory_game.Models;
using Memory_game.Commands;
using System.Windows;
using Memory_game.Views;
using System.ComponentModel;
using System.Windows.Threading;
using Memory_game.Services;
using System.Diagnostics;

namespace Memory_game.ViewModels
{
    public class GameViewModel : INotifyPropertyChanged
    {
        private User _currentUser;
        public ObservableCollection<Card> Cards { get; set; }
        private Card _firstFlippedCard;
        private bool _isBusyFlipping = false;
        private bool _isGameOver;
        private Stopwatch _stopwatch;
        private double _totalTimeInSeconds;



        private DispatcherTimer _timer;
        private TimeSpan _timeRemaining;
        private bool incremented = false;

        public string TimeDisplay => _timeRemaining.ToString(@"mm\:ss");

        private bool _isTimeUp;
        public bool IsTimeUp
        {
            get => _isTimeUp;
            set
            {
                _isTimeUp = value;
                OnPropertyChanged(nameof(IsTimeUp));
            }
        }


        public bool IsGameOver
        {
            get => _isGameOver;
            set
            {
                _isGameOver = value;
                OnPropertyChanged(nameof(IsGameOver));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }



        #region Commands

        public ICommand FlipCardCommand { get; }
        public ICommand PlayAgainCommand { get; }
        public ICommand ExitCommand { get; }
        public ICommand SaveGameCommand { get; }
        public ICommand AboutCommand { get; }
        public ICommand NewGameCommand { get; }


        #endregion

        public GameViewModel(User user)
        {
            _currentUser = user;
            Cards = new ObservableCollection<Card>();
            FlipCardCommand = new RelayCommand<Card>(FlipCard);
            PlayAgainCommand = new RelayCommand(PlayAgain);
            ExitCommand = new RelayCommand(ReturnToMainMenu);
            SaveGameCommand = new RelayCommand(SaveGame);
            AboutCommand = new RelayCommand(ShowAbout);
            NewGameCommand = new RelayCommand(PlayAgain);


            InitializeGame(); 
            SetupTimer(GameConfiguration.TimeLimitSeconds);
        }

        public GameViewModel(User user, GameState savedState)
        {
            _currentUser = user;
            Cards = new ObservableCollection<Card>(savedState.Cards);
            _timeRemaining = TimeSpan.FromSeconds(savedState.RemainingSeconds);

            FlipCardCommand = new RelayCommand<Card>(FlipCard);
            PlayAgainCommand = new RelayCommand(PlayAgain);
            ExitCommand = new RelayCommand(ReturnToMainMenu);
            SaveGameCommand = new RelayCommand(SaveGame);

            SetupTimer(_timeRemaining.TotalSeconds);
        }


        private void InitializeGame()
        {
            _firstFlippedCard = null;
            _isBusyFlipping = false;
            IsGameOver = false;
            IsTimeUp = false;

            int rows = GameConfiguration.Rows;
            int cols = GameConfiguration.Columns;
            int totalCards = rows * cols;

            
            var allImages = GameConfiguration.SelectedImages;

            var selectedImages = allImages.Take(totalCards / 2).ToList();

            var cardImages = selectedImages.Concat(selectedImages).ToList();
            var rng = new Random();
            cardImages = cardImages.OrderBy(_ => rng.Next()).ToList();

            Cards = new ObservableCollection<Card>();
            for (int i = 0; i < cardImages.Count; i++)
            {
                Cards.Add(new Card
                {
                    Id = (i / 2).ToString(),
                    ImagePath = cardImages[i],
                    IsFlipped = false,
                    IsMatched = false
                });
            }

            _timeRemaining = TimeSpan.FromSeconds(GameConfiguration.TimeLimitSeconds);

            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += TimerTick;
            _timer.Start();

            OnPropertyChanged(nameof(TimeDisplay));
        }


        #region Methods
        private async void FlipCard(Card card)
        {
            if (_isBusyFlipping || card.IsFlipped || card.IsMatched)
                return;

            card.IsFlipped = true;

            if (_firstFlippedCard == null)
            {
                _firstFlippedCard = card;
                return;
            }

            _isBusyFlipping = true;

            if (_firstFlippedCard.ImagePath == card.ImagePath)
            {
                await Task.Delay(500);
                _firstFlippedCard.IsMatched = true;
                card.IsMatched = true;
            }
            else
            {
                await Task.Delay(500);
                _firstFlippedCard.IsFlipped = false;
                card.IsFlipped = false;
            }

            _firstFlippedCard = null;
            _isBusyFlipping = false;

            if (Cards.All(c => c.IsMatched) && incremented==false)  // Game ends if all cards are matched
            {
                _timer.Stop();
                _stopwatch.Stop();
                IsTimeUp = false;
                IsGameOver = true;
                _currentUser.GamesPlayed++;
                _currentUser.GamesWon++;
               incremented = true;
                SaveUserStats();
            }

        }




        private async void PlayAgain()
        {
            var newGame = new GameView();
            newGame.DataContext = new GameViewModel(_currentUser);
            newGame.Show();

            await Task.Delay(100); 

            Application.Current.Windows
                .OfType<Window>()
                .FirstOrDefault(w => w is GameView)
                ?.Close();
        }

        private void SetupTimer(double seconds)
        {
            _totalTimeInSeconds = seconds;
            _stopwatch = Stopwatch.StartNew();

            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += TimerTick;
            _timer.Start();
        }
        private void TimerTick(object sender, EventArgs e)
        {
            var elapsed = _stopwatch.Elapsed.TotalSeconds;
            var remaining = _totalTimeInSeconds - elapsed;

            if (remaining <= 0 && incremented==false)
            {
                _timer.Stop();
                _stopwatch.Stop();
                _timeRemaining = TimeSpan.Zero;
                OnPropertyChanged(nameof(TimeDisplay));

                IsTimeUp = true;
                IsGameOver = true;
                _currentUser.GamesPlayed++;
                SaveUserStats();
                incremented = true;
                return;
            }

            _timeRemaining = TimeSpan.FromSeconds(remaining);
            OnPropertyChanged(nameof(TimeDisplay));

            if (Cards.All(c => c.IsMatched) && incremented==false)  // Game ends if all cards are matched
            {
                
                _timer.Stop();
                _stopwatch.Stop();
                IsTimeUp = false;
                IsGameOver = true;
                _currentUser.GamesPlayed++;
                _currentUser.GamesWon++;
                incremented = true;
                SaveUserStats();
            }
        }



        private void SaveGame()
        {
            var service = new GameService();

            var state = new GameState
            {
                Cards = Cards.ToList(),
                RemainingSeconds = (int)_timeRemaining.TotalSeconds
            };

            service.SaveGame(state, _currentUser.Username);

            var mainMenu = new MainMenuView
            {
                DataContext = new MainMenuViewModel(_currentUser)
            };
            mainMenu.Show();

            Application.Current.Windows
                .OfType<Window>()
                .FirstOrDefault(w => w is GameView)
                ?.Close();
        }



        private void SaveUserStats()
        {
            var userService = new UserService();
            userService.UpdateUser(_currentUser); 
        }

        private void ReturnToMainMenu()
        {
            var mainMenu = new MainMenuView
            {
                DataContext = new MainMenuViewModel(_currentUser)
            };
            mainMenu.Show();

            Application.Current.Windows
                .OfType<Window>()
                .FirstOrDefault(w => w is GameView)
                ?.Close();
        }

        private void ShowAbout()
        {
            MessageBox.Show("Nume: Stoica Andra\nEmail: andra.stoica@student.unitbv.ro\nGrupa: 10LF234\nSpecializare: INFORMATICA",
                            "About", MessageBoxButton.OK, MessageBoxImage.Information);
        }


        #endregion
    }
}
