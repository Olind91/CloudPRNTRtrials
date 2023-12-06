using Cloud_API.Contexts;
using Cloud_API.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cloud_API.Helpers.Repositories
{
    public class PrintJobRepository : Repo<PrintJob>
    {
        private readonly PrintContext _context;

        public PrintJobRepository(PrintContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PrintJob>> GetRecentPrintJobsAsync(int count)
        {
            return await _context.Set<PrintJob>()
                .OrderByDescending(p => p.CreatedAt)
                .Take(count)
                .ToListAsync();
        }

        public IEnumerable<PrintJob> GetPendingJobsForPrinter(string printerMAC)
        {
            return _context.PrintJobs
                .Where(job => job.PrinterMAC == printerMAC && job.Status == PrintJobStatus.Pending)
                .ToList();
        }

        public bool TryGetPrintJob(string jobId, out PrintJob? jobData)
        {
            if (int.TryParse(jobId, out var jobIdInt))
            {
                jobData = _context.PrintJobs.FirstOrDefault(job => job.Id == jobIdInt);
                return jobData != null;
            }

            jobData = null;
            return false;
        }

        public async Task<PrintJob> GetFirstPendingJobForPrinter(string printerMAC)
        {
            return await _context.PrintJobs
                .FirstOrDefaultAsync(job => job.PrinterMAC == printerMAC && job.Status == PrintJobStatus.Pending);
        }

        public async Task<PrintJob> GetPrintJobByIdAsync(int jobId)
        {
            return await _context.PrintJobs.FindAsync(jobId);
        }

    }
}
