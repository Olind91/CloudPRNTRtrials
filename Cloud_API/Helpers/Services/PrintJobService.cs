using Cloud_API.Helpers.Repositories;
using Cloud_API.Interfaces;
using Cloud_API.Models;
using Microsoft.EntityFrameworkCore;

namespace Cloud_API.Helpers.Services
{
    public class PrintJobService : IPrintJobService
    {
        private const string HardCodedPrinterMAC = "00:11:62:1e:a4:e1"; //For now

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

        public async Task<PrintJob> FindJobFromMac(string printerMAC)
        {
            var pendingJob = await _printJobRepository.GetFirstPendingJobForPrinter(printerMAC);

            return pendingJob;
        }

        public async Task<IEnumerable<PrintJob>> GetRecentPrintJobsAsync(int count)
        {
            return await _printJobRepository.GetRecentPrintJobsAsync(count);
        }

        public bool IsJobAvailable(string printerMAC)
        {
            // Check if the provided printerMAC matches the hard-coded MAC address
            if (printerMAC == HardCodedPrinterMAC)
            {
                // Check if there are any pending print jobs for the specific printer
                var availableJobs = _printJobRepository.GetPendingJobsForPrinter(printerMAC);

                return availableJobs.Any(); // Return true if there are available jobs, false otherwise
            }

            // Return false for other printers (or handle accordingly based on your actual requirements)
            return false;
        }

        public void RemovePrintJob(string jobId)
        {
            if (_printJobRepository.TryGetPrintJob(jobId, out var printJob))
            {
                _printJobRepository.Delete(printJob);
            }
        }

        public bool TryGetPrintJob(string jobId, out PrintJob jobData)
        {
            return _printJobRepository.TryGetPrintJob(jobId, out jobData);
        }


        public async Task<PrintJob> UpdateJobStatus(int jobId, PrintJobStatus status)
        {
            var job = await _printJobRepository.GetPrintJobByIdAsync(jobId);

            if (job != null)
            {
                job.Status = status;
                await _printJobRepository.UpdateAsync(job);
                return job;  // Return the updated job
            }

            return null;
        }


    }
}
