using KnowledgeTestTask.Model;
using System.Collections.ObjectModel;


namespace KnowledgeTestTask.Core
{
    public static class QuestionStore
    {
        public static ObservableCollection<Question> Questions { get; } = [];
    }
}