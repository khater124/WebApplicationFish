namespace WebApplicationFish.Models
{
    public class Fish
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Method { get; set; } = "";
        public string Type { get; set; } = "";
        public string Place { get; set; } = "";
        public string Weather { get; set; } = "";
        public decimal Weight { get; set; }
        public string ImageFileName { get; set; } = "";
        public DateTime CaughtAt { get; set; }
        
    }
}
