using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryEntities.Models
{
    public class AuthAdminRequest
    {
        public bool lowLevel { get; set; } = false;
        public string  userName {  get; set; }
        public string  secret {  get; set; }
    }   
}
