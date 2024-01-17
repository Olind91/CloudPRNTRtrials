using Cloud_Blazor.Models;

namespace Cloud_Blazor.Helpers.Interfaces
{
    public interface IPrintJobService
    {
        List<PrintJob> PrintJobs { get; set; }
        Task CreatePrintJob(PrintJob printjob);
        Task<List<PrintJob>> GetPrintJobs();
    }
}
