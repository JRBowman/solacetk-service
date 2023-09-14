using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaceTK.Models.Telemetry
{
    public interface ISolTkOperation<T>
    {
        public T? Data { get; set; }
        public SolTkOperationStatus Status { get; set; }
        public SolTkOperationResultCode ResultCode { get; set; }

        public int CorrelationId { get; set; }
        public string OperationName { get; set; }

        public DateTimeOffset? StartTime { get; set; }
        public DateTimeOffset? EndTime { get; set; }
    }
}
