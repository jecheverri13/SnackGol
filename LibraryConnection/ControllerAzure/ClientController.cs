using LibraryConnection.Context;
using LibraryEntities.Models;
using System.Net;
using Client = LibraryConnection.DbSet.Client;
using Microsoft.Extensions.Configuration;
using Azure.Storage.Blobs.Models;

namespace LibraryConnection.ControllerAzure
{
    public class ClientController
    {

        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public ClientController(IConfiguration configuration, HttpClient httpClient)
        {
            _configuration = configuration;
            _httpClient = httpClient;
        }

        /// <summary>
        /// Obtener todos los clientes, sin tener encuenta la sede y la URI(Contrato de la base a la cual hago referencia)
        /// </summary>
        /// <returns>Lista de usuarios</returns>
        public static Response<List<ClientResponse>> GetDbClients()
        {
            try
            {
                using (var contexto = new ApplicationDbContext())
                {
                    var clientResponses = contexto.clients
                        .AsEnumerable()
                        .Select(e => new ClientResponse
                        {
                            document = e.document,
                            name = e.name,
                            emailAddress = e.emailAddress,
                            docType = e.docType,
                            status = e.status
                        })
                        .ToList();

                    return new Response<List<ClientResponse>>(true, HttpStatusCode.OK, clientResponses);
                }
            }
            catch (Exception ex)
            {
                string errorMessage = "Error: " + ex.Message;
                if (ex.InnerException != null)
                {
                    errorMessage += " Inner exception: " + ex.InnerException.Message;
                }

                return new Response<List<ClientResponse>>(false, HttpStatusCode.InternalServerError, "Error GetDbClients", errorMessage);
            }
        }

        public static Response<List<ClientResponse>> GetDbClientsPending()
        {
            try
            {
                using (var contexto = new ApplicationDbContext())
                {
                    var clientResponses = contexto.clients
                        .AsEnumerable()
                        .Where(e => e.status == "pending")
                        .Select(e => new ClientResponse
                        {
                            document = e.document,
                            name = e.name,
                            emailAddress = e.emailAddress,
                            docType = e.docType,
                            status = e.status
                        })
                        .ToList();
                    return new Response<List<ClientResponse>>(true, HttpStatusCode.OK, clientResponses);
                }
            }
            catch (Exception ex)
            {
                string errorMessage = "Error: " + ex.Message;
                if (ex.InnerException != null)
                {
                    errorMessage += " Inner exception: " + ex.InnerException.Message;
                }
                return new Response<List<ClientResponse>>(false, HttpStatusCode.InternalServerError, "Error GetDbClientsPending", errorMessage);
            }
        }

