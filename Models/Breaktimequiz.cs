using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;



namespace QuizMaster.Models
{
    public class Breaktimequiz
    {
        public long Id { get; set; }
        public DateTime Date { get; set; }
        public string Question { get; set; }
    }
}