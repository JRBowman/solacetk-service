namespace SolaceTK.Models.Telemetry
{
    public class SolTkOperationStatus : IModelTK
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Tags { get; set; }

        public ICollection<string>? Logs { get; set; }
        public ICollection<SolTkException>? Exceptions { get; set; }
        public ICollection<string>? Errors { get; set; }

        //public ICollection<SoltkOp>

        public void AddException(Exception ex)
        {
            var soltkEx = new SolTkException()
            {
                StackTrace = ex.StackTrace,
                Data = ex.Data,
                Message = ex.Message,
                Name = ex.TargetSite?.Name
            };
            Exceptions ??= new List<SolTkException>();
            Exceptions.Add(soltkEx);
        }

        public void AddErrors(params string[] errors)
        {
            Errors ??= new List<string>();
            (Errors as List<string>).AddRange(errors);
        }

        public void AddLogs(params string[] logs)
        {
            Logs ??= new List<string>();
            (Logs as List<string>).AddRange(logs);
        }
    }
}
