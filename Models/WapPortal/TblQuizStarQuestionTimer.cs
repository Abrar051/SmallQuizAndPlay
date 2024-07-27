using System;
using System.Collections.Generic;

#nullable disable

namespace QuizMaster.Models.WapPortal
{
    public partial class TblQuizStarQuestionTimer
    {
        public long Id { get; set; }
        public string TypeName { get; set; }
        public int? QuestionCount { get; set; }
        public string Timer { get; set; }
        public DateTime? TimeStamp { get; set; }
    }
}
