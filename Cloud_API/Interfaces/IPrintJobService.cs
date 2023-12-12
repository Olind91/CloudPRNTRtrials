using Cloud_API.Models;

namespace Cloud_API.Interfaces
{
    public interface IPrintJobService
    {
        Task<PrintJob> CreatePrintJobAsync(string content);
        Task<PrintJob> FindJobFromMac(string printerMAC);
        Task<IEnumerable<PrintJob>> GetRecentPrintJobsAsync(int count);
        bool IsJobAvailable(string printerMAC);
        void RemovePrintJob(string jobId);
        bool TryGetPrintJob(string jobId, out PrintJob jobData);
        Task<PrintJob> UpdateJobStatus(int jobId, PrintJobStatus status);
        Task<PrintJob?> GetSinglePrintJobAsync(int id);


    }
}
