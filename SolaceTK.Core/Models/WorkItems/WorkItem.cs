using System;
using System.Collections.Generic;

namespace SolaceTK.Core.Models.WorkItems
{
    public class WorkItem : IModelTK
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        
        public string Tags { get; set; }

        // Work Item Details:
        public float HoursEstimate { get; set; }
        public float HoursActual { get; set; }

        public WorkPayment Payment { get; set; }
        public bool IsPaid { get; set; }

        public int WorkProjectId { get; set; }

        public ICollection<WorkComment> Comments { get; set; }
        public WorkArtifact Artifact { get; set; }

        public DateTimeOffset Created { get; set; }
        public DateTimeOffset Updated { get; set; }
    }
}
