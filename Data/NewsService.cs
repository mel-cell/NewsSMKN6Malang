#nullable enable
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Text.Json.Serialization;

namespace NewsSMKN6Malang.Data
{
    public class NewsService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiBaseUrl;
        private string? _token;
        
        public bool IsAuthenticated => !string.IsNullOrEmpty(_token);

        public NewsService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiBaseUrl = configuration["ApiSettings:BaseUrl"]?.TrimEnd('/') ?? "";
        }

        public async Task<bool> LoginAsync(string email, string password)
        {
            try
            {
                var loginData = new { email, password };
                var response = await _httpClient.PostAsJsonAsync($"{_apiBaseUrl}/api/users/login", loginData);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
                    _token = result?.Token;
                    if (!string.IsNullOrEmpty(_token))
                    {
                        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("JWT", _token);
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Login failed: {ex.Message}");
            }
            
            return false;
        }

        public void Logout()
        {
            _token = null;
            _httpClient.DefaultRequestHeaders.Authorization = null;
        }

        public async Task<List<NewsItem>> GetNewsAsync()
        {
            try
            {
                // We don't always need auth for GET news, but if we do, we can call EnsureAuthenticatedAsync
                // await EnsureAuthenticatedAsync(); 

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

        public async Task<bool> CreateNewsAsync(NewsCreateDto news)
        {
            if (!IsAuthenticated) return false;
            if (string.IsNullOrEmpty(_token)) return false;

            try
            {
                var response = await _httpClient.PostAsJsonAsync($"{_apiBaseUrl}/api/news", news);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating news: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> UpdateNewsAsync(string id, NewsCreateDto news)
        {
            if (!IsAuthenticated) return false;
            if (string.IsNullOrEmpty(_token)) return false;

            try
            {
                var response = await _httpClient.PatchAsJsonAsync($"{_apiBaseUrl}/api/news/{id}", news);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating news: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteNewsAsync(string id)
        {
            if (!IsAuthenticated) return false;
            if (string.IsNullOrEmpty(_token)) return false;

            try
            {
                var response = await _httpClient.DeleteAsync($"{_apiBaseUrl}/api/news/{id}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting news: {ex.Message}");
                return false;
            }
        }

        private class LoginResponse
        {
            [JsonPropertyName("token")]
            public string? Token { get; set; }
        }
    }

    public class NewsCreateDto
    {
        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;

        [JsonPropertyName("excerpts")]
        public string Excerpts { get; set; } = string.Empty;

        [JsonPropertyName("content")]
        public object? Content { get; set; }

        [JsonPropertyName("_status")]
        public string Status { get; set; } = "published";
    }
}
