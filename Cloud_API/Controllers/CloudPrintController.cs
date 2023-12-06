using Cloud_API.Helpers.Repositories;
using Cloud_API.Interfaces;
using Cloud_API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using StarMicronics.CloudPrnt;
using StarMicronics.CloudPrnt.CpMessage;
using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

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
        receipt.AppendLine("<text>----- Receipt -----</text>");
        receipt.AppendLine("<text>Item 1       $10.00</text>");
        receipt.AppendLine("<text>Item 2       $15.00</text>");
        receipt.AppendLine("<text>Total        $25.00</text>");
        receipt.AppendLine("<cut/>"); 

        return receipt.ToString();
    }

    [HttpPost("poll")]
    public IActionResult HandleCloudPRNTPoll([FromBody] PollRequest pollRequest)
    {
        Console.WriteLine(JsonConvert.SerializeObject(pollRequest));
        Console.WriteLine($"Received CloudPRNT request from {pollRequest.printerMAC}, status: {pollRequest.statusCode}");

        // Create a response object
        PollResponse pollResponse = new PollResponse();

        if (_printJobService.IsJobAvailable(pollRequest.printerMAC))
        {
            pollResponse.jobReady = true;
            pollResponse.mediaTypes = new List<string> { "text/vnd.star.markup" };
            pollResponse.deleteMethod = "DELETE";// Set the deleteMethod property
        }
        else
        {
            pollResponse.jobReady = false;
            pollResponse.mediaTypes = null;
            pollResponse.deleteMethod = "GET"; // Set the deleteMethod property
        }

        string jsonResponse = JsonConvert.SerializeObject(pollResponse);
        Console.WriteLine(JsonConvert.SerializeObject(pollResponse));
        return Ok(jsonResponse);
    }

    [HttpGet("printjob")]
    public async Task<IActionResult> GetPrintJob([FromQuery] string printerMAC, [FromQuery] string mediaType)
    {

        // Check if the provided printerMAC matches the hard-coded MAC address
        if (printerMAC != "00:11:62:1e:a4:e1")
        {
            // Return a response indicating that the printer is not authorized
            return Unauthorized("Unauthorized printer");
        }

        // Check if there are any pending print jobs for the specific printer
        if (_printJobService.IsJobAvailable(printerMAC))
        {
            // Get the first pending print job
            var printJob = await _printJobService.FindJobFromMac(printerMAC);

            // Set the print job status to "InProgress" in the service
            await _printJobService.UpdateJobStatus(printJob.Id, PrintJobStatus.InProgress);

            // Provide the print job content in the specified media type from the service
            byte[] printJobData = Encoding.UTF8.GetBytes(printJob.Content);

            // Set the response content type based on the requested media type
            Response.Headers.Add("Content-Type", mediaType);

            // Set the response status
            Response.StatusCode = (int)HttpStatusCode.OK;

            // Return the file content
            return File(printJobData, mediaType);

        }

        // Return a response indicating that no print jobs are available
        return NotFound("No print jobs available");
    }



}