using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizMaster.Models
{
    [Keyless]
    public class QuizQuestionCategorySelection
    {
        public string QuestionCategory { get; set; }
        public string HomeThemeImage { get; set; }
        //public string ID { get; set; }
    }
}
