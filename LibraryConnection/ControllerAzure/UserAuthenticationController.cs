using LibraryConnection.Context;
using LibraryConnection.DbSet;
using LibraryEncrypt.Encryption;
using LibraryEntities;
using LibraryEntities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace LibraryConnection.ControllerAzure
{
    public class UserAuthenticationController
    {
        public bool VerifyUserAndPassword(LoginRequest login)
        {
            try
            {
                using (var contexto = new ApplicationDbContext())
                {
                    var user = contexto.users
                        .FirstOrDefault(u => u.email == login.UserNname);
                    if (user != null) 
                    {
                        return EncryptCSS.Encrypt(login.Password) == user.password;
                    };
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
