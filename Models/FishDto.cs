using System.ComponentModel.DataAnnotations;

namespace WebApplicationFish.Models
{
    public class FishDto
    {
        [Required]
        public string Name { get; set; } = "";
        [Required]
        public string Method { get; set; } = "";
        [Required]
        public string Type { get; set; } = "";
        [Required]
        public string Place { get; set; } = "";
        [Required]
        public string Weather { get; set; } = "";
        [Required]
        public decimal Weight { get; set; }
     
        public IFormFile? ImageFile { get; set; }
    }
}
