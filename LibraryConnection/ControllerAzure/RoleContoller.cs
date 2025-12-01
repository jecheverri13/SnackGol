using LibraryConnection.Context;
using LibraryConnection.DbSet;
using LibraryEncrypt.Encryption;
using LibraryEntities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace LibraryConnection.ControllerAzure
{
    internal class RoleContoller
    {
        /// <summary>
        /// Registro de roles, no se podra crear un rol con el mismo nombre
        /// </summary>
        /// <param name="oRole">Role a Crear</param>
        /// <returns>Objeto dynamic con el estado de la petición de creación</returns>
        public static Response<dynamic> PostDbRole(RoleRequest oRole)
        {
            try
            {
                using (var contexto = new ApplicationDbContext())
                {
                    //Verificar si ya existe un usuario con el mismo email
                    var existingClient = contexto.roles
                        .FirstOrDefault(c => c.name == oRole.name);
                    if (existingClient != null)
                    {
                        // Si ya existe, retornar un error indicando que el cliente ya está registrado
                        return new Response<dynamic>(false, HttpStatusCode.Conflict, "Ya existe un rol con el mismo nombre.");
                    }
                    Role role = new Role()
                    {
                        name=oRole.name,
                        description=oRole.description
                    };

                    // Si no existe, agregar el nuevo cliente
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

                return new Response<dynamic>(false, HttpStatusCode.InternalServerError, "Error PostDbClient", errorMessage);
            }
        }
        /// <summary>
        /// Obtener una consulta por medio de su email
        /// </summary>
        /// <param email="email">email del usuario</param>
        /// <returns>Query</returns>
        public static Response<RoleResponse> GetDbRoles()
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
                    .ToList()
                    .FirstOrDefault();


                    if (oRole != null)
                    {
                        return new Response<RoleResponse>(true, HttpStatusCode.OK, oRole);
                    }
                    else
                    {
                        return new Response<RoleResponse>(false, HttpStatusCode.NotFound, "No se encontró un role");
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
                return new Response<RoleResponse>(false, HttpStatusCode.InternalServerError, "Error obtenendo el role", errorMessage);
            }
        }
    }   
}
