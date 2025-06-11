using KnowledgeTestTask.Core;
using KnowledgeTestTask.View.Pages;
using KnowledgeTestTask.ViewModel;
using System.Windows;


namespace KnowledgeTestTask.View.Windows
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            FrameManager.MainFrame = ContentFrame;
            DataContext = new MainWindowViewModel();
            FrameManager.NavigateTo(new CreatePage());
        }
    }
}