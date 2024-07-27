namespace QuizMaster.Models
{
    public class TblQuizStarSubscriptionTournament2
    {
        public long Id { get; set; }
        public string Msisdn { get; set; }
        public string Fbid { get; set; }
        public DateTime? RegDate { get; set; }
        public DateTime? ReactivationDate { get; set; }
        public DateTime? DeactivationDate { get; set; }
        public int? RegStatus { get; set; }
        public DateTime? LastLimitDate { get; set; }
        public DateTime? TimeStamp { get; set; }
        public int? RegistrationType { get; set; }
        public int? ReistrationMedium { get; set; }
        public DateTime? LastChargeDate { get; set; }
        public string SubscriptionRequestId { get; set; }

        public string CKEY { get; set; }
    }
}
