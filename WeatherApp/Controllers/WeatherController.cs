using Microsoft.AspNetCore.Mvc;
using WeatherApp.Models;
using WeatherApp.Models.DTO;
using WeatherApp.Models.Services;

namespace WeatherApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeatherController : ControllerBase
    {
        private readonly ILogger<WeatherController> _logger;
        private readonly IWeatherService _weatherservice;

        public WeatherController(
            ILogger<WeatherController> logger,
            IWeatherService weatherservice)
        {
            _logger = logger;
            _weatherservice = weatherservice;
        }

        [HttpPost]
        public async Task<ApiResponse<IEnumerable<WeatherDTO>>> Get([FromBody] WeatherRequestDTO weatherRequestDTO)
        {
            _logger.LogInformation("Weather Forecast Request at: {time}", DateTime.Now);
            try 
            {
                IEnumerable<WeatherDTO> result = await _weatherservice.GetWeather(weatherRequestDTO);
                return ApiResponse<IEnumerable<WeatherDTO>>.SuccessResponse(data: result, message: "Query Success");
            }
            catch (Exception e)
            {
                return ApiResponse<IEnumerable<WeatherDTO>>.ErrorResponse(error: e.Message);
            }
        }

        [HttpGet("cities")]
        public IEnumerable<String> Get()
        {
            _logger.LogInformation("Availlable Cities Request at: {time}", DateTime.Now);
            return _weatherservice.GetListCities();
        }
    }
}