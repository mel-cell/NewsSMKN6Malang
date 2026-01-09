using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace NewsSMKN6Malang.Data
{
    public class NewsService
    {
        private readonly string _apiBaseUrl;

        public NewsService(IConfiguration configuration)
        {
            _apiBaseUrl = configuration["ApiSettings:BaseUrl"];
        }

        public Task<List<NewsItem>> GetNewsAsync()
        {
            // Mock data simulating API response
            var news = new List<NewsItem>
            {
                new NewsItem
                {
                    Id = 1,
                    Title = "Juara 1 Lomba Kompetensi Siswa Tingkat Nasional",
                    Description = "Siswa SMKN 6 Malang berhasil meraih medali emas dalam ajang LKS Nasional bidang Cloud Computing.",
                    Category = "Prestasi",
                    ImageUrl = "https://via.placeholder.com/400x250?text=Prestasi+Siswa",
                    PublishedDate = DateTime.Now.AddDays(-2)
                },
                new NewsItem
                {
                    Id = 2,
                    Title = "Kunjungan Industri ke PT. Telkom Indonesia",
                    Description = "Siswa jurusan SIJA melakukan kunjungan industri untuk mempelajari infrastruktur jaringan modern.",
                    Category = "Kegiatan",
                    ImageUrl = "https://via.placeholder.com/400x250?text=Kunjungan+Industri",
                    PublishedDate = DateTime.Now.AddDays(-5)
                },
                new NewsItem
                {
                    Id = 3,
                    Title = "Penerimaan Peserta Didik Baru 2026 Dibuka",
                    Description = "Segera daftarkan diri anda untuk menjadi bagian dari keluarga besar SMK Negeri 6 Malang.",
                    Category = "Pengumuman",
                    ImageUrl = "https://via.placeholder.com/400x250?text=PPDB+2026",
                    PublishedDate = DateTime.Now.AddDays(-10)
                }
            };

            return Task.FromResult(news);
        }
    }
}
