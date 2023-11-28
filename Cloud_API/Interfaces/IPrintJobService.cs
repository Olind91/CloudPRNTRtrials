using Cloud_API.Models;

namespace Cloud_API.Interfaces
{
    public interface IPrintJobService
    {
        Task<PrintJob> CreatePrintJobAsync(string content);
        Task<IEnumerable<PrintJob>> GetRecentPrintJobsAsync(int count);
        bool IsJobAvailable(string printerMAC);
        void RemovePrintJob(string jobId);
        bool TryGetPrintJob(string jobId, out object jobData);
        // Add other methods as needed
    }
}
