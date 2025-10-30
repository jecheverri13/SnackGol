using LibraryConnection.Context;
using LibraryConnection.DbSet;
using LibraryEncrypt.Encryption;
using LibraryEntities.Models;
using System.Net;


namespace LibraryConnection.ControllerAzure
{
    public class UserController
    {
        /// <summary>
        /// Registro de Usuarios
        /// </summary>
        /// <param name="oUser">Usuario a Crear</param>
        /// <returns>Objeto dynamic con el estado de la petición de creación</returns>
        public static Response<dynamic> PostDbUser(UserRequest oUser)
        {
            try
            {
                using (var contexto = new ApplicationDbContext())
                {
                    var existingClient = contexto.users
                        .FirstOrDefault(c => c.email == oUser.email);
                    var role = contexto.roles.FirstOrDefault(r => r.id == oUser.id_role);
                    if(role == null)
                    {
                        return new Response<dynamic>(false, HttpStatusCode.BadRequest, "El rol especificado no existe.");
                    }
                    if (existingClient != null)
                    {
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
                    contexto.users.Add(user);
                    int index = contexto.SaveChanges();
                    if (index >= 1)
                    {
                        return new Response<dynamic>(true, HttpStatusCode.Created, oUser);
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
        /// Obtener usuario por medio de su email
        /// </summary>
        /// <param email="email">email del usuario</param>
        /// <returns>UserResponse</returns>
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

                return new Response<UserResponse>(false, HttpStatusCode.InternalServerError, "Error obteniendo el usuario", errorMessage);
            }
        }

        /// <summary>
        /// Obtener todos los usuarios
        /// </summary>
        /// <returns>Lista de usuarios</returns>
        public static Response<List<UserResponse>> GetDbUsers()
        {
            try
            {
                using (var contexto = new ApplicationDbContext())
                {
                    var userResponses = contexto.users
                        .AsEnumerable()
                        .Select(e => new UserResponse
                        {
                            id = e.id,
                            name = e.name,
                            email = e.email,
                            last_name = e.last_name,
                            id_role = e.id_role
                        })
                        .ToList();

                    return new Response<List<UserResponse>>(true, HttpStatusCode.OK, userResponses);
                }
            }
            catch (Exception ex)
            {
                string errorMessage = "Error: " + ex.Message;
                if (ex.InnerException != null)
                {
                    errorMessage += " Inner exception: " + ex.InnerException.Message;
                }

                return new Response<List<UserResponse>>(false, HttpStatusCode.InternalServerError, "Error GetDbUsers", errorMessage);
            }
        }
    }

}
