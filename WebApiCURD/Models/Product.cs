using System.ComponentModel.DataAnnotations;

namespace WebApiCURD.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        [Required]
        public string Brand { get; set; } = "";
        [Required]
        public string Category { get; set; } = "";
        [Required]
        public string Price { get; set; } = "";
        [Required]
        public string Description { get; set; } = "";
        public DateTime Created { get; set; }
    }
}
