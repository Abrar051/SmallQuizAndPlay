using System;
using System.Collections.Generic;

#nullable disable

namespace QuizMaster.Models.WapPortal
{
    public partial class JhotPotThemeQuestion
    {
        public double Id { get; set; }
        public string Question { get; set; }
        public string Option1 { get; set; }
        public string Option2 { get; set; }
        public string Option3 { get; set; }
        public string Answer { get; set; }
        public double? Level { get; set; }
        public string Theme { get; set; }
        public DateTime? TimeStamp { get; set; }
        public string UploadBy { get; set; }
        public string QuestionCategory { get; set; }
        public string QuestionImage { get; set; }
    }
}
