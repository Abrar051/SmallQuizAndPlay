using System;
using System.Collections.Generic;

namespace QuizMaster.Models
{
    public partial class TblPuzzleWordLib
    {
        public long Id { get; set; }
        public string Words { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
