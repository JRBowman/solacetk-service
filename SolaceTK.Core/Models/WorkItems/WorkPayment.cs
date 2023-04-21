using System;
using System.Collections.Generic;

namespace SolaceTK.Core.Models.WorkItems
{
    public class WorkPayment : IModelTK
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        
        public string Tags { get; set; }

        public WorkProject Project { get; set; }

        public DateTimeOffset PaymentDate { get; set; }
        public float Amount { get; set; }
        //public ICollection<SolTkData> PaymentData { get; set; }
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset Updated { get; set; }

        public ICollection<WorkItem> WorkItems { get; set; }

    }
}
