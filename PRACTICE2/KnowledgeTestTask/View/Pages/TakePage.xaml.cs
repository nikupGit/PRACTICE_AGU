using System.Windows.Controls;
using KnowledgeTestTask.ViewModel;


namespace KnowledgeTestTask.View.Pages
{
    public partial class TakePage : Page
    {
        public TakePage()
        {
            InitializeComponent();
            DataContext = new TakeViewModel();
            Loaded += (s, e) => System.Diagnostics.Debug.WriteLine($"TakePage DataContext: {DataContext?.GetType().Name}");
        }
    }
}