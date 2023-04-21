using System;
using System.Collections.Generic;

namespace SolaceTK.Core.Models.WorkItems
{
    public class WorkProject : IModelTK
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        
        public string Tags { get; set; }

        public float TotalEstimatedHours { get; set; }
        public float TotalActualHours { get; set; }
        public float PaidHours { get; set; }
        public float TotalPaid { get; set; }
        public float RemainingPayment { get; set; }
        public bool IsProjectBillable { get; set; }

        public ICollection<WorkItem> WorkItems { get; set; }

        public ICollection<WorkPayment> Payments { get; set; }

        public ICollection<WorkComment> Comments { get; set; }
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset Updated { get; set; }
    }
}
