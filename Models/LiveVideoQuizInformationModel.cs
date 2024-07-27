using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizMaster.Models
{
    [Keyless]
    public class LiveVideoQuizInformationModel
    {
        public string LiveQuizName { get; set; }
        public DateTime ScheduledDateTime { get; set; }
        public string VideoName { get; set; }
        public int? status { get; set; }
        [Precision(18,0)]
        public decimal? Duration { get; set; } //in seconds
        public Int64? LiveQuizUId { get; set; }
        [Precision(18, 0)]
        public decimal? QuestionId { get; set; }
        public int? QuesAppearingSec { get; set; }
        public int? OptionShowingDurationSec { get; set; }
        public string Question { get; set; }
        public string Option1 { get; set; }
        public string Option2 { get; set; }
        public string Option3 { get; set; }
        public string Answer { get; set; }
    }
}
