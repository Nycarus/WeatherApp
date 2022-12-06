using Microsoft.AspNetCore.DataProtection.KeyManagement;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Nodes;
using WeatherApp.Models.DTO;

namespace WeatherApp.Models.Services
{
    public class WeatherService : IWeatherService
    {
        private readonly ILogger<WeatherService> _logger;
        private readonly IConfiguration _config;
        private IHttpClientFactory _httpClientFactory;
        public WeatherService(
            ILogger<WeatherService> logger,
            IHttpClientFactory httpClientFactory,
            IConfiguration config)
        {
            _logger = logger;
            _config = config;
            _httpClientFactory = httpClientFactory;
        }
        public async Task<IEnumerable<WeatherDTO>> GetWeather(WeatherRequestDTO weatherRequestDTO)
        {
            int cnt = weatherRequestDTO.Cnt ?? 40;

            string baseUrl = _config["WeatherApi:Url"];
            string apiKey = _config["WeatherApi:ServiceApiKey"];

            if (!(weatherRequestDTO.Lon == null || weatherRequestDTO.Lat == null))
            {
                _logger.LogInformation("Location Request");

                decimal lat = Math.Round((decimal)weatherRequestDTO.Lat, 2);
                decimal lon = Math.Round((decimal)weatherRequestDTO.Lon, 2);
                string url = $"{baseUrl}/data/2.5/forecast?lat={lat}" +
                    $"&lon={lon}&cnt={cnt}&appid={apiKey}&units=metric&mode=json";

                var data = await requestWeatherDataFromApi(url: url);

                if (data != null)
                {
                    return data;
                }
                else
                {
                    _logger.LogInformation("Location Request Failed: Data Returned Null.");
                    throw new Exception(message: "Data queried but returned null.");
                }
            }
            else if (!(string.IsNullOrEmpty(weatherRequestDTO.City) || string.IsNullOrEmpty(weatherRequestDTO.CountryCode)))
            {
                _logger.LogInformation("City Request");

                string countryCode = weatherRequestDTO.CountryCode;
                string city = weatherRequestDTO.City.ToUpper();
                string url = $"{baseUrl}/geo/1.0/direct?q={city},{countryCode}&limit={1}&appid={apiKey}";

                var data = await requestWeatherDataFromApiUsingNames(url: url, cnt: cnt);
                if (data != null)
                {
                    return data;
                }
                else
                {
                    _logger.LogInformation("City Request Failed: Data Returned Null.");
                    throw new Exception(message: "Data queried but returned null.");
                }
            }
            else
            {
                throw new Exception(message: "Data pairs did not meet criteria. Provide lon with lat or city and countryCode together.");
            }
        }

        public IEnumerable<string> GetListCities()
        {
            return new string[] { "city_test", "city_test_2" };
        }

        private async Task<IEnumerable<WeatherDTO>> requestWeatherDataFromApi(string url)
        {
            using (var httpClient = _httpClientFactory.CreateClient())
            {
                HttpResponseMessage res = await httpClient.GetAsync(url);
                if (res.IsSuccessStatusCode)
                {
                    string responseString = await res.Content.ReadAsStringAsync();
                    var result = JsonNode.Parse(responseString);

                    if (result == null)
                    {
                        _logger.LogInformation("API returned empty JSON.");
                        throw new Exception("External API returned empty JSON.");
                    }

                    var data = result["list"];

                    if (data == null || data.AsArray().Count == 0)
                    {
                        _logger.LogInformation("API returned empty list of data.");
                        throw new Exception("External API returned empty list of data.");
                    }

                    List<WeatherDTO> list = new List<WeatherDTO>();

                    var dataArray = (JsonArray)data;
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
                    throw new Exception("Unable to communicate with the External API.");
                }
            }
        }

        private async Task<IEnumerable<WeatherDTO>> requestWeatherDataFromApiUsingNames(string url, int cnt)
        {
            using (var httpClient = _httpClientFactory.CreateClient())
            {
                HttpResponseMessage res = await httpClient.GetAsync(url);
                if (res.IsSuccessStatusCode)
                {
                    string responseString = await res.Content.ReadAsStringAsync();
                    var result = JsonNode.Parse(responseString);

                    _logger.LogInformation(result.ToJsonString());
                    _logger.LogInformation(result.ToJsonString());
                    _logger.LogInformation(result.AsArray().Count.ToString());

                    if (result == null || result.AsArray().Count == 0)
                    {
                        _logger.LogInformation("API returned empty JSON.");
                        throw new Exception("External API returned empty JSON.");
                    }

                    if (result[0]["lat"] == null || result[0]["lon"] == null)
                    {
                        _logger.LogInformation("API returned empty list of data.");
                        throw new Exception("External API returned empty list of data.");
                    }
                    else
                    {
                        string baseUrl = _config["WeatherApi:Url"];
                        string apiKey = _config["WeatherApi:ServiceApiKey"];
                        decimal lat = (decimal)result[0]["lat"];
                        decimal lon = (decimal)result[0]["lon"];

                        url = $"{baseUrl}/data/2.5/forecast?lat={lat}" +
                    $"&lon={lon}&cnt={cnt}&appid={apiKey}&units=metric&mode=json";

                        return await requestWeatherDataFromApi(url: url);
                    }
                }
                else
                {
                    throw new Exception("Unable to communicate with the External API.");
                }
            }
        }
    }

    public interface IWeatherService
    {
        Task<IEnumerable<WeatherDTO>> GetWeather(WeatherRequestDTO weatherRequestDTO);
        IEnumerable<string> GetListCities();
    }
}
