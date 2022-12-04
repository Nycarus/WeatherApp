using Microsoft.Extensions.Logging;
using WeatherApp.Controllers;

namespace WeatherApp.Models
{
    public class WeatherService : IWeatherService
    {
        private readonly ILogger<WeatherService> _logger;
        private readonly IConfiguration _config;
        private IHttpClientFactory _httpClientFactory;
        public WeatherService(
            ILogger<WeatherService> logger,
            IHttpClientFactory httpClientFactory,
            IConfiguration config) {
            _logger = logger;
            _config = config;
            _httpClientFactory = httpClientFactory;
        }
        public IEnumerable<WeatherDTO> GetWeather(WeatherRequestDTO weatherRequestDTO)
        {
            int count = weatherRequestDTO.cnt ?? 7;
            string baseUrl = _config["WeatherApi:Url"];
            string apiKey = _config["WeatherApi:ServiceApiKey"];

            if (!(String.IsNullOrEmpty(weatherRequestDTO.lon) || String.IsNullOrEmpty(weatherRequestDTO.lat)))
            {
                string url = $"{baseUrl}/data/2.5/forecast/daily?lat={weatherRequestDTO.lat}" +
                    $"&lon={weatherRequestDTO.lon}&cnt={count}&appid={apiKey}";

                return Enumerable.Empty<WeatherDTO>();
            }
            else if (!String.IsNullOrEmpty(weatherRequestDTO.city))
            {
                string url = $"{baseUrl}/geo/1.0/direct?q={weatherRequestDTO.city}&limit={1}&appid={apiKey}";

                return Enumerable.Empty<WeatherDTO>();
            }
            else
            {
                return Enumerable.Empty<WeatherDTO>();
            }
        }

        public IEnumerable<String> GetListCities()
        {
            return new string[] {"city_test", "city_test_2"};
        }
    }

    public interface IWeatherService
    {
        IEnumerable<WeatherDTO> GetWeather(WeatherRequestDTO weatherRequestDTO);
        IEnumerable<String> GetListCities();
    }
}
