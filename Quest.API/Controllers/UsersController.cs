using System;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
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
using Quest.API.Helpers;
using Quest.API.Models;
using Quest.API.Models.ViewModels.Accounts;
using Quest.API.Models.ViewModels.Profiles;
using Quest.API.Services;
using Quest.Application.Accounts.Queries;
using Quest.DAL.Data;
using Quest.Domain.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Quest.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UsersController : Controller
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{name}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetUserByName(string name)
        {
            var user = await _mediator.Send(new GetAccountByNameQuery(name));

            if (user == null)
            {
                return BadRequest("User with that username not found.");
            }

            return Json(new AccountVM(user));
        }
    }
}
