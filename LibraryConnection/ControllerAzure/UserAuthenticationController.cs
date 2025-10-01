using LibraryConnection.Context;
using LibraryConnection.DbSet;
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
        //public static Response<dynamic> PostDbUserAutentication(UserAuthentication oUserAuthentication)
        //{
        //    try
        //    {
        //        using (var contexto = new ApplicationDbContext())
        //        {
        //            UserAuthentication? oDeleteUserAuthentication = contexto.userAuthentication.Where(u => u.idUser == oUserAuthentication.idUser || u.email == oUserAuthentication.email).FirstOrDefault();

        //            if (oDeleteUserAuthentication != null)
        //            {
        //                contexto.userAuthentication.Remove(oDeleteUserAuthentication);
        //                contexto.SaveChanges();
        //            }
        //            contexto.userAuthentication.Add(oUserAuthentication);
        //            contexto.SaveChanges();

        //            return new Response<dynamic>(true, HttpStatusCode.Created, new { oUserAuthentication.id, oUserAuthentication.idUser });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        string errorMessage = "Error: " + ex.Message;
        //        if (ex.InnerException != null)
        //        {
        //            errorMessage += " Inner exception: " + ex.InnerException.Message;
        //        }

        //        return new Response<dynamic>(false, HttpStatusCode.InternalServerError, "Error PostDbUserAutentication", errorMessage);
        //    }
        //}

        //public static Response<dynamic> GetDbUserAutenticationyById(int id, string code)
        //{
        //    try
        //    {
        //        using (var contexto = new ApplicationDbContext())
        //        {
        //            UserAuthentication? oUserAuthentication = contexto.userAuthentication.FirstOrDefault(u => u.id == id && (u.otpCode == code || u.emailCode == code));
        //            if(oUserAuthentication !=  null)
        //            {
        //                contexto.userAuthentication.Remove(oUserAuthentication);
        //                contexto.SaveChanges();
        //                return new Response<dynamic>(true, HttpStatusCode.OK, new { oUserAuthentication.idUser});
        //            }                    
        //            else
        //            {
        //                return new Response<dynamic>(
        //                );
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        string errorMessage = "Error: " + ex.Message;
        //        if (ex.InnerException != null)
        //        {
        //            errorMessage += " Inner exception: " + ex.InnerException.Message;
        //        }
        //        return new Response<dynamic>(false, HttpStatusCode.InternalServerError, "Error GetDbUserAutenticationyById", errorMessage);
        //    }
        //}

        //public static Response<dynamic> DeleteDbUserAutenticationyById(int id)
        //{
        //    try
        //    {
        //        using (var contexto = new ApplicationDbContext())
        //        {
        //            UserAuthentication? oUserAuthentication = contexto.userAuthentication.FirstOrDefault(u => u.id == id);
        //            if (oUserAuthentication != null)
        //            {
        //                contexto.userAuthentication.Remove(oUserAuthentication);
        //                contexto.SaveChanges();

        //                return new Response<dynamic>(true, HttpStatusCode.OK);
        //            }
        //            else
        //            {
        //                return new Response<dynamic>(true, HttpStatusCode.OK);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        string errorMessage = "Error: " + ex.Message;
        //        if (ex.InnerException != null)
        //        {
        //            errorMessage += " Inner exception: " + ex.InnerException.Message;
        //        }

        //        return new Response<dynamic>(false, HttpStatusCode.InternalServerError, "Error DeleteDbUserAutenticationyById", errorMessage);
        //    }
        //}
    }
}
