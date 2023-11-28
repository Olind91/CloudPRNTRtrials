using Cloud_API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StarMicronics.CloudPrnt;
using StarMicronics.CloudPrnt.CpMessage;
using System;
using System.Collections.Generic;
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
    public async Task<IActionResult> CreatePrintJob([FromBody] string content)
    {
        var printJob = await _printJobService.CreatePrintJobAsync(content);
        return Ok(printJob.Id); // Return the print job ID to the client
    }

    [HttpPost("poll")]
    public IActionResult HandleCloudPRNTPoll([FromBody] string request)
    {
        PollRequest pollRequest = PollRequest.FromJson(request);
        Console.WriteLine($"Received CloudPRNT request from {pollRequest.printerMAC}, status: {pollRequest.statusCode}");

        // Create a response object
        PollResponse pollResponse = new PollResponse();

        // Example logic: Check if a job is available in your database
        if (_printJobService.IsJobAvailable(pollRequest.printerMAC))
        {
            pollResponse.jobReady = true;
            pollResponse.mediaTypes = new List<string>(Document.GetOutputTypesFromType("text/vnd.star.markup"));
        }
        else
        {
            pollResponse.jobReady = false;
            pollResponse.mediaTypes = null;
        }

        return Ok(JsonConvert.SerializeObject(pollResponse));
    }

    [HttpGet("job")]
    public async Task<IActionResult> GetPrintJob([FromQuery] string type)
    {
        // Example: Create a simple markup language job
        StringBuilder job = new StringBuilder();
        job.Append("Hello World!\n");
        job.Append("[barcode: type code39; data 12345; height 10mm]\n");
        job.Append("[cut]");
        byte[] jobData = Encoding.UTF8.GetBytes(job.ToString());

        // Save the job in the database
        var jobId = await _printJobService.CreatePrintJobAsync(Encoding.UTF8.GetString(jobData));

        // Return the job ID to the client
        return Ok(jobId);
    }

    [HttpPost("print")]
    public IActionResult PrintJob([FromBody] string jobId)
    {
        if (_printJobService.TryGetPrintJob(jobId, out var jobData))
        {
            // Print the job
            // Add logic here to interact with the CloudPRNT SDK to send the print job to the printer

            // Remove the job from the database after printing (optional)
            _printJobService.RemovePrintJob(jobId);

            return Ok("Job printed successfully");
        }
        else
        {
            return NotFound("Job not found");
        }
    }
}