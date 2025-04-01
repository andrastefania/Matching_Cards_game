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
        public ObservableCollection<string> VikingImages { get; set; }
        public ObservableCollection<string> WizardImages { get; set; }
        public ObservableCollection<string> FarmerImages { get; set; }


        private int _vikingIndex;
        private int _wizardIndex;
        private int _farmerIndex;


        public string CurrentVikingImage => VikingImages[_vikingIndex];
        public string CurrentWizardImage => WizardImages[_wizardIndex];
        public string CurrentFarmerImage => FarmerImages[_farmerIndex];

        #region Commands

        public ICommand PrevVikingCommand { get; }
        public ICommand NextVikingCommand { get; }
        public ICommand PrevWizardCommand { get; }
        public ICommand NextWizardCommand { get; }
        public ICommand PrevFarmerCommand { get; }
        public ICommand NextFarmerCommand { get; }
        public ICommand ChooseVikingCommand { get; }
        public ICommand ChooseWizardCommand { get; }
        public ICommand ChooseFarmerCommand { get; }

        #endregion

        #region CONSTRUCTOR
        public CategorySelectionViewModel()
        {
            VikingImages = new ObservableCollection<string>
            {
                "Images/skzoo1.jpeg","Images/skzoo2.jpeg","Images/skzoo3.jpeg",
                "Images/skzoo4.jpeg","Images/skzoo5.jpeg","Images/skzoo6.jpeg",
                "Images/skzoo7.jpeg","Images/skzoo8.jpeg"
            };

            WizardImages = new ObservableCollection<string>
            {
                "Images/anime1.jpeg", "Images/anime2.jpeg", "Images/anime3.png",
                 "Images/anime4.jpeg", "Images/anime5.jpeg", "Images/anime6.jpeg",
                  "Images/anime7.jpeg", "Images/anime8.jpeg"
            };

            FarmerImages = new ObservableCollection<string>
            {
                 "Images/food1.jpeg","Images/food2.jpeg","Images/food3.jpeg",
                 "Images/food4.jpeg","Images/food5.jpeg","Images/food6.jpeg",
                 "Images/food7.jpeg","Images/food8.jpeg"
            };

            PrevVikingCommand = new RelayCommand(() => ChangeImage(VikingImages, ref _vikingIndex, false, nameof(CurrentVikingImage)));
            NextVikingCommand = new RelayCommand(() => ChangeImage(VikingImages, ref _vikingIndex, true, nameof(CurrentVikingImage)));

            PrevWizardCommand = new RelayCommand(() => ChangeImage(WizardImages, ref _wizardIndex, false, nameof(CurrentWizardImage)));
            NextWizardCommand = new RelayCommand(() => ChangeImage(WizardImages, ref _wizardIndex, true, nameof(CurrentWizardImage)));

            PrevFarmerCommand = new RelayCommand(() => ChangeImage(FarmerImages, ref _farmerIndex, false, nameof(CurrentFarmerImage)));
            NextFarmerCommand = new RelayCommand(() => ChangeImage(FarmerImages, ref _farmerIndex, true, nameof(CurrentFarmerImage)));



            ChooseVikingCommand = new RelayCommand(() => ChooseCategory("Vikings", VikingImages));
            ChooseWizardCommand = new RelayCommand(() => ChooseCategory("Wizards", WizardImages));
            ChooseFarmerCommand = new RelayCommand(() => ChooseCategory("Farmers", FarmerImages));

        }

        #endregion

        #region Methods

        private void ChangeImage(ObservableCollection<string> images, ref int index, bool next, string propertyName)
        {
            if (images.Count == 0) return;

            index = (index + (next ? 1 : -1) + images.Count) % images.Count;
            OnPropertyChanged(propertyName);
        }



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
