using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json.Nodes;
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
        public async Task<IEnumerable<WeatherDTO>> GetWeather(WeatherRequestDTO weatherRequestDTO)
        {
            int cnt = weatherRequestDTO.Cnt ?? 5;

            string baseUrl = _config["WeatherApi:Url"];
            string apiKey = _config["WeatherApi:ServiceApiKey"];

            if (!(weatherRequestDTO.Lon == null || weatherRequestDTO.Lat == null))
            {
                decimal lat = Math.Round((decimal)weatherRequestDTO.Lat, 2);
                decimal lon = Math.Round((decimal)weatherRequestDTO.Lon, 2);

                _logger.LogInformation("Location Request");
                string url = $"{baseUrl}/data/2.5/forecast?lat={lat}" +
                    $"&lon={lon}&cnt={cnt}&appid={apiKey}";

                try
                {
                    using (var httpClient = _httpClientFactory.CreateClient())
                    {
                        HttpResponseMessage res = await httpClient.GetAsync(url);
                        if (res.IsSuccessStatusCode)
                        {
                            string responseString = await res.Content.ReadAsStringAsync();
                            var result = JsonObject.Parse(responseString);
                            
                            if (result == null)
                            {
                                _logger.LogInformation("API returned empty json.");
                                // TODO: Throw error for no content
                                throw new Exception();
                            }

                            var data = result["list"];

                            if (data == null)
                            {
                                _logger.LogInformation("API returned empty list of data.");
                                // TODO: Throw error for no content
                                throw new Exception();
                            }

                            List<WeatherDTO> list = new List<WeatherDTO>();

                            var dataArray = (JsonArray) data;
                            foreach (var element in dataArray)
                            {
                                WeatherDTO weatherDTO = new WeatherDTO();
                                weatherDTO.Temp = (decimal)element["main"]["temp"];
                                weatherDTO.TempMin = (decimal)element["main"]["temp_min"];
                                weatherDTO.TempMax = (decimal)element["main"]["temp_max"];
                                weatherDTO.Humidity = (int)element["main"]["humidity"];
                                if (element["weather"] != null)
                                {
                                    weatherDTO.Weather = (string)element["weather"][0]["main"];
                                    weatherDTO.Description = (string)element["weather"][0]["description"];
                                }
                                if (element["clouds"] != null)
                                {
                                    weatherDTO.Cloud = (int)element["clouds"]["all"];
                                }
                                weatherDTO.Time = (string)element["dt_txt"];
                                list.Add(weatherDTO);
                            }
                            return list;
                        }
                        else
                        {
                            return Enumerable.Empty<WeatherDTO>();
                        }
                    }
                    return Enumerable.Empty<WeatherDTO>();

                }
                catch (Exception e) 
                {
                    _logger.LogError(e.ToString());
                }

                return Enumerable.Empty<WeatherDTO>();
            }
            else if (!String.IsNullOrEmpty(weatherRequestDTO.City))
            {
                _logger.LogInformation("City Request");
                string url = $"{baseUrl}/geo/1.0/direct?q={weatherRequestDTO.City}&limit={1}&appid={apiKey}";

                return Enumerable.Empty<WeatherDTO>();
            }
            else
            {
                // TODO: Throw Error, because no values
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
        Task<IEnumerable<WeatherDTO>> GetWeather(WeatherRequestDTO weatherRequestDTO);
        IEnumerable<String> GetListCities();
    }
}
