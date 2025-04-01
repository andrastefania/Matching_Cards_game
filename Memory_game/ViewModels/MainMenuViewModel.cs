using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Memory_game.Commands;
using System.Windows.Input;
using System.Windows;
using Memory_game.Models;
using Memory_game.Views;
using Memory_game.Services;

namespace Memory_game.ViewModels
{
    public class MainMenuViewModel : INotifyPropertyChanged
    {
        public ICommand SelectCategoryCommand { get; }
        public ICommand NewGameCommand { get; }
        public ICommand OpenGameCommand { get; }
        public ICommand SaveGameCommand { get; }
        public ICommand ViewStatsCommand { get; }
        public ICommand ExitCommand { get; }
        public ICommand StandardBoardCommand { get; }
        public ICommand CustomBoardCommand { get; }
        public ICommand AboutCommand { get; }
        public ICommand SetCustomBoardCommand { get; }
        public ICommand OpenSavedGameCommand { get; }



        private User _currentUser;

        public MainMenuViewModel(User currentUser)
        {
            _currentUser = currentUser;
            SelectCategoryCommand = new RelayCommand(OpenCategorySelection);
            NewGameCommand = new RelayCommand(StartNewGame);
            OpenGameCommand = new RelayCommand(OpenGame);
            SaveGameCommand = new RelayCommand(SaveGame);
            ViewStatsCommand = new RelayCommand(OpenStats);
            ExitCommand = new RelayCommand(ReturnToLogin);
            AboutCommand = new RelayCommand(ShowAbout);

            StandardBoardCommand = new RelayCommand(() => SetBoardSize(4, 4));
            //CustomBoardCommand = new RelayCommand(OpenCustomSizeDialog);
            //OpenSavedGameCommand = new RelayCommand(OpenSavedGame, CanLoadSavedGame);
            //SelectCategoryCommand = new RelayCommand(OpenCategorySelection);
            OpenSavedGameCommand = new RelayCommand(OpenSavedGame, CanLoadSavedGame);


            CustomBoardCommand = new RelayCommand(() =>
            {
                var window = new BoardSettingsView
                {
                    DataContext = new BoardSettingsViewModel()
                };
                window.ShowDialog(); // or Show() if you prefer
            });

            AboutCommand = new RelayCommand(ShowAbout);
            CommandManager.InvalidateRequerySuggested();
        }

        private void StartNewGame() { /* navigate to GameView */ }
        
        private void OpenGame()
        {
            var gameView = new GameView
            {
                DataContext = new GameViewModel(_currentUser) // Pass the user if needed
            };
            gameView.Show();

            Application.Current.Windows
                .OfType<Window>()
                .FirstOrDefault(w => w is Views.MainMenuView)
                ?.Close();
        }
        private void OpenCategorySelection()
        {
            var window = new CategorySelectionView
            {
                DataContext = new CategorySelectionViewModel()
            };

            window.ShowDialog();
            Application.Current.Windows
            .OfType<Window>()
            .FirstOrDefault(w => w is GameView)
            ?.Close();
        }




        private void SaveGame() { /* save current game */ }
        //private void OpenStats() { /* show statistics */ }
        private void SetBoardSize(int rows, int cols) { /* save board config */ }

        private void ShowAbout()
        {
            MessageBox.Show("Nume: Stoica Andra\nEmail: andra.stoica@student.unitbv.ro\nGrupa: 10LF234\nSpecializare: INFORMATICA",
                            "About", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));


        #region Methods
        private void CloseMenu()
        {
            Application.Current.Windows
                .OfType<Window>()
                .FirstOrDefault(w => w is MainMenuView)
                ?.Close();
        }
        private void LoadGame()
        {
            var gameService = new GameService();
            var state = gameService.LoadGame(_currentUser.Username);

            if (state != null)
            {
                var gameView = new GameView();
                gameView.DataContext = new GameViewModel(_currentUser, state);
                gameView.Show();
                CloseMenu();
            }
        }

        //private void OpenSavedGame()
        //{
        //    var gameService = new GameService();
        //    var state = gameService.LoadGame(_currentUser.Username);

        //    if (state != null)
        //    {
        //        var gameView = new GameView
        //        {
        //            DataContext = new GameViewModel(_currentUser, state)
        //        };
        //        gameView.Show();
        //        CloseMenu();
        //    }
        //}

        private void OpenSavedGame()
        {
            var gameService = new GameService();
            var state = gameService.LoadGame(_currentUser.Username);

            if (state != null)
            {
                var gameView = new GameView
                {
                    DataContext = new GameViewModel(_currentUser, state)
                };
                gameView.Show();
                CloseMenu();
            }
            else
            {
                MessageBox.Show("No saved game found.");
            }
            
        }


        private bool CanLoadSavedGame()
        {
            return GameService.HasSavedGame(_currentUser.Username);
        }


        private void OpenStats()
        {
            var statsWindow = new StatisticsView
            {
                DataContext = new StatisticsViewModel(_currentUser)
            };
            statsWindow.ShowDialog(); 
        }

        private void ReturnToLogin()
        {
            var loginView = new LoginView
            {
                DataContext = new LoginViewModel()
            };
            loginView.Show();

            Application.Current.Windows
                .OfType<Window>()
                .FirstOrDefault(w => w is MainMenuView)
                ?.Close();
        }


        #endregion
    }

}
