#nullable enable
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Text.Json.Serialization;

namespace NewsSMKN6Malang.Data
{
    public class NewsService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiBaseUrl;

        public NewsService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiBaseUrl = configuration["ApiSettings:BaseUrl"]?.TrimEnd('/') ?? "";
        }

        public async Task<List<NewsItem>> GetNewsAsync()
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<PayloadResponse<NewsItem>>($"{_apiBaseUrl}/api/news");
                return response?.Docs ?? new List<NewsItem>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching news: {ex.Message}");
                return new List<NewsItem>();
            }
        }

        public async Task<NewsItem?> GetNewsByIdAsync(string id)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<NewsItem>($"{_apiBaseUrl}/api/news/{id}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching news by id: {ex.Message}");
                return null;
            }
        }
    }
}
