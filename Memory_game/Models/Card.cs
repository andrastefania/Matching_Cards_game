using System.ComponentModel;

namespace Memory_game.Models
{
    public class Card : INotifyPropertyChanged
    {
        private bool _isFlipped;
        private bool _isMatched;

        public string Id { get; set; }
        public string ImagePath { get; set; }

        public bool IsFlipped
        {
            get => _isFlipped;
            set
            {
                _isFlipped = value;
                OnPropertyChanged(nameof(IsFlipped));
            }
        }

        public bool IsMatched
        {
            get => _isMatched;
            set
            {
                _isMatched = value;
                OnPropertyChanged(nameof(IsMatched));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
