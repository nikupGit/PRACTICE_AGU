using System.Windows.Controls;
using System.Collections.ObjectModel;
using KnowledgeTestTask.Model;
using KnowledgeTestTask.ViewModel;


namespace KnowledgeTestTask.View.Pages
{
    public partial class ResultPage : Page
    {
        public ResultPage(ObservableCollection<Question> questions)
        {
            InitializeComponent();
            DataContext = new ResultViewModel(questions);
        }
    }
}