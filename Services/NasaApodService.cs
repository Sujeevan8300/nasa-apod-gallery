using NasaApodGallery.DTOs;
using System.Text.Json;

namespace NasaApodGallery.Services
{
    public class NasaApodService : INasaApodService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        // Constructor - receives HttpClient and configuration
        public NasaApodService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiKey = configuration["Nasa:Apikey"];
        }

        public async Task<List<ApodDto>> GetApodRangeAsync(DateTime startDate, DateTime endDate)
        {
            // Build the NASA API URL
            string url = $"https://api.nasa.gov/planetary/apod" +
                         $"?api_key={_apiKey}" +
                         $"&start_date={startDate:yyyy-MM-dd}" +
                         $"&end_date={endDate:yyyy-MM-dd}";

            // Call NASA
            HttpResponseMessage response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            // Read the JSON text
            string json = await response.Content.ReadAsStringAsync();

            // Convert JSON into List<ApodDto>
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true   // Ignores capital/small letter differences
            };

            List<ApodDto> result = JsonSerializer.Deserialize<List<ApodDto>>(json, options);

            return result ?? new List<ApodDto>();   
        }
    }
}