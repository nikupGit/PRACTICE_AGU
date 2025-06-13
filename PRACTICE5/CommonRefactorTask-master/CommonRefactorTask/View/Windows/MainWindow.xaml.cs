using CommonRefactorTask.ViewModel;
using System.Windows;


namespace CommonRefactorTask.View.Windows
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();
        }
    }
}