using System.Windows.Controls;


namespace KnowledgeTestTask.Core
{
    public static class FrameManager
    {
        public static Frame? MainFrame { get; set; }

        public static void NavigateTo<T>(T page) where T : Page
        {
            MainFrame?.Navigate(page);
        }
    }
}