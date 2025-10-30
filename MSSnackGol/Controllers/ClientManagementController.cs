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
    public class ClientManagementController : ControllerBase
    {
        private readonly ILogger<ClientManagementController> _logger;
        public ClientManagementController(ILogger<ClientManagementController> logger)
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
                return StatusCode((int)HttpStatusCode.InternalServerError, "Error al crear el cliente.");
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
                return StatusCode((int)HttpStatusCode.InternalServerError, "Error al obtener los clientes.");
            }
        }
    }
}
