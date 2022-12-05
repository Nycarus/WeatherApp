namespace WeatherApp.Models
{
    public class WeatherDTO
    {
        public decimal Temp { get; set; } = default!;
        public decimal TempMin { get; set; } = default!;
        public decimal TempMax { get; set; } = default!;
        public int Humidity { get; set; } = default!;
        public string? Weather { get; set; }
        public string? Description { get; set; }
        public int Cloud { get; set; } = default!;
        public string Time { get; set; } = default!;
    }
}