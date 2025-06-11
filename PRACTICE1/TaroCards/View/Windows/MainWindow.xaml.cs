using System.Windows;


namespace TaroCards.View.Windows
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new ViewModel.MainWindowViewModel();
        }
    }
}