using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizMaster.Models
{
    public class WinnerListModel
    {
        public string currentDate { get; set; }
        public List<sp_Bkash_Result_Jhotpot_Result> dailyWinnerList { get; set; }
        public List<sp_Bkash_Result_Jhotpot_Result> HappyHourWinnerList { get; set; }
        public List<sp_Bkash_Result_Jhotpot_Result> SecondHappyHourWinnerList { get; set; }
    }
}
