using Microsoft.EntityFrameworkCore;
using Cloud_API.Interfaces;
using Cloud_API.Models;
using Cloud_API.Contexts;

namespace Cloud_API.Helpers.Services
{
    public class ReceiptService : IReceiptService
    {
        private readonly PrintContext context;

        public ReceiptService(PrintContext _context)
        {
            context = _context;
        }


        public async Task<Receipt?> CreateReceiptAsync(Receipt receipt)
        {
            context.Receipts.Add(receipt);
            await context.SaveChangesAsync();
            return receipt;
        }



        public async Task<List<Receipt>> GetAllReceiptsAsync()
        {
            var receipts = await context.Receipts.ToListAsync();
            return receipts;
        }

        public async Task<Receipt?> GetSingleReceiptAsync(int id)
        {
            var singleReceipt = await context.Receipts.FindAsync(id);
            if (singleReceipt == null)
                return null;

            return singleReceipt;
        }
        public async Task<bool> DeleteReceiptAsync(int id)
        {
            var receiptToDelete = await context.Receipts.FindAsync(id);
            if (receiptToDelete == null)
                return false;

            context.Receipts.Remove(receiptToDelete);
            await context.SaveChangesAsync();

            return true;
        }

    }
}

