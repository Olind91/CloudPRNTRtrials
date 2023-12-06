namespace Cloud_API.Models
{
    public class PrintJob
    {
        public int Id { get; set; }
        public string? Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public string PrinterMAC { get; set; } = "00:11:62:1e:a4:e1";
        public PrintJobStatus Status { get; set; } = PrintJobStatus.Pending;
    }

    public enum PrintJobStatus
    {
        Pending,
        InProgress,
        Completed,
        Failed,
        
    }
}
