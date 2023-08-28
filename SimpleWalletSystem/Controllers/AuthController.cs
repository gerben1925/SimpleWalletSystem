using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimpleWalletSystem.Model;
using SimpleWalletSystem.Services;

namespace SimpleWalletSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuth _authService;
        public AuthController(IAuth auth)
        {
            _authService = auth;
        }

        [HttpPost("Register")]
        public IActionResult registerUser([FromBody] RegisterUser newUser)
        {
            try
            {
                bool validateUsername = _authService.ValidateUser(newUser.Username);
                if (validateUsername == true)
                {
                    return Conflict("Username already exist! ");
                }
                DataSet ds = _authService.NewUser(newUser);
                return Ok("Successfully Register!");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }
    }
}
