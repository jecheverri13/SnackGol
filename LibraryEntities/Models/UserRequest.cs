using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryEntities.Models
{
    public class UserRequest
    {

       
        [Required]
        public string name { get; set; }
        [Required]
        public string password { get; set; }
        [Required]
        public string last_name { get; set; }
        [Required]
        public string email { get; set; }
        public long id_role { get; set; }
    }
}
