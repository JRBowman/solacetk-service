using System;

namespace SolaceTK.Core.Models.WorkItems
{
    public class WorkComment : IModelTK
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        
        public string Tags { get; set; }

        // Comment Info:
        public string Comment { get; set; }
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset Updated { get; set; }
    }
}
