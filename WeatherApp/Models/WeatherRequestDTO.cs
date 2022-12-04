namespace WeatherApp.Models
{
    public class WeatherRequestDTO
    {
        public String? city { get; set; }
        public String? lat { get; set; }
        public String? lon { get; set; }
        public int? cnt { get; set; }
    }
}