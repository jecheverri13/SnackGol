using LibraryEntities.Models;
using Microsoft.AspNetCore.Mvc;

namespace MSSnackGol.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginManagmentController : ControllerBase
    {

        public LoginManagmentController()
        {
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] RequestSL oLogin)
        {
            throw new NotImplementedException();
        }
    }
}
