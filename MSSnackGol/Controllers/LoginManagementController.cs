using LibraryEntities;
using LibraryEntities.Models;
using Microsoft.AspNetCore.Mvc;

namespace MSSnackGol.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginManagementController : ControllerBase
    {

        public LoginManagementController()
        {
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest oLogin)
        {
            throw new NotImplementedException();
        }
    }
}
