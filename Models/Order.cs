using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GardenCenter.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Customer")]
        public int CustomerId { get; set; }

        public string? Date { get; set; }
        public Item? Items { get; set; }
        public decimal OrderTotal { get; set; }

    }
}