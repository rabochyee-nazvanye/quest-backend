using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Quest.API.Helpers.Errors;
using Quest.API.ViewModels.Teams;
using Quest.Application.Participants.Commands;
using Quest.Application.Teams.Commands;
using Quest.Application.Teams.Queries;
using Quest.DAL.Data;
using Quest.Domain.Models;

namespace Quest.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InvitesController : Controller
    {
        private readonly Db _context;
        private readonly IMediator _mediator;
        private readonly UserManager<ApplicationUser> _userManager;

        public InvitesController(IMediator mediator, Db context, UserManager<ApplicationUser> userManager)
        {
            _mediator = mediator;
            _context = context;
            _userManager = userManager;
        }

        [HttpGet("{inviteCode}")]
        [Authorize]
        public async Task<IActionResult> JoinTeamViaInviteCode(string inviteCode)
        {
            if (string.IsNullOrEmpty(inviteCode))
            {
                ModelState.AddModelError("Id", "Invite code should not be empty");
                return BadRequest();
            }

            var userId = _userManager.GetUserId(User);

            var team = await _mediator.Send(new GetTeamBySecretQuery(inviteCode));

            if (team == null)
                return ApiError.ProblemDetails(HttpStatusCode.Forbidden, "Invalid code");

            var result = await _mediator.Send(
                new AddUserToTeamCommand(userId, userId, inviteCode, team.Id));

            if (!result.Result)
                return ApiError.ProblemDetails(HttpStatusCode.Forbidden, result.Message);
            
            return Ok(new ParticipantDTO(team));
        }
    }
}
