namespace QuizMaster.Models
{
    public class GetPaymentListBySubIdModel
    {
        public string Id { get; set; }
        public string SubscriptionId { get; set; }
        public string DueDate { get; set; }
        public string Status { get; set; }
        public string TrxId { get; set; }
        public string TrxTime { get; set; }
        public string Amount { get; set; }
        public string ReverseTrxAmount { get; set; }
        public string ReverseTrxId { get; set; }
        public string ReverseTrxTime { get; set; }
    }
}
