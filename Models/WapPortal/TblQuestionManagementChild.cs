using System;
using System.Collections.Generic;

#nullable disable

namespace QuizMaster.Models.WapPortal
{
    public partial class TblQuestionManagementChild
    {
        public decimal Id { get; set; }
        public string LiveQuizId { get; set; }
        public string QuestionManagementParentId { get; set; }
        public DateTime? TimeStamp { get; set; }
        public int? Slno { get; set; }
    }
}
