namespace SolaceTK.Models.WorkItems
{
    public class WorkPayment : SolTkModelBase
    {
        public WorkProject? Project { get; set; }

        public DateTimeOffset PaymentDate { get; set; }
        public float Amount { get; set; }
        //public ICollection<SolTkData> PaymentData { get; set; }
        public ICollection<WorkItem>? WorkItems { get; set; } = new List<WorkItem>();

    }
}
