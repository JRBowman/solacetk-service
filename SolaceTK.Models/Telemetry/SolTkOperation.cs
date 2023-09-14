using SolaceTK.Models.Identity;

namespace SolaceTK.Models.Telemetry
{
    public class SolTkOperation<T> : SolTkModelBase, ISolTkOperation<T>
    {
        public SolTkOperation() 
        {
            Status = new();
        }

        public SolTkOperation(string name, string description = "", string tags = "", int correlationId = 0)
        {
            Name = name;
            Description = description;
            Tags = tags;
            OperationName = name;
            Status = new();
            CorrelationId = correlationId;
        }

        public T? Data { get; set; }
        public SolTkOperationStatus Status { get; set; } = new();
        public SolTkOperationResultCode ResultCode { get; set; }
        public ICollection<SolTkOperation<T>> Operations { get; set; }

        public int CorrelationId { get; set; }
        public string OperationName { get; set; } = "op";

        public DateTimeOffset? StartTime { get; set; }
        public DateTimeOffset? EndTime { get; set; }

        public void Start()
        {
            StartTime = DateTimeOffset.UtcNow;
        }

        public void Stop()
        {
            EndTime = DateTimeOffset.UtcNow;
        }

    }

    public enum SolTkOperationResultCode
    {
        NoOp,
        InvalidModel,
        Ok,
        Failed,
        Merged,
        Succeeded,
        ExThrown,
        Updated,
        Deleted,
        Created
    }
}
