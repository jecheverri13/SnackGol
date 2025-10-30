using LibraryConnection.Context;
using LibraryConnection.ControllerAzure;
using LibraryEntities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace MSSnackGol.Controllers
{
    [Route("api/[controller]")]
    public class UserManagementController : ControllerBase
    {
        [HttpPost("Create")]
        public IActionResult UserCreation([FromBody] UserRequest userData)
        {
            try
            {
                var oResponse = UserController.PostDbUser(userData);
                return StatusCode((int)oResponse.status, oResponse);
            }
            catch (Exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "Error al enviar el correo de confirmación de creación de usuario");
            }
        }
        [HttpGet("All")]
        public IActionResult UserGetAll()
        {
            try
            {
                var oResponse = UserController.GetDbUsers();
                return StatusCode((int)oResponse.status, oResponse);
            }
            catch (Exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "Error al enviar el correo de confirmación de creación de usuario");
            }
        }
    }
}
