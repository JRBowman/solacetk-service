namespace SolaceTK.Models.WorkItems
{
    public class WorkProject : SolTkModelBase
    {
        public float TotalEstimatedHours { get; set; }
        public float TotalActualHours { get; set; }
        public float PaidHours { get; set; }
        public float TotalPaid { get; set; }
        public float RemainingPayment { get; set; }
        public bool IsProjectBillable { get; set; }

        public ICollection<WorkItem>? WorkItems { get; set; } = new List<WorkItem>();

        public ICollection<WorkPayment>? Payments { get; set; } = new List<WorkPayment>();

        public ICollection<WorkComment>? Comments { get; set; } = new List<WorkComment>();
    }
}
