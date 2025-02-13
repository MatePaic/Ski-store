using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace API.DTOs
{
    public class AddressDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Line1 { get; set; } = string.Empty;
        public string? Line2 { get; set; }
        [Required]
        public string City { get; set; } = string.Empty;
        [Required]
        public string State { get; set; } = string.Empty;

        [Required]
        [JsonPropertyName("postal_code")]
        public string PostalCode { get; set; } = string.Empty;
        [Required]
        public string Country { get; set; } = string.Empty;
    }
}
