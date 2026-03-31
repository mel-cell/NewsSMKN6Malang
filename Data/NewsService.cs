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
        private readonly string _adminEmail;
        private readonly string _adminPassword;
        private string? _token;

        public NewsService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiBaseUrl = configuration["ApiSettings:BaseUrl"]?.TrimEnd('/') ?? "";
            _adminEmail = configuration["ApiSettings:AdminEmail"] ?? "";
            _adminPassword = configuration["ApiSettings:AdminPassword"] ?? "";
        }

        private async Task EnsureAuthenticatedAsync()
        {
            if (!string.IsNullOrEmpty(_token)) return;

            try
            {
                var loginData = new { email = _adminEmail, password = _adminPassword };
                var response = await _httpClient.PostAsJsonAsync($"{_apiBaseUrl}/api/users/login", loginData);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
                    _token = result?.Token;
                    if (!string.IsNullOrEmpty(_token))
                    {
                        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("JWT", _token);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Login failed: {ex.Message}");
            }
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

        private class LoginResponse
        {
            [JsonPropertyName("token")]
            public string? Token { get; set; }
        }
    }
}
