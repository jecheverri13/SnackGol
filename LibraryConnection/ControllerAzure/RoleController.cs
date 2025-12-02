using LibraryConnection.Context;
using LibraryConnection.DbSet;
using LibraryEntities.Models;
using System.Net;

namespace LibraryConnection.ControllerAzure
{
    public class RoleController
    {
        /// <summary>
        /// Registro de roles, no se podrá crear un rol con el mismo nombre
        /// </summary>
        /// <param name="oRole">Rol a Crear</param>
        /// <returns>Objeto dynamic con el estado de la petición de creación</returns>
        public static Response<dynamic> PostDbRole(RoleRequest oRole)
        {
            try
            {
                using (var contexto = new ApplicationDbContext())
                {
                    var existingRole = contexto.roles
                        .FirstOrDefault(c => c.name == oRole.name);
                    if (existingRole != null)
                    {
                        return new Response<dynamic>(false, HttpStatusCode.Conflict, "Ya existe un rol con el mismo nombre.");
                    }
                    Role role = new()
                    {
                        name=oRole.name,
                        description=oRole.description
                    };
                    contexto.roles.Add(role);

                    int index = contexto.SaveChanges();
                    if (index >= 1)
                    {
                        return new Response<dynamic>(true, HttpStatusCode.Created, role);
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

                return new Response<dynamic>(false, HttpStatusCode.InternalServerError, "Error PostDbRole.", errorMessage);
            }
        }
        /// <summary>
        /// Obtener todos los roles
        /// </summary>
        /// <returns>Response<RoleResponse></returns>
        public static Response<List<RoleResponse>> GetDbRoles()
        {
            try
            {
                using (var contexto = new ApplicationDbContext())
                {

                    var oRole = contexto.roles
                    .AsEnumerable()
                     .Select(e => new RoleResponse
                     {
                         id = e.id,
                         name = e.name,
                         description = e.description
                     })
                    .ToList();
                    


                    if (oRole != null)
                    {
                        return new Response<List<RoleResponse>>(true, HttpStatusCode.OK, oRole);
                    }
                    else
                    {
                        return new Response<List<RoleResponse>>(false, HttpStatusCode.NotFound, "No se encontró un rol.");
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
                return new Response<List<RoleResponse>>(false, HttpStatusCode.InternalServerError, "Error obteniendo el rol.", errorMessage);
            }
        }
    }   
}
