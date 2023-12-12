using Cloud_Blazor.Models;

namespace Cloud_Blazor.Helpers.Interfaces
{
    public interface IReceiptService
    {
        List<Receipt> Receipts { get; set; }
        Task<Receipt?> GetReceiptById(int id);
        Task CreateReceipt(Receipt receipt);
        Task DeleteReceipt(int id);
        Task<List<Receipt>> GetReceipts();
    }
}
