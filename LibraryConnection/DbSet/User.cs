using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryConnection.DbSet
{
        public class User
        {
            [Key]
            [Required]
            public long id { get; set; }
            [Required]
            public string name { get; set; }
            [Required]
            public string last_name { get; set; }
            [Required]
            public string email { get; set; }
            [Required]
            public string password { get; set; }

            [ForeignKey("User")]
            [Required]
            public long id_role { get; set; }
            public Role Role { get; set; }
    }
}
