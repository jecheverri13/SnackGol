using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static LibraryEntities.Models.AuthAdminResponse;

namespace LibraryEntities.Models
{
    public class RoleRequest
    {
        [Required]
        public string name { get; set; }
        [Required]
        public string description { get; set; }
        
    }
}
