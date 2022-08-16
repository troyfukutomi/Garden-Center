using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GardenCenter.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public string? Sku { get; set; }
        public string? Type { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Manufacturer { get; set; }
        public decimal Price { get; set; }
    }
}