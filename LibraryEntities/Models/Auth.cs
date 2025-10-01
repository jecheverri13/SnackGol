using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryEntities.Models
{
    public class Auth
    {
        public String token { get; set; }
        public int expiresIn { get; set; }
    }
}
