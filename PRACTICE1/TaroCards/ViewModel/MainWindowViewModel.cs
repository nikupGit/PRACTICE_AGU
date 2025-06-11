using System.Collections.ObjectModel;
using System.Windows.Input;
using TaroCards.Command;
using TaroCards.Core.Enums;
using TaroCards.Model;


namespace TaroCards.ViewModel
{
    public class MainWindowViewModel : BaseViewModel
    {
        private readonly Random _rand = new();
        private string _searchText = "";
        private List<Card> _originalCards = new();

        public ObservableCollection<Card> CardList { get; } = new();

        public ICommand ShuffleCommand { get; }
        public ICommand SortCommand { get; }
        public ICommand RestoreCommand { get; }
        public ICommand GenerateCommand { get; }

        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                FilterCards();
            }
        }

        public MainWindowViewModel()
        {
            GenerateCommand = new DelegateCommand(GenerateCards);
            ShuffleCommand = new DelegateCommand(ShuffleCards);
            SortCommand = new DelegateCommand(SortCards);
            RestoreCommand = new DelegateCommand(RestoreCards);

            GenerateCards(null);
        }

        private void GenerateCards(object parameter)
        {
            CardList.Clear();
            _originalCards.Clear();

            int count = _rand.Next(10, 41);
            var values = (CardEnum[])Enum.GetValues(typeof(CardEnum));

            for (int i = 0; i < count; i++)
            {
                var card = new Card
                {
                    Key = i + 1,
                    Name = values[_rand.Next(values.Length)].ToString()
                };

                CardList.Add(card);
                _originalCards.Add(card);
            }
        }

        private void ShuffleCards(object parameter)
        {
            var shuffled = CardList.OrderBy(_ => _rand.Next()).ToList();
            CardList.Clear();
            foreach (var card in shuffled) CardList.Add(card);
        }

        private void SortCards(object parameter)
        {
            var sorted = CardList.OrderBy(c => c.Name).ToList();
            CardList.Clear();
            foreach (var card in sorted) CardList.Add(card);
        }

        private void RestoreCards(object parameter)
        {
            CardList.Clear();
            foreach (var card in _originalCards) CardList.Add(card);
        }

        private void FilterCards()
        {
            if (string.IsNullOrWhiteSpace(_searchText))
            {
                RestoreCards(null);
                return;
            }

            var filtered = _originalCards.Where(c => c.Name.Contains(_searchText, StringComparison.OrdinalIgnoreCase)).ToList();

            CardList.Clear();
            foreach (var card in filtered) CardList.Add(card);
        }
    }
}