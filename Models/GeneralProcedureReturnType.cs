using Microsoft.EntityFrameworkCore;

namespace QuizMaster.Models
{
    [Keyless]
    public class GeneralProcedureReturnType
    {
        public int responseInt { get; set; }
        public string responseString { get; set; }

    }
}
