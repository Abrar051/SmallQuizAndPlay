namespace QuizMaster.Models
{
    public class AnswerViewModel
    {
        public string Answer { get; set; }
        public string FbId { get; set; }
        public int IsRight { get; set; } // Change data type to int
        public DateTime TimeStamp { get; set; }
        public int TimeTaken { get; set; }
        public int Round { get; set; }
        public string ServiceName { get; set; }
    }
}
