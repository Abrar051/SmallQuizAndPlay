using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QuizMaster.Models
{
    public class SignInModel
    {
        [Required]
        public string username { get; set; }

        [Required]
        public string password { get; set; }
        public string mobile_number { get; set; }
    }
}