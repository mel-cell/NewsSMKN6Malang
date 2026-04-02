#nullable enable
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace NewsSMKN6Malang.Data
{
    public class PayloadResponse<T>
    {
        [JsonPropertyName("docs")]
        public List<T> Docs { get; set; } = new();

        [JsonPropertyName("totalDocs")]
        public int TotalDocs { get; set; }

        [JsonPropertyName("limit")]
        public int Limit { get; set; }

        [JsonPropertyName("totalPages")]
        public int TotalPages { get; set; }

        [JsonPropertyName("page")]
        public int Page { get; set; }
    }

    public class NewsItem
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;

        [JsonPropertyName("content")]
        public object? Content { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; } = string.Empty;

        [JsonPropertyName("category")]
        public string? Category { get; set; }

        [JsonPropertyName("heroImage")]
        public ImageInfo? HeroImage { get; set; }

        [JsonPropertyName("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("updatedAt")]
        public DateTime UpdatedAt { get; set; }

        [JsonPropertyName("excerpts")]
        public string? Description { get; set; }

        // Helper property to map API fields to UI fields
        public string DisplayImageUrl => HeroImage?.Url != null
            ? (HeroImage.Url.StartsWith("http") ? HeroImage.Url : $"https://test.smkn6malang.sch.id{HeroImage.Url}")
            : "https://via.placeholder.com/400x250?text=No+Image";
        public string DisplayCategory => Category ?? "Berita";
        public DateTime DisplayDate => CreatedAt;
    }

    public class ImageInfo
    {
        [JsonPropertyName("url")]
        public string Url { get; set; } = string.Empty;

        [JsonPropertyName("alt")]
        public string? Alt { get; set; }
    }
}
