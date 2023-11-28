using Cloud_API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using StarMicronics.CloudPrnt;
using StarMicronics.CloudPrnt.CpMessage;
using System;
using System.Text;
using System.Threading.Tasks;

[ApiController]
[Route("api/cloudprnt")]
public class CloudPRNTController : ControllerBase
{
    private readonly IPrintJobService _printJobService;

    public CloudPRNTController(IPrintJobService printJobService)
    {
        _printJobService = printJobService;
    }

    [HttpPost("job")]
    public async Task<IActionResult> CreatePrintJob()
    {
        // Generate receipt content
        string receiptContent = GenerateReceiptContent();

        // Create a new print job with the generated receipt content
        var printJobId = await _printJobService.CreatePrintJobAsync(receiptContent);

        return Ok(printJobId); // Return the print job ID to the client
    }

    private string GenerateReceiptContent()
    {
        // Simple example: Generate a receipt with a header, items, and a total
        StringBuilder receipt = new StringBuilder();
        receipt.AppendLine("----- Receipt -----");
        receipt.AppendLine("Item 1       $10.00");
        receipt.AppendLine("Item 2       $15.00");
        receipt.AppendLine("Total        $25.00");
        receipt.AppendLine("-------------------");

        return receipt.ToString();
    }

    [HttpPost("poll")]
    public IActionResult HandleCloudPRNTPoll([FromBody] string request)
    {
        PollRequest pollRequest = PollRequest.FromJson(request);
        Console.WriteLine($"Received CloudPRNT request from {pollRequest.printerMAC}, status: {pollRequest.statusCode}");

        // Create a response object
        PollResponse pollResponse = new PollResponse();

        // Example logic: Check if a job is available
        if (_printJobService.IsJobAvailable(pollRequest.printerMAC))
        {
            pollResponse.jobReady = true;
            pollResponse.mediaTypes = new List<string> { "text/vnd.star.markup" }; // Specify supported media types
        }
        else
        {
            pollResponse.jobReady = false;
            pollResponse.mediaTypes = null;
        }

        return Ok(pollResponse);
    }

    [HttpPost("print")]
    public IActionResult PrintJob([FromBody] string jobId)
    {
        if (_printJobService.TryGetPrintJob(jobId, out var jobData))
        {
            // Print the job
            // Add logic here to interact with the CloudPRNT SDK to send the print job to the printer

            // Remove the job from the service after printing (optional)
            _printJobService.RemovePrintJob(jobId);

            return Ok("Job printed successfully");
        }
        else
        {
            return NotFound("Job not found");
        }
    }
}