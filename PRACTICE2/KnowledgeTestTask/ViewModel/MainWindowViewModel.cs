using KnowledgeTestTask.Command;
using KnowledgeTestTask.Core;
using KnowledgeTestTask.View.Pages;
using System.Windows;


namespace KnowledgeTestTask.ViewModel
{
    public class MainWindowViewModel : BaseViewModel
    {
        public DelegateCommand StartTestCommand { get; }
        public DelegateCommand CreateTestCommand { get; }

        public MainWindowViewModel()
        {
            StartTestCommand = new DelegateCommand(_ => StartTest());
            CreateTestCommand = new DelegateCommand(_ => CreateTest());
        }

        private static void StartTest()
        {
            if (QuestionStore.Questions.Count == 0)
            {
                MessageBox.Show("Тест не создан!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            FrameManager.NavigateTo(new TakePage());
        }

        private static void CreateTest()
        {
            QuestionStore.Questions.Clear();
            MessageBox.Show("Новый тест начат, предыдущие вопросы удалены.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
            FrameManager.NavigateTo(new CreatePage());
        }
    }
}