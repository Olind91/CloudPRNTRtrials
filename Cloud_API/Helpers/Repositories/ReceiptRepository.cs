
using Cloud_API.Contexts;
using Cloud_API.Helpers.Repositories;
using Cloud_API.Models;

namespace Cloud_Api.Helpers.Repositories
{
    public class ReceiptRepository : Repo<Receipt>
    {
        
        public ReceiptRepository(PrintContext context) : base(context)
        {
        }

    }
}
