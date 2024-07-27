using System;
using System.Collections.Generic;

namespace QuizMaster.Models
{
    public partial class TblQuizStarLiveVideoQuizScheduleDetail
    {
        public long Id { get; set; }
        public long? LiveQuizUid { get; set; }
        public decimal? QuestionId { get; set; }
        public int? SerialNo { get; set; }
        public int? QuesAppearingSec { get; set; }
        public int? OptionShowingDurationSec { get; set; }
        public DateTime? TimeStamp { get; set; }

        public virtual TblQuizStarLiveVideoQuizSchedule LiveQuizU { get; set; }
    }
}
