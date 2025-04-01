using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Memory_game.Commands;
using System.Windows.Input;
using System.Windows;
using Memory_game.Models;

namespace Memory_game.ViewModels
{
    public class BoardSettingsViewModel
    {
        public string SelectedMode { get; set; } = "Standard (4x4)";
        public string Rows { get; set; } = "4";
        public string Columns { get; set; } = "4";
        public string TimeLimit { get; set; } = "60";
        public string SelectedBoardSize { get; set; } = "4x4";
        public List<string> BoardSizes { get; } = new List<string>
        {
            "2x2",
            "2x4",
            "4x4"
        };

        #region Commands

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        #endregion

        #region Methods

        public BoardSettingsViewModel()
        {
            SaveCommand = new RelayCommand(Save);
            CancelCommand = new RelayCommand(Cancel);
        }

        private void Save()
        {
            var parts = SelectedBoardSize.Split('x');
            if (parts.Length == 2 &&
                int.TryParse(parts[0], out int rows) &&
                int.TryParse(parts[1], out int cols) &&
                int.TryParse(TimeLimit, out int time))
            {
                GameConfiguration.Rows = rows;
                GameConfiguration.Columns = cols;
                GameConfiguration.TimeLimitSeconds = time;

                CloseWindow();
            }
            else
            {
                MessageBox.Show("Invalid board size or time.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }


        private void Cancel()
        {
            CloseWindow();
        }

        private void CloseWindow()
        {
            Application.Current.Windows
                .OfType<Window>()
                .FirstOrDefault(w => w is Views.BoardSettingsView)
                ?.Close();
        }

        #endregion
    }
}