        /// <summary>
        /// Obtener un usuario en especifico teniendo encuenta su indentificador
        /// </summary>
        /// <param name="idClient">Id del Usuario</param>
        /// <returns>Client</returns>
        public static Response<ClientResponse> GetDbClientById(string idClient)
        {
            try
            {
                using (var contexto = new ApplicationDbContext())
                {

                    var oClient = contexto.clients
                    .AsEnumerable()
                    .Where(e => e.document == idClient)
                    .Select(e => new ClientResponse
                    {
                        name = e.name,
                        document = e.document,
                        emailAddress = e.emailAddress,
                        docType = e.docType,
                        status = e.status
                    })
                    .ToList()
                    .FirstOrDefault();


                    if (oClient != null)
                    {
                        return new Response<ClientResponse>(true, HttpStatusCode.OK, oClient);
                    }
                    else
                    {
                        return new Response<ClientResponse>(false, HttpStatusCode.NotFound, "No se encontró un usuario con el ID proporcionado");
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

                return new Response<ClientResponse>(false, HttpStatusCode.InternalServerError, "Error GetRoleById", errorMessage);
            }
        }

        /// <summary>
        /// Checa si un usuario existe con el numero de documento proporcionado.
        /// </summary>
        /// <param name="documentNumber">Id del Usuario</param>
        /// <returns>Client</returns>
        public static Response<Object> ClientExistWithThisDocumentNumber(string documentNumber)
        {
            try
            {
                using (var contexto = new ApplicationDbContext())
                {
                    var oClient = contexto.clients
                    .Where(e => e.document == documentNumber)
                    .ToList()
                    .FirstOrDefault();

                    var responseBody = new { userExist = (oClient != null) };
                    return new Response<Object>(true, HttpStatusCode.OK, responseBody);
                }
            }
            catch (Exception ex)
            {
                string errorMessage = "Error: " + ex.Message;
                if (ex.InnerException != null)
                {
                    errorMessage += " Inner exception: " + ex.InnerException.Message;
                }

                return new Response<Object>(false, HttpStatusCode.InternalServerError, "Error get user by document number", errorMessage);
            }
        }

        /// <summary>
        /// Registro de Usuarios y Asesores, para determina si es cliente o asesor se tiene el cuenta el estado de los campos (isAdvisor, isClient)
        /// </summary>
        /// <param name="oClient">Usuario a Crear</param>
        /// <returns>Objeto dynamic con el estado de la petición de creación</returns>
        public static Response<dynamic> PostDbClient(ClientRequest oClient)
        {
            try
            {
                    using (var contexto = new ApplicationDbContext())
                    {
                    //Verificar si ya existe un cliente con el mismo documento
                    var existingClient = contexto.clients
                        .FirstOrDefault(c => c.document == oClient.document);
                    if (existingClient != null)
                    {
                        // Si ya existe, retornar un error indicando que el cliente ya está registrado
                        return new Response<dynamic>(false, HttpStatusCode.Conflict, "Ya existe un cliente con el mismo número de documento.");
                    }
                    Client client = new Client()
                    {
                        document = oClient.document,
                        name = oClient.name,
                        emailAddress = oClient.emailAddress,
                        docType = oClient.docType,
                        status = "pending"
                    };


                    // Si no existe, agregar el nuevo cliente
                    contexto.clients.Add(client);

                        int index = contexto.SaveChanges();
                        if (index >= 1)
                        {
                            return new Response<dynamic>(true, HttpStatusCode.Created, oClient);
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

        public static Response<dynamic> PutDbClient(Client updatedClient)
        {
            try
            {
                using (var contexto = new ApplicationDbContext())
                {
                    var existingClient = contexto.clients.FirstOrDefault(u => u.document == updatedClient.document);

                    if (existingClient != null)
                    {
                        // Actualizar los campos del cliente existente con los valores del cliente actualizado
                        existingClient.name = updatedClient.name;
                        existingClient.docType = updatedClient.docType;
                        existingClient.emailAddress = updatedClient.emailAddress;
                        contexto.SaveChanges();

                        return new Response<dynamic>(true, HttpStatusCode.NoContent);
                    }
                    else
                    {
                        return new Response<dynamic>(false, HttpStatusCode.NotFound, "No se encontró el usuario con el ID proporcionado");
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

                return new Response<dynamic>(false, HttpStatusCode.InternalServerError, "Error UpdateDbClient", errorMessage);
            }
        }

        public static Response<dynamic> UpdateClientStatus(string document, string status)
        {
            try
            {
                using (var contexto = new ApplicationDbContext())
                {
                    var existingClient = contexto.clients.FirstOrDefault(u => u.document == document);
                    if (existingClient != null)
                    {
                        // Actualizar el estado del cliente existente
                        existingClient.status = status;
                        contexto.SaveChanges();
                        return new Response<dynamic>(true, HttpStatusCode.NoContent);
                    }
                    else
                    {
                        return new Response<dynamic>(false, HttpStatusCode.NotFound, "No se encontró el usuario con el ID proporcionado");
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
                return new Response<dynamic>(false, HttpStatusCode.InternalServerError, "Error UpdateClientStatus", errorMessage);
            }
        }
    }
}
