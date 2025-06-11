using KnowledgeTestTask.Command;
using KnowledgeTestTask.Core;
using KnowledgeTestTask.Model;
using KnowledgeTestTask.View.Pages;
using System.Collections.ObjectModel;
using System.Windows;


namespace KnowledgeTestTask.ViewModel
{
    public class TakeViewModel : BaseViewModel
    {
        private int _currentIndex = 0;
        private Question _currentQuestion = new();
        private string _selectedAnswer = string.Empty;
        private int _currentQuestionNumber = 1;

        public ObservableCollection<Question> Questions { get; } = new();
        public Question CurrentQuestion
        {
            get => _currentQuestion;
            set
            {
                _currentQuestion = value;
                OnPropertyChanged();
            }
        }

        public string SelectedAnswer
        {
            get => _selectedAnswer;
            set
            {
                _selectedAnswer = value;
                OnPropertyChanged();
            }
        }

        public int CurrentQuestionNumber
        {
            get => _currentQuestionNumber;
            private set
            {
                _currentQuestionNumber = value;
                OnPropertyChanged();
            }
        }

        public int TotalQuestions => Questions.Count;
        public bool IsLastQuestion => _currentIndex == Questions.Count - 1;

        public DelegateCommand AnswerCommand { get; }
        public DelegateCommand FinishCommand { get; }

        public TakeViewModel()
        {
            AnswerCommand = new DelegateCommand(_ => AnswerQuestion());
            FinishCommand = new DelegateCommand(_ => FinishTest());

            if (!QuestionStore.Questions.Any())
            {
                MessageBox.Show("Тест не содержит вопросов!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                FrameManager.NavigateTo(new CreatePage());
                return;
            }

            foreach (var question in QuestionStore.Questions)
            {
                Questions.Add(new Question
                {
                    Number = question.Number,
                    Meaning = question.Meaning,
                    Variables = new ObservableCollection<string>(question.Variables),
                    RightAnswer = question.RightAnswer
                });
            }

            CurrentQuestion = Questions.First();
            SelectedAnswer = CurrentQuestion.LastAnswer;
            OnPropertyChanged(nameof(TotalQuestions));
            OnPropertyChanged(nameof(CurrentQuestionNumber));
        }

        private void AnswerQuestion()
        {
            SaveAnswer();

            if (!IsLastQuestion)
            {
                _currentIndex++;
                CurrentQuestion = Questions[_currentIndex];
                SelectedAnswer = CurrentQuestion.LastAnswer;
                CurrentQuestionNumber = _currentIndex + 1;
                OnPropertyChanged(nameof(IsLastQuestion));
                OnPropertyChanged(nameof(TotalQuestions));
                OnPropertyChanged(nameof(CurrentQuestionNumber));
            }
        }

        private void SaveAnswer()
        {
            if (!string.IsNullOrEmpty(SelectedAnswer))
            {
                CurrentQuestion.LastAnswer = SelectedAnswer;
                CurrentQuestion.IsOk = string.Equals(
                    CurrentQuestion.LastAnswer,
                    CurrentQuestion.RightAnswer,
                    StringComparison.OrdinalIgnoreCase
                );
            }
        }

        private void FinishTest()
        {
            SaveAnswer();
            FrameManager.NavigateTo(new ResultPage(Questions));
        }
    }
}