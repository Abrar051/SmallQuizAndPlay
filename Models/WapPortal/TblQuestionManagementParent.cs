using System;
using System.Collections.Generic;

#nullable disable

namespace QuizMaster.Models.WapPortal
{
    public partial class TblQuestionManagementParent
    {
        public decimal Id { get; set; }
        public string QuizName { get; set; }
        public DateTime? ScheduledDateTime { get; set; }
        public int? Status { get; set; }
        public string NumberOfQuestions { get; set; }
        public DateTime? CreatedTimeStamp { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedTimeStamp { get; set; }
        public string UpdatedBy { get; set; }
    }
}
