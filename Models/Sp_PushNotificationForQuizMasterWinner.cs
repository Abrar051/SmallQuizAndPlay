using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizMaster.Models
{
    [Keyless]
    public class Sp_PushNotificationForQuizMasterWinner
    {
        public string PushNotificationToken { get; set; }
    }
}
