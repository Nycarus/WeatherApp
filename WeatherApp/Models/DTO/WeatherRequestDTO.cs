using System.ComponentModel.DataAnnotations;

namespace WeatherApp.Models.DTO
{
    public class WeatherRequestDTO
    {
        [RegularExpression("^[a-zA-Z\\s]+$", ErrorMessage = "City name can only contain alphabet and space characters.")]
        public string? City { get; set; }

        [RegularExpression("^[a-zA-Z\\s]{2}$", ErrorMessage = "Country name can only contain alphabet characters with 2 spaces in length.")]
        public string? CountryCode { get; set; }

        [Range(-90, 90, ErrorMessage = "Valid latitude value between (-90, 90)")]
        public decimal? Lat { get; set; }

        [Range(-180, 180, ErrorMessage = "Valid longitude value between (-180, 180)")]
        public decimal? Lon { get; set; }

        [Range(1, 5, ErrorMessage = "Valid cnt value between (1, 5)")]
        public int? Cnt { get; set; }
    }
}