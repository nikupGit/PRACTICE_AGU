using KnowledgeTestTask.Command;
using KnowledgeTestTask.Core;
using KnowledgeTestTask.Model;
using System.Windows;


namespace KnowledgeTestTask.ViewModel
{
    public class CreateViewModel : BaseViewModel
    {
        private Question _currentQuestion = new();
        private string _newOption = string.Empty;

        public Question CurrentQuestion
        {
            get => _currentQuestion;
            set
            {
                _currentQuestion = value;
                OnPropertyChanged();
            }
        }

        public string NewOption
        {
            get => _newOption;
            set
            {
                _newOption = value;
                OnPropertyChanged();
            }
        }

        public int QuestionNumber => QuestionStore.Questions.Count + 1;
        public int QuestionsCount => QuestionStore.Questions.Count;

        public DelegateCommand AddOptionCommand { get; }
        public DelegateCommand AddQuestionCommand { get; }

        public CreateViewModel()
        {
            QuestionStore.Questions.CollectionChanged += (s, e) =>
            {
                OnPropertyChanged(nameof(QuestionsCount));
                OnPropertyChanged(nameof(QuestionNumber));
            };
            AddOptionCommand = new DelegateCommand(_ => AddOption());
            AddQuestionCommand = new DelegateCommand(_ => AddQuestion());
        }

        private void AddOption()
        {
            if (string.IsNullOrWhiteSpace(NewOption))
            {
                MessageBox.Show("Вариант ответа не может быть пустым!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (CurrentQuestion.Variables.Contains(NewOption))
            {
                MessageBox.Show("Такой вариант уже существует!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            CurrentQuestion.Variables.Add(NewOption);
            NewOption = string.Empty;
        }

        private void AddQuestion()
        {
            if (string.IsNullOrWhiteSpace(CurrentQuestion.Meaning))
            {
                MessageBox.Show("Вопрос не может быть пустым!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (CurrentQuestion.Variables.Count < 2)
            {
                MessageBox.Show("Должно быть хотя бы 2 варианта ответа!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(CurrentQuestion.RightAnswer))
            {
                MessageBox.Show("Выберите правильный ответ!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!CurrentQuestion.Variables.Contains(CurrentQuestion.RightAnswer))
            {
                MessageBox.Show("Правильный ответ должен быть одним из вариантов!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            CurrentQuestion.Number = QuestionNumber;
            QuestionStore.Questions.Add(CurrentQuestion);
            CurrentQuestion = new Question();
            NewOption = string.Empty;
            OnPropertyChanged(nameof(QuestionsCount));
            OnPropertyChanged(nameof(QuestionNumber));
            MessageBox.Show($"Вопрос #{QuestionNumber - 1} добавлен!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}