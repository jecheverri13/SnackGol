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
    [ApiController]
    public class RoleManagementController : ControllerBase
    {
        [HttpPost("Create")]
        public IActionResult RoleCreation([FromBody] RoleRequest roleData)
        {
            try
            {
                var oResponse = RoleController.PostDbRole(roleData);
                return StatusCode((int)oResponse.status, oResponse);
            }
            catch (Exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "Error al crear el rol.");
            }
        }
        [HttpGet("All")]
        public IActionResult RoleGetAll()
        {
            try
            {
                var oResponse = RoleController.GetDbRoles();
                return StatusCode((int)oResponse.status, oResponse);
            }
            catch (Exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "Error al obtener los roles");
            }
        }
    }
}
