using Cloud_Blazor.Helpers.Interfaces;
using Cloud_Blazor.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Cloud_Blazor.Helpers.Services
{
    public class PrintJobService : IPrintJobService
    {
        private readonly HttpClient _http;
        private readonly NavigationManager _navigationManager;

        public PrintJobService(HttpClient http, NavigationManager navigationManager)
        {
            _http = http;
            _navigationManager = navigationManager;
        }

        public List<PrintJob> PrintJobs { get; set; } = new List<PrintJob>();

        public async Task CreatePrintJob(PrintJob printJob)
        {
            var response = await _http.PostAsJsonAsync("https://192.168.1.159:45455/api/cloudprnt/job", printJob);
            Console.WriteLine($"Response Status Code: {response.StatusCode}");
            Console.WriteLine($"Response Content: {await response.Content.ReadAsStringAsync()}");

            if (response.IsSuccessStatusCode)
            {
                var createdPrintJob = await response.Content.ReadFromJsonAsync<PrintJob>();

                if (createdPrintJob != null && createdPrintJob.Id > 0)
                {
                    // Update the local PrintJobs list with the created print job
                    PrintJobs.Add(createdPrintJob);

                }
                else
                {
                    // Handle the case where the print job creation failed or the ID is not available
                    Console.WriteLine("Error: Print job creation failed or ID not available");
                }
            }
            else
            {
                // Handle error if necessary
                Console.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
            }
        }

        public async Task<List<PrintJob>> GetPrintJobs()
        {
            var result = await _http.GetFromJsonAsync<List<PrintJob>>("https://192.168.1.159:45455/api/cloudprnt");
            PrintJobs = result ?? new List<PrintJob>();
            return PrintJobs;
        }
    }
}
