using System.Collections.ObjectModel;


namespace KnowledgeTestTask.Model
{
    public class Question
    {
        public int Number { get; set; }
        public string Meaning { get; set; } = string.Empty;
        public ObservableCollection<string> Variables { get; set; } = [];
        public string RightAnswer { get; set; } = string.Empty;
        public string LastAnswer { get; set; } = string.Empty;
        public bool IsOk { get; set; }
    }
}