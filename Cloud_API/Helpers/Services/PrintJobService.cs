using Cloud_API.Helpers.Repositories;
using Cloud_API.Interfaces;
using Cloud_API.Models;

namespace Cloud_API.Helpers.Services
{
    public class PrintJobService : IPrintJobService
    {
        private readonly PrintJobRepository _printJobRepository;

        public PrintJobService(PrintJobRepository printJobRepository)
        {
            _printJobRepository = printJobRepository;
        }

        public async Task<PrintJob> CreatePrintJobAsync(string content)
        {
            var printJob = new PrintJob
            {
                Content = content,
                CreatedAt = DateTime.UtcNow
            };

            return await _printJobRepository.AddAsync(printJob);
        }

        public async Task<IEnumerable<PrintJob>> GetRecentPrintJobsAsync(int count)
        {
            return await _printJobRepository.GetRecentPrintJobsAsync(count);
        }

        public bool IsJobAvailable(string printerMAC)
        {
            throw new NotImplementedException();
        }

        public void RemovePrintJob(string jobId)
        {
            throw new NotImplementedException();
        }

        public bool TryGetPrintJob(string jobId, out object jobData)
        {
            throw new NotImplementedException();
        }

        
    }
}
