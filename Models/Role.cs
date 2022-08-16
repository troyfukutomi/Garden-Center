using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GardenCenter.Models
{
    public class Role
    {
        [Key]
        public int Id { get; set; }
        public bool Admin { get; set; }
        public bool Employee { get; set; }
    }
}