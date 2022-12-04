using System.ComponentModel.DataAnnotations;

namespace WeatherApp.Models
{
    public class WeatherRequestDTO
    {
        [RegularExpression("^[a-zA-Z\\s]+$", ErrorMessage = "City name can only contain alphabets and space characters.")]
        public String? City { get; set; }

        [Range(-90, 90, ErrorMessage = "Valid latitude value between (-90, 90)")]
        public decimal? Lat { get; set; }

        [Range(-180, 180, ErrorMessage = "Valid longitude  value between (-180, 180)")]
        public decimal? Lon { get; set; }

        [Range(1, 5, ErrorMessage = "Valid cnt value between (1, 5)")]
        public int? Cnt { get; set; }
    }
}