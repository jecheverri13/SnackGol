using LibraryConnection.ControllerAzure;
using LibraryConnection.DbSet;
using LibraryEntities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace MSSnackGol.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientManagmentController : ControllerBase
    {
        private readonly ILogger<ClientManagmentController> _logger;
        public ClientManagmentController(ILogger<ClientManagmentController> logger)
        {
            _logger = logger;
        }



        [HttpPost("Create")]
        public IActionResult ClientCreation([FromBody] ClientRequest clientData)
        {
            try
            {
                var oResponse = ClientController.PostDbClient(clientData);
                return StatusCode((int)oResponse.status, oResponse);
            }
            catch (Exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "Error al enviar el correo de confirmación de creación de usuario");
            }
        }
        [HttpGet("All")]
        [Authorize]
        public IActionResult ClientGetAll()
        {
            try
            {
                var oResponse = ClientController.GetDbClients();
                return StatusCode((int)oResponse.status, oResponse);
            }
            catch (Exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "Error al enviar el correo de confirmación de creación de usuario");
            }
        }
        [HttpGet("Pending")]
        public IActionResult ClientGetPending()
        {
            try
            {
                var oResponse = ClientController.GetDbClientsPending();
                return StatusCode((int)oResponse.status, oResponse);
            }
            catch (Exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "Error al enviar el correo de confirmación de creación de usuario");
            }
        }
        [HttpPatch("UpdateStatus")]
        public IActionResult UpdateClienStatus(string document, string status)
        {
            try
            {
                var oResponse = ClientController.UpdateClientStatus(document, status);
                return StatusCode((int)oResponse.status, oResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el estado del cliente");
                return StatusCode(
                    (int)HttpStatusCode.InternalServerError,
                    new Response<dynamic>(false, HttpStatusCode.InternalServerError, "Error al actualizar el estado del cliente", ex.Message)
                );
            }
        }
    }
}
