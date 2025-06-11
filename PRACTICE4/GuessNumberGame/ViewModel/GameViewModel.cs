using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Input;
using GuessNumberGame.Command;
using GuessNumberGame.Model;


namespace GuessNumberGame.ViewModel
{
    public class GameViewModel : INotifyPropertyChanged
    {
        private int targetNumber;
        private int attempts;
        private string _guessInput;
        private string _hintText;
        private string _playerName;
        private bool _isInputEnabled;
        private Visibility _resultPanelVisibility;
        private ObservableCollection<GameResult> _results;

        public event PropertyChangedEventHandler? PropertyChanged;

        public string GuessInput
        {
            get => _guessInput;
            set
            {
                _guessInput = value;
                OnPropertyChanged(nameof(GuessInput));
            }
        }

        public string HintText
        {
            get => _hintText;
            set
            {
                _hintText = value;
                OnPropertyChanged(nameof(HintText));
            }
        }

        public string PlayerName
        {
            get => _playerName;
            set
            {
                _playerName = value;
                OnPropertyChanged(nameof(PlayerName));
            }
        }

        public bool IsInputEnabled
        {
            get => _isInputEnabled;
            set
            {
                _isInputEnabled = value;
                OnPropertyChanged(nameof(IsInputEnabled));
            }
        }

        public Visibility ResultPanelVisibility
        {
            get => _resultPanelVisibility;
            set
            {
                _resultPanelVisibility = value;
                OnPropertyChanged(nameof(ResultPanelVisibility));
            }
        }

        public ObservableCollection<GameResult> Results
        {
            get => _results;
            set
            {
                _results = value;
                OnPropertyChanged(nameof(Results));
            }
        }

        public ICommand CheckGuessCommand { get; }
        public ICommand SaveResultCommand { get; }

        public GameViewModel()
        {
            Results = new ObservableCollection<GameResult>(LoadResults());
            CheckGuessCommand = new DelegateCommand(CheckGuess, CanCheckGuess);
            SaveResultCommand = new DelegateCommand(SaveResult, CanSaveResult);
            StartNewGame();
        }

        private void StartNewGame()
        {
            Random random = new();
            targetNumber = random.Next(1, 101);
            attempts = 0;
            HintText = "Новое число загадано! Введите ваш вариант.";
            GuessInput = "";
            ResultPanelVisibility = Visibility.Collapsed;
            IsInputEnabled = true;
        }

        private void CheckGuess(object parameter)
        {
            if (int.TryParse(GuessInput, out int guess))
            {
                attempts++;

                if (guess < 1 || guess > 100)
                {
                    HintText = "Пожалуйста, введите число от 1 до 100.";
                    return;
                }

                if (guess == targetNumber)
                {
                    HintText = $"Поздравляем! Вы угадали число за {attempts} попыток!";
                    IsInputEnabled = false;
                    ResultPanelVisibility = Visibility.Visible;
                }
                else if (guess < targetNumber)
                {
                    HintText = "Загаданное число больше.";
                }
                else
                {
                    HintText = "Загаданное число меньше.";
                }
            }
            else
            {
                HintText = "Пожалуйста, введите корректное число.";
            }
        }

        private bool CanCheckGuess(object parameter) => IsInputEnabled;

        private void SaveResult(object parameter)
        {
            if (string.IsNullOrEmpty(PlayerName?.Trim()))
            {
                MessageBox.Show("Пожалуйста, введите имя игрока.");
                return;
            }

            GameResult result = new()
            {
                PlayerName = PlayerName.Trim(),
                Attempts = attempts,
                Date = DateTime.Now
            };

            Results.Add(result);
            SaveResults();
            StartNewGame();
        }

        private bool CanSaveResult(object parameter) => !string.IsNullOrEmpty(PlayerName?.Trim());

        private static System.Collections.Generic.List<GameResult> LoadResults()
        {
            var loadedResults = new System.Collections.Generic.List<GameResult>();
            try
            {
                if (File.Exists("results.txt"))
                {
                    string[] lines = File.ReadAllLines("results.txt");
                    foreach (string line in lines)
                    {
                        string[] parts = line.Split('|');
                        if (parts.Length == 3 &&
                            DateTime.TryParse(parts[2], out DateTime date) &&
                            int.TryParse(parts[1], out int attempts))
                        {
                            loadedResults.Add(new GameResult
                            {
                                PlayerName = parts[0],
                                Attempts = attempts,
                                Date = date
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке результатов: {ex.Message}");
            }
            return loadedResults;
        }

        private void SaveResults()
        {
            try
            {
                var lines = new System.Collections.Generic.List<string>();
                foreach (GameResult result in Results)
                {
                    lines.Add($"{result.PlayerName}|{result.Attempts}|{result.Date}");
                }
                File.WriteAllLines("results.txt", lines);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении результатов: {ex.Message}");
            }
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}