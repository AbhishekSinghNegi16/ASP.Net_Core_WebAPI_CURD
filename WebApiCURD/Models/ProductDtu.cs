using System.ComponentModel.DataAnnotations;

namespace WebApiCURD.Models
{
    public class ProductDtu
    {
        //data we are getting from client
        [Required]
        public string Name { get; set; } = "";
        [Required]
        public string Brand { get; set; } = "";
        [Required]
        public string Category { get; set; } = "";
        [Required]
        public string Price { get; set; } = "";
        [Required]
        public string Description { get; set; } = "";
    }
}
