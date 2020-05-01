using System;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Mime;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using Quest.API.BindingModels.Sessions;
using Quest.API.Helpers;
using Quest.API.Helpers.Errors;
using Quest.API.ResourceModels.Users;
using Quest.API.Services;
using Quest.Application.Users.Queries;
using Quest.DAL.Data;
using Quest.Domain.Models;

namespace Quest.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class SessionController : Controller
    {
        private readonly IConfiguration _config;
        private readonly ITokenService _tokenService;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMediator _mediator;

        public SessionController(IConfiguration config, ITokenService tokenService, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, Db context, IMediator mediator)
        {
            _config = config;
            _tokenService = tokenService;
            _signInManager = signInManager;
            _userManager = userManager;
            _mediator = mediator;
        }
        
        
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody]LoginBM login)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await _signInManager.PasswordSignInAsync(login.Username, login.Password, true, false);
            if (result.Succeeded)
            {
                return Ok(new
                {
                    token = new
                    {
                        result = await _tokenService.BuildToken(login.Username)
                    }
                });
            }

            return ApiError.ProblemDetails(HttpStatusCode.Forbidden, "Wrong username or password.");
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetLoggedInUser()
        {
            var userId = _userManager.GetUserId(User);

            var user = await _mediator.Send(new GetUserByIdQuery(userId));

            if (user == null)
            {
                return ApiError.ProblemDetails(HttpStatusCode.InternalServerError, 
                    "Failed to load user details");
            }

            return Ok(new UserRM(user));
        }
    }
}
