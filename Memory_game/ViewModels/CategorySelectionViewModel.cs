using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using Memory_game.Commands;
using Memory_game.Views;
using Memory_game.Models;
using System.Linq;

namespace Memory_game.ViewModels
{
    public class CategorySelectionViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<string> CuteAnimalsImages { get; set; }
        public ObservableCollection<string> AnimeImages { get; set; }
        public ObservableCollection<string> FoodImages { get; set; }


        private int _cuteAnimalsIndex;
        private int _animeIndex;
        private int _foodIndex;


        public string CurrentAnimalImage => CuteAnimalsImages[_cuteAnimalsIndex];
        public string CurrentAnimeImage => AnimeImages[_animeIndex];
        public string CurrentFoodImage => FoodImages[_foodIndex];

        #region Commands
        public ICommand ChooseCuteAnimalsCommand { get; }
        public ICommand ChooseAnimeCommand { get; }
        public ICommand ChooseFoodCommand { get; }

        #endregion

        #region CONSTRUCTOR
        public CategorySelectionViewModel()
        {
            CuteAnimalsImages = new ObservableCollection<string>
            {
                "Images/skzoo1.jpeg","Images/skzoo2.jpeg","Images/skzoo3.jpeg",
                "Images/skzoo4.jpeg","Images/skzoo5.jpeg","Images/skzoo6.jpeg",
                "Images/skzoo7.jpeg","Images/skzoo8.jpeg"
            };

            AnimeImages = new ObservableCollection<string>
            {
                "Images/anime1.jpeg", "Images/anime2.jpeg", "Images/anime3.png",
                 "Images/anime4.jpeg", "Images/anime5.jpeg", "Images/anime6.jpeg",
                  "Images/anime7.jpeg", "Images/anime8.jpeg"
            };

            FoodImages = new ObservableCollection<string>
            {
                 "Images/food1.jpeg","Images/food2.jpeg","Images/food3.jpeg",
                 "Images/food4.jpeg","Images/food5.jpeg","Images/food6.jpeg",
                 "Images/food7.jpeg","Images/food8.jpeg"
            };
            ChooseCuteAnimalsCommand = new RelayCommand(() => ChooseCategory("Cute Animals", CuteAnimalsImages));
            ChooseAnimeCommand = new RelayCommand(() => ChooseCategory("Anime", AnimeImages));
            ChooseFoodCommand = new RelayCommand(() => ChooseCategory("Food", FoodImages));

        }

        #endregion

        #region Methods
        private void ChooseCategory(string name, ObservableCollection<string> images)
        {
            GameConfiguration.SelectedCategory = name;
            GameConfiguration.SelectedImages = images.ToList();

            var mainMenu = new MainMenuView
            {
                DataContext = new MainMenuViewModel(GameConfiguration.CurrentUser)
            };
            mainMenu.Show();

            Application.Current.Windows
                .OfType<Window>()
                .FirstOrDefault(w => w is Views.CategorySelectionView)
                ?.Close();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        #endregion
    }
}
