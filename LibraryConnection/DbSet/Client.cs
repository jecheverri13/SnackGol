using System.ComponentModel.DataAnnotations;

namespace LibraryConnection.DbSet
{
    public class Client
    {
        [Key]
        public virtual string? document { get; set; }
        public string? name { get; set; }
        public string? emailAddress { get; set; }
        public string? docType { get; set; }

        public string? status { get; set; }
        public ICollection<Order>? orders { get; set; }
    }

}
