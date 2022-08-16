using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GardenCenter.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Title { get; set; }

        public virtual Role? Roles { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
    }
}