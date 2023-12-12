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
            return await context.Receipts.ToListAsync();
        }

        public async Task<Receipt?> GetSingleReceiptAsync(int id)
        {
            try
            {
                var singleReceipt = await context.Receipts.FindAsync(id);
                return singleReceipt;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception during receipt creation: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> DeleteReceiptAsync(int id)
        {
            var receiptToDelete = await context.Receipts.FindAsync(id);

            if (receiptToDelete == null)
            {
                return false;
            }

            context.Receipts.Remove(receiptToDelete);
            await context.SaveChangesAsync();

            return true;
        }
    }
}

