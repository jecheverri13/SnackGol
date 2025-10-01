using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryEntities.Models
{
    public class AuthAdminResponse
    {
        public Session session { get; set; }
        public User user { get; set; }
        public class Session
        {
            public string sessionId { get; set; }
            public long createdAt { get; set; }
            public long expiresAt { get; set; }
            public string csrfToken { get; set; }
        }

        public class User
        {
            public string name { get; set; }
            public string locale { get; set; }
            public string userName { get; set; }
            public string uuid { get; set; }
            public bool externalAuthentication { get; set; }
            public string logonLanguageCode { get; set; }
        }
    }
}
