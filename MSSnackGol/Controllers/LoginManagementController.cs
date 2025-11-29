using LibraryAuthentication;
using LibraryConnection.ControllerAzure;
using LibraryEntities;
using LibraryEntities.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace MSSnackGol.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginManagementController : ControllerBase
    {
        private readonly JwtTokenHandler _jwtTokenHandler;

        public LoginManagementController(JwtTokenHandler jwtTokenHandler)
        {
            _jwtTokenHandler = jwtTokenHandler;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest oLogin)
        {
            try
            {
                //Primero verificar que exista el usuario en la base de datos
                UserAuthenticationController userAuthController = new UserAuthenticationController();
                bool isValidUser = userAuthController.VerifyUserAndPassword(oLogin);
                if (!isValidUser)
                {
                    return StatusCode(401, new Response<dynamic>(false, HttpStatusCode.Unauthorized, "Invalid username or password"));
                }

                var authenticationResponse = _jwtTokenHandler.GenerateJwtToken(oLogin);
                if (authenticationResponse == null)
                {
                    return StatusCode(401, new Response<dynamic>(false, HttpStatusCode.Unauthorized, Unauthorized()));
                }
                if (authenticationResponse.status == HttpStatusCode.OK)
                {
                    return Ok(authenticationResponse);
                }
                else
                {
                    return StatusCode((int)authenticationResponse.status, authenticationResponse);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response<dynamic>(false, HttpStatusCode.InternalServerError, "Error Authenticate", ex.Message));
            }
        }
    }
}
