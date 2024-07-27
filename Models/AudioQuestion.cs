namespace QuizMaster.Models
{
    public class AudioQuestion
    {
        public string ImageFile { get; set; }
        public string AudioFile { get; set; }
        public string Option1 { get; set; }
        public string Option2 { get; set; }
        public string Option3 { get; set; }
        public string Answer { get; set; }
        public DateTime TimeStamp { get; set; }
        public string QuestionCategory { get; set; }
    }
}
