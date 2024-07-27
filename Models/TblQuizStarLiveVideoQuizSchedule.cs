using System;
using System.Collections.Generic;

namespace QuizMaster.Models
{
    public partial class TblQuizStarLiveVideoQuizSchedule
    {
        public TblQuizStarLiveVideoQuizSchedule()
        {
            TblQuizStarLiveVideoQuizScheduleDetails = new HashSet<TblQuizStarLiveVideoQuizScheduleDetail>();
        }

        public long Id { get; set; }
        public string LiveQuizName { get; set; }
        public DateTime? ScheduledDateTime { get; set; }
        public DateTime? TimeStamp { get; set; }
        public int? NumberOfQuestions { get; set; }
        public string VideoName { get; set; }
        public int? Status { get; set; }
        public decimal? Duration { get; set; }
        public string EntryBy { get; set; }

        public virtual ICollection<TblQuizStarLiveVideoQuizScheduleDetail> TblQuizStarLiveVideoQuizScheduleDetails { get; set; }
    }
}
