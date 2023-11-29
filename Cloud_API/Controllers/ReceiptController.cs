using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Cloud_API.Interfaces;
using Cloud_API.Models;

namespace Cloud_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReceiptController : ControllerBase
    {
        private readonly IReceiptService receiptService;

        public ReceiptController(IReceiptService _receiptService)
        {
            receiptService = _receiptService;
        }



        [HttpGet]
        public async Task<ActionResult<List<Receipt>>> GetAllReceiptsAsync()
        {
            return await receiptService.GetAllReceiptsAsync();
        }


        [HttpGet("{id}")]

        public async Task<ActionResult<Receipt>> GetSingleReceiptAsync(int id)
        {
            var result = await receiptService.GetSingleReceiptAsync(id);
            if (result == null)
                return NotFound("Couldn't find ID");

            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<Receipt>> CreateReceiptAsync(Receipt receipts)
        {
            var result = await receiptService.CreateReceiptAsync(receipts);
            return CreatedAtAction(nameof(GetSingleReceiptAsync), new { id = result.Id }, result);
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteReceiptAsync(int id)
        {
            var result = await receiptService.DeleteReceiptAsync(id);

            if (result == null)
            {
                return NotFound("Couldn't find ID");
            }

            return Ok(result);
        }



    }
}
