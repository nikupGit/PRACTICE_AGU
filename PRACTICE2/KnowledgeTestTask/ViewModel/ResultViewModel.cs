using KnowledgeTestTask.Command;
using KnowledgeTestTask.Core;
using KnowledgeTestTask.Model;
using System.Collections.ObjectModel;
using KnowledgeTestTask.View.Pages;


namespace KnowledgeTestTask.ViewModel
{
    public class ResultViewModel : BaseViewModel
    {
        public int CorrectAnswers { get; set; }
        public int TotalQuestions { get; set; }
        public string Result { get; set; }

        public DelegateCommand ReturnCommand { get; }

        public ResultViewModel(ObservableCollection<Question> questions)
        {
            TotalQuestions = questions.Count;
            CorrectAnswers = questions.Count(q => q.IsOk);

            if (TotalQuestions == 0)
            {
                Result = "Тест не содержит вопросов";
            }
            else
            {
                Result = CalculateResult();
            }

            ReturnCommand = new DelegateCommand(_ => FrameManager.NavigateTo(new CreatePage()));
        }

        private string CalculateResult()
        {
            double percentage = (double)CorrectAnswers / TotalQuestions * 100;
            return percentage switch
            {
                >= 85 => "Отлично (5)",
                >= 65 => "Хорошо (4)",
                >= 45 => "Удовлетворительно (3)",
                _ => "Неудовлетворительно (2)"
            };
        }
    }
}