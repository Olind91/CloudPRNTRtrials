using Cloud_API.Interfaces;
using Cloud_API.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using System.Text;


[ApiController]
[Route("api/cloudprnt")]
public class CloudPRNTController : ControllerBase
{
    private readonly IPrintJobService _printJobService;

    public CloudPRNTController(IPrintJobService printJobService)
    {
        _printJobService = printJobService;
    }

    #region CREATE RECIEPT & PRINTJOB
    [HttpPost("job")]
    public async Task<IActionResult> CreatePrintJob([FromBody] PrintJob printJob)
    {
        if (printJob == null)
        {
            return BadRequest("Invalid PrintJob object in the request.");
        }

        // Extract content from the PrintJob object
        string receiptContent = printJob?.Content;

        // Validate that content is not null or empty
        if (string.IsNullOrWhiteSpace(receiptContent))
        {
            return BadRequest("Invalid or empty Content field in the PrintJob object.");
        }

        // Create a new print job with the extracted receipt content
        var printJobId = await _printJobService.CreatePrintJobAsync(receiptContent);

        return Ok(printJobId); // Return the print job ID to the client
    }
    #endregion


    #region POST-REQUEST/RESPONSE
    [HttpPost]
    public IActionResult HandleCloudPRNTPoll([FromBody] PollRequest pollRequest)
    {
        // Log the incoming request details
        Console.WriteLine($"Received CloudPRNT request from {pollRequest.printerMAC}, status: {pollRequest.statusCode}");
        Console.WriteLine(JsonConvert.SerializeObject(pollRequest));

        // Create a response object
        PollResponse pollResponse = new PollResponse();

        // Check if there is a print job available for the specified printer
        if (_printJobService.IsJobAvailable(pollRequest.printerMAC))
        {
            // Get the first pending print job for the printer
            var pendingJob = _printJobService.FindJobFromMac(pollRequest.printerMAC).Result;

            if (pendingJob != null)
            {
                // Indicate that a job is ready and specify the supported media types
                pollResponse.jobReady = true;
                pollResponse.mediaTypes = new List<string> { "text/plain" };

                // Set the jobToken in the response using the ID of the pending job
                pollResponse.jobToken = pendingJob.Id.ToString();
                pollResponse.jobGetUrl = "https://192.168.1.159:45455/api/cloudprnt/";
            }
            else
            {
                // Indicate that no job is ready
                pollResponse.jobReady = false;
                pollResponse.mediaTypes = null;
            }
        }
        else
        {
            // Indicate that no job is ready
            pollResponse.jobReady = false;
            pollResponse.mediaTypes = null;
        }

        // Log the generated response details
        Console.WriteLine(JsonConvert.SerializeObject(pollResponse));

        // Serialize the response object to JSON
        string jsonResponse = JsonConvert.SerializeObject(pollResponse);

        // Return the JSON response
        return Ok(jsonResponse);
    }
    #endregion


    #region GET-RESPONSE
    [HttpGet]
    public async Task<IActionResult> GetPrintJob([FromQuery] string? status, [FromQuery] string? printerMAC, [FromQuery] string? statusCode, [FromQuery] string token)
    {
        try
        {
            // Log to check if the method is reached
            Console.WriteLine("GetPrintJob method reached");
           

            // Retrieve the printerMAC from the request headers
            string headerPrinterMAC = Request.Headers["Printer-MAC"].FirstOrDefault() ?? "00:11:62:1e:a4:e1";

            // Log the retrieved printerMAC
            Console.WriteLine($"Header Printer MAC: {headerPrinterMAC}");

            // Check if the provided printerMAC matches the hard-coded MAC address
            if (headerPrinterMAC != "00:11:62:1e:a4:e1")
            {
                // Log the unauthorized message
                Console.WriteLine("Unauthorized printer");

                // Return a response indicating that the printer is not authorized
                return Unauthorized("Unauthorized printer");
            }

            // Log to check if the authorization check passed
            Console.WriteLine("Authorization check passed");

            // Parse the jobToken from the JSON
            if (string.IsNullOrEmpty(token))
            {
                // Log an error if jobToken is missing
                Console.WriteLine("Missing jobToken in the request");
                return BadRequest("Missing jobToken in the request");
            }

            // Convert jobToken to integer
            if (!int.TryParse(token, out int jobId))
            {
                // Log an error if parsing fails
                Console.WriteLine("Invalid jobToken format");
                return BadRequest("Invalid jobToken format");
            }

            // Check if there is a print job with the specified jobId
            var printJobTask = _printJobService.GetSinglePrintJobAsync(jobId);
            var printJob = await printJobTask;

            // Log the job details
            Console.WriteLine($"Print job: {(printJob != null ? printJob.Id.ToString() : "null")}, {printJob?.Content}");

            if (printJob != null && printJob.PrinterMAC == headerPrinterMAC)
            {
                // Set the print job status to "InProgress" in the service
                await _printJobService.UpdateJobStatus(printJob.Id, PrintJobStatus.InProgress);

                // Log the success message
                Console.WriteLine("Print job found");

                // Return the plain text content with OK status
                return Ok(printJob.Content);
            }

            // Log the failure message
            Console.WriteLine("Print job not found");

            // Return a response indicating that no matching print job is available
            return NotFound("Print job not found");
        }
        catch (Exception ex)
        {
            // Log any exceptions that occur
            Console.WriteLine($"Exception: {ex.Message}");
            return StatusCode(500, "Internal Server Error");
        }
    }
    #endregion


    


}