using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizMaster.Models
{
    [Keyless]
    public class sp_Bkash_Result_Jhotpot_Result
    {
        public string name { get; set; }
        public string fbid { get; set; }
        public Nullable<int> round { get; set; }
        public Nullable<int> Time { get; set; }
        public Nullable<int> RightAns { get; set; }
    }
}
