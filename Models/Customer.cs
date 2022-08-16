using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GardenCenter.Models
{
    public class Customer
    {
        [Key]
        public long Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public Address? Address { get; set; }
    }
}