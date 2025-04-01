using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Memory_game.Models;
using Memory_game.Services;
using Memory_game.Commands;
using Memory_game.Views;
using System.Windows;


namespace Memory_game.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        private UserService _userService;
        private Card _firstFlippedCard;

        #region Commands

        public ICommand PlayCommand { get; }
        public ICommand CreateUserCommand { get; }
        public ICommand DeleteUserCommand { get; }

        #endregion


        public ObservableCollection<User> Users { get; set; }

        private User _selectedUser;

        public User SelectedUser
        {
            get => _selectedUser;
            set
            {
                _selectedUser = value;
                OnPropertyChanged(nameof(SelectedUser));
                
                (DeleteUserCommand as RelayCommand)?.RaiseCanExecuteChanged();
                (PlayCommand as RelayCommand)?.RaiseCanExecuteChanged();
            }
        }


        public LoginViewModel()
        {
            _userService = new UserService();

            var userList = _userService.LoadUsers();
            Users = new ObservableCollection<User>(userList);
            PlayCommand = new RelayCommand(Play, () => SelectedUser != null);
            CreateUserCommand = new RelayCommand(CreateUser);
            DeleteUserCommand = new RelayCommand(DeleteUser, () => SelectedUser != null);

        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        #region Methods
        
        private void Play()
        {
            GameConfiguration.CurrentUser = SelectedUser;

            var mainMenu = new MainMenuView
            {
                DataContext = new MainMenuViewModel(SelectedUser)
            };
            mainMenu.Show();

            Application.Current.Windows
                .OfType<Window>()
                .FirstOrDefault(w => w is LoginView)
                ?.Close();
        }





        private void CreateUser()
        {
            var vm = new CreateUserViewModel();
            var window = new CreateUserView
            {
                DataContext = vm
            };

            if (window.ShowDialog() == true)
            {
                var newUser = new User
                {
                    Username = vm.Username,
                    ImagePath = vm.SelectedAvatar,
                    GamesPlayed = 0,
                    GamesWon = 0
                };

                Users.Add(newUser);
                _userService.SaveUsers(Users.ToList());
            }
        }

        private void DeleteUser()
        {
            if (SelectedUser == null)
                return;

            var result = MessageBox.Show(
                $"Are you sure you want to delete user '{SelectedUser.Username}'?",
                "Confirm Delete",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                Users.Remove(SelectedUser);
                SelectedUser = null;
                _userService.SaveUsers(Users.ToList());
            }
        }

        #endregion
    }
}
