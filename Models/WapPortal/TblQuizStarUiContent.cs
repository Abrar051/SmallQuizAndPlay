using System;
using System.Collections.Generic;

#nullable disable

namespace QuizMaster.Models.WapPortal
{
    public partial class TblQuizStarUiContent
    {
        public long Id { get; set; }
        public long? TypeId { get; set; }
        public string Title { get; set; }
        public string TitleEnglish { get; set; }
        public string Details { get; set; }
        public string DetailsEnglish { get; set; }
        public DateTime? TimeStamp { get; set; }
    }
}
