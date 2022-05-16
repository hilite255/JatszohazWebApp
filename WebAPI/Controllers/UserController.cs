using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Domain.Services;
using WebAPI.DTOs;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace WebAPI.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService userService;

        public UserController(UserService userService)
        {
            this.userService = userService;
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody]RegisterDto user)
        {
            var res = await userService.Register(user.Username, user.Password, user.Email);
            if (res)
                return Ok();
            else
                return BadRequest();
        }

        [HttpPost("login")]
        public async  Task<IActionResult> Login([FromBody]LoginDto user)
        {
            var res = await userService.Login(user.Username, user.Password);
            if (res == null)
                return BadRequest("Wrong username or password");
            return Ok(res);
                
        }
    }
}
