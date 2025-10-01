using LibraryConnection.Context;
using LibraryConnection.DbSet;
using LibraryConnection.Dtos;
using LibraryEncrypt.Encryption;
using LibraryEntities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;


namespace LibraryConnection.ControllerAzure
{
    internal class UserController
    {
        /// <summary>
        /// Registro de Usuarios y Asesores, para determina si es cliente o asesor se tiene el cuenta el estado de los campos (isAdvisor, isClient)
        /// </summary>
        /// <param name="oUser">Usuario a Crear</param>
        /// <returns>Objeto dynamic con el estado de la petición de creación</returns>
        public static Response<dynamic> PostDbUser(UserRequest oUser)
        {
            try
            {
                using (var contexto = new ApplicationDbContext())
                {
                    //Verificar si ya existe un usuario con el mismo email
                    var existingClient = contexto.users
                        .FirstOrDefault(c => c.email == oUser.email);
                    var role = contexto.roles.FirstOrDefault(r => r.id == oUser.id_role);
                    if(role == null)
                    {
                        return new Response<dynamic>(false, HttpStatusCode.BadRequest, "El rol especificado no existe.");
                    }
                    if (existingClient != null)
                    {
                        // Si ya existe, retornar un error indicando que el cliente ya está registrado
                        return new Response<dynamic>(false, HttpStatusCode.Conflict, "Ya existe un usuario con el mismo email.");
                    }
                    var encryptedPassword = EncryptCSS.Encrypt(oUser.password);
                    User user = new User()
                    {
                        name = oUser.name,
                        last_name = oUser.last_name,
                        email = oUser.email,
                        id_role=oUser.id_role,
                        password = encryptedPassword
                    };
                    // Si no existe, agregar el nuevo cliente
                    contexto.users.Add(user);
                    int index = contexto.SaveChanges();
                    if (index >= 1)
                    {
                        return new Response<dynamic>(true, HttpStatusCode.Created, user);
                    }
                }
                return new Response<dynamic>(false, HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                string errorMessage = "Error: " + ex.Message;
                if (ex.InnerException != null)
                {
                    errorMessage += " Inner exception: " + ex.InnerException.Message;
                }

                return new Response<dynamic>(false, HttpStatusCode.InternalServerError, "Error PostDbClient", errorMessage);
            }
        }
        /// <summary>
        /// Obtener una consulta por medio de su email
        /// </summary>
        /// <param email="email">email del usuario</param>
        /// <returns>Query</returns>
        public static Response<UserResponse> GetDbUserByEmail(string userEmail)
        {
            try
            {
                using (var contexto = new ApplicationDbContext())
                {

                    var oUser = contexto.users
                    .AsEnumerable()
                    .Where(e => e.email == userEmail)
                     .Select(e => new UserResponse
                     {
                         id = e.id,
                         name = e.name,
                         last_name= e.last_name,
                         email = e.email,
                         id_role = e.id_role
                     })
                    .ToList()
                    .FirstOrDefault();


                    if (oUser != null)
                    {
                        return new Response<UserResponse>(true, HttpStatusCode.OK, oUser);
                    }
                    else
                    {
                        return new Response<UserResponse>(false, HttpStatusCode.NotFound, "No se encontró un usuario con su email");
                    }
                }
            }
            catch (Exception ex)
            {
                string errorMessage = "Error: " + ex.Message;
                if (ex.InnerException != null)
                {
                    errorMessage += " Inner exception: " + ex.InnerException.Message;
                }

                return new Response<UserResponse>(false, HttpStatusCode.InternalServerError, "Error obtenendo el usuario", errorMessage);
            }
        }
    }

}
