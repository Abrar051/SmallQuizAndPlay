using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace QuizMaster.Models.WapPortal
{
    [Keyless]
    public class GetLiveQuizVideosDataModel
    {
        [Column(TypeName = "decimal(18,4)")]
        public decimal? QuesManagementId { get; set; }
        public string QuizName { get; set; }
        public DateTime? ScheduledDateTime { get; set; }
        public int? Status { get; set; }
        public string NumberOfQuestions { get; set; }
        public DateTime? CreatedTimeStamp { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedTimeStamp { get; set; }
        public string UpdatedBy { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal? LiveQuizVideoId { get; set; }
        public string VideoName { get; set; }
        public string VideoURL { get; set; }
        public string Duration { get; set; }
        public string VideoCategory_id { get; set; }
        public string Question { get; set; }
        public string Option1 { get; set; }
        public string Option2 { get; set; }
        public string Option3 { get; set; }
        public string Answer { get; set; }
        public string OptionAppearingTime { get; set; }
        public string OptionShowingDuration { get; set; }
        public DateTime? VideoCreatedTimeStamp { get; set; }
        public string VideoCreatedBy { get; set; }
        public DateTime? VideoUpdatedTimeStamp { get; set; }
        public string VideoUpdatedBy { get; set; }
        public int? SLNo { get; set; }
    }
}
