using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Mime;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Quest.API.Models;
using Quest.API.Services;
using Quest.Domain.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Quest.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class AccountController : Controller
    {
        private readonly IConfiguration _config;
        private readonly ITokenService _tokenService;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(IConfiguration config, ITokenService tokenService, SignInManager<ApplicationUser> signInManager)
        {
            _config = config;
            _tokenService = tokenService;
            _signInManager = signInManager;
        }


        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody]LoginModel login)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await _signInManager.PasswordSignInAsync(login.Username, login.Password, true, false);
            if (result.Succeeded)
            {
                return Ok(new
                {
                    token = _tokenService.BuildToken(login.Username)
                });
            }

            return Unauthorized("Wrong username or password.");
        }
    }
}
