using Cloud_API.Contexts;
using Cloud_API.Models;

namespace Cloud_API.Helpers.Repositories
{
    public class PrintJobRepository : Repo<PrintJob>
    {
        public PrintJobRepository(PrintContext context) : base(context)
        {
        }
    }
}
