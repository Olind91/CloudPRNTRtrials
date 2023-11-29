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
    public class ReceiptService : IReceiptService
    {
        private readonly HttpClient _http;
        private readonly NavigationManager _navigationManager;

        public ReceiptService(HttpClient http, NavigationManager navigationManager)
        {
            _http = http;
            _navigationManager = navigationManager;
        }

        public List<Receipt> Receipts { get; set; } = new List<Receipt>();

        public async Task CreateReceipt(Receipt receipt)
        {
            var result = await _http.PostAsJsonAsync("https://localhost:7029/api/Receipt", receipt);

            if (result.IsSuccessStatusCode)
            {
                var createdReceipt = await result.Content.ReadFromJsonAsync<Receipt>();
                _navigationManager.NavigateTo($"/receipts/{createdReceipt.Id}");
            }
            else
            {
            }
        }

        public async Task DeleteReceipt(int id)
        {
            var result = await _http.DeleteAsync($"https://localhost:7029/api/Receipt/{id}");
            if (result.IsSuccessStatusCode)
            {
                await GetReceipts();
                _navigationManager.NavigateTo("Receipts");
            }
        }

        public async Task<Receipt?> GetReceiptById(int id)
        {
            var result = await _http.GetAsync($"https://localhost:7029/api/Receipt/{id}");
            if(result.StatusCode == HttpStatusCode.OK)
            {
                return await result.Content.ReadFromJsonAsync<Receipt?>();
            }
            return null;
        }

        public async Task<List<Receipt>> GetReceipts()
        {
            var result = await _http.GetFromJsonAsync<List<Receipt>>("https://localhost:7029/api/Receipt");
            Receipts = result ?? new List<Receipt>();
            return Receipts;
        }
    }
}
