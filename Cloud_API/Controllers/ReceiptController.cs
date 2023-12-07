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




        [HttpPost]
        public async Task<ActionResult<Receipt>> CreateReceiptAsync(Receipt receipts)
        {
            try
            {
                // Create the receipt
                var createdReceipt = await receiptService.CreateReceiptAsync(receipts);

                if (createdReceipt == null || createdReceipt.Id <= 0)
                {
                    // Handle the case where the ID is not properly assigned
                    Console.WriteLine("Error: Receipt ID not properly assigned.");
                    return BadRequest("Error: Receipt ID not properly assigned.");
                }

                // Return the created receipt directly
                return Ok(createdReceipt);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Exception during receipt creation: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }


        [HttpGet("{id}")]

        public async Task<ActionResult<Receipt>> GetSingleReceiptAsync(int id)
        {
            var result = await receiptService.GetSingleReceiptAsync(id);
            if (result == null)
                return NotFound("Couldn't find ID");

            return Ok(result);
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
