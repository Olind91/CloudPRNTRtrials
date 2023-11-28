using Cloud_API.Contexts;
using Cloud_API.Models;
using Microsoft.EntityFrameworkCore;

namespace Cloud_API.Helpers.Repositories
{
    public class PrintJobRepository : Repo<PrintJob>
    {
        private readonly PrintContext _context;
        
        public PrintJobRepository(PrintContext context) : base(context)
        {
            _context= context;
        }

        public async Task<IEnumerable<PrintJob>> GetRecentPrintJobsAsync(int count)
        {
            return await _context.Set<PrintJob>()
                .OrderByDescending(p => p.CreatedAt)
                .Take(count)
                .ToListAsync();
        }
    }
}
