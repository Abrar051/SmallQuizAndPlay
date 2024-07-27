using System;
using System.Collections.Generic;

#nullable disable

namespace QuizMaster.Models.WapPortal
{
    public partial class TblLiveQuizVideoQuestionsList
    {
        public decimal Id { get; set; }
        public string VideoName { get; set; }
        public string VideoUrl { get; set; }
        public string Duration { get; set; }
        public string VideoCategoryId { get; set; }
        public string Question { get; set; }
        public string Option1 { get; set; }
        public string Option2 { get; set; }
        public string Option3 { get; set; }
        public string Answer { get; set; }
        public string OptionAppearingTime { get; set; }
        public string OptionShowingDuration { get; set; }
        public DateTime? CreatedTimeStamp { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedTimeStamp { get; set; }
        public string UpdatedBy { get; set; }
        public string GcloudVideoUrl { get; set; }
    }
}
