using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Memory_game.Commands;

namespace Memory_game.ViewModels
{
    public class CreateUserViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<string> AvatarPaths { get; set; }
        private int _selectedIndex;
        private string _username;

        public event PropertyChangedEventHandler PropertyChanged;

        public string Username
        {
            get => _username;
            set { _username = value; OnPropertyChanged(); }
        }

        public string SelectedAvatar => AvatarPaths[_selectedIndex];

        public ICommand NextCommand { get; }
        public ICommand PreviousCommand { get; }
        public ICommand CreateUserCommand { get; }

        public CreateUserViewModel()
        {
            AvatarPaths = new ObservableCollection<string>
            {
                "Images/avatar1.jpeg",
                "Images/avatar2.jpeg",
                "Images/avatar3.jpeg",
                "Images/avatar4.jpeg",
                "Images/avatar5.jpeg",
                "Images/avatar6.jpeg",
                "Images/avatar7.jpeg",
                "Images/avatar8.jpeg",
                "Images/avatar9.jpeg",
                "Images/avatar10.jpeg"
            };

            _selectedIndex = 0;

            NextCommand = new RelayCommand(() =>
            {
                _selectedIndex = (_selectedIndex + 1) % AvatarPaths.Count;
                OnPropertyChanged(nameof(SelectedAvatar));
            });

            PreviousCommand = new RelayCommand(() =>
            {
                _selectedIndex = (_selectedIndex - 1 + AvatarPaths.Count) % AvatarPaths.Count;
                OnPropertyChanged(nameof(SelectedAvatar));
            });

            CreateUserCommand = new RelayCommand(() =>
            {
                
            });
        }

        protected void OnPropertyChanged([CallerMemberName] string name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
