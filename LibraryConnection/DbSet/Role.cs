using System.ComponentModel.DataAnnotations;

namespace LibraryConnection.DbSet
{
    public class Role
    {
        [Key]
        [Required]
        public  long id { get; set; }
        [Required]
        public string? name { get; set; }
        [Required]
        public string? description { get; set; }
        public ICollection<User>? users { get; set; }
    }
}
