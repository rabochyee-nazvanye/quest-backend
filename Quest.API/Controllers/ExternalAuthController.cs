using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Quest.API.BindingModels.ExternalAuth;
using Quest.API.Helpers.Errors;
using Quest.API.Services;
using Quest.Application.ExternalAuth.Commands;
using Quest.Domain.Models;

namespace Quest.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ExternalAuthController : Controller
    {
        private readonly IMediator _mediator;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ITokenService _tokenService;

        public ExternalAuthController(IMediator mediator, UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager, ITokenService tokenService)
        {
            _mediator = mediator;
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
        }
        
        [HttpPost("authenticate")]
        [AllowAnonymous]
        public async Task<IActionResult> Facebook([FromBody] ExternalAuthBM model)
        {
            var response = await _mediator.Send(
                new AuthenticateCommand(model.AccessToken, model.OAuthProvider));

            if (response.Result == null)
            {
                return ApiError.ProblemDetails(HttpStatusCode.Forbidden, response.Message);
            }

            var user = response.Result;
            var userPrincipal = await _signInManager.CreateUserPrincipalAsync(user);
            await _signInManager.Context.SignInAsync(IdentityConstants.ApplicationScheme,
                userPrincipal,
                new AuthenticationProperties { IsPersistent = true });

            return Ok(new {
                token = new
                {
                    result = await _tokenService.BuildToken(user.UserName)
                }
            });
        }
    }
}