using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaceTK.Models.Telemetry
{
    public class SolTkException : IModelTK
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Tags { get; set; }

        public string? Message { get; set; }
        public IDictionary? Data { get; set; }
        public string? StackTrace { get; set; }
       
    }
}
