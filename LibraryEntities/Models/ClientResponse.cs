using System.ComponentModel.DataAnnotations;

namespace LibraryEntities.Models
{
    public class ClientResponse
    {
        [Key]
        public virtual string? document { get; set; }
        public string? name { get; set; }
        public string? emailAddress { get; set; }
        public string? docType { get; set; }
    }
}
