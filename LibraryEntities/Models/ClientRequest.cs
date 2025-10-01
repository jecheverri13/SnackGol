using System.ComponentModel.DataAnnotations;

namespace LibraryEntities.Models
{
    public class ClientRequest
    {
        [Key]
        public virtual string? document { get; set; } = "2222222";
        public string? name { get; set; } = "DefaultCustomer";

        public string? emailAddress { get; set; } = "default.customer@consensussa.com";
        public string? docType { get; set; } = "CC";

    }
}
