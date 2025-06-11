using System.Windows.Controls;
using KnowledgeTestTask.ViewModel;


namespace KnowledgeTestTask.View.Pages
{
    public partial class CreatePage : Page
    {
        public CreatePage()
        {
            InitializeComponent();
            DataContext = new CreateViewModel();
            Loaded += (s, e) => System.Diagnostics.Debug.WriteLine($"CreatePage DataContext: {DataContext?.GetType().Name}");
        }
    }
}