using System;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Mime;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using NpgsqlTypes;
using Quest.API.BindingModels.Users;
using Quest.API.Helpers;
using Quest.API.Helpers.Errors;
using Quest.API.ResourceModels.Users;
using Quest.API.Services;
using Quest.Application.Users.Commands;
using Quest.Application.Users.Queries;
using Quest.DAL.Data;
using Quest.Domain.Models;

namespace Quest.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UsersController : Controller
    {
        private readonly IMediator _mediator;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ITokenService _tokenService;

        public UsersController(IMediator mediator, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ITokenService tokenService)
        {
            _mediator = mediator;
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
        }

        [HttpGet("{name}")]
        public async Task<IActionResult> GetUserByName(string name)
        {
            var user = await _mediator.Send(new GetUserByNameQuery(name));

            if (user == null)
            {
                return NotFound("User with that username not found.");
            }

            return Json(new UserRM(user));
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> CreateNewUser(CreateUserBM model)
        {
            var userId = _userManager.GetUserId(User);
            if (userId != null)
                return ApiError.ProblemDetails(HttpStatusCode.Forbidden, "Cannot register while logged in!");
            
            if (!ModelState.IsValid)
                return BadRequest();

            var response = await _mediator.Send(new CreateUserCommand(model.Username, model.Password));
            if (response.Result == null)
                return ApiError.ProblemDetails(HttpStatusCode.Forbidden, response.Message);

            var user = response.Result;
            var userPrincipal = await _signInManager.CreateUserPrincipalAsync(user);
            await _signInManager.Context.SignInAsync(IdentityConstants.ApplicationScheme,
                userPrincipal,
                new AuthenticationProperties { IsPersistent = true });

            return Created($"/users/{user.Id}", new
            {
                token = new
                {
                    result = await _tokenService.BuildToken(user.UserName)
                }
            });
        }
    }
}
