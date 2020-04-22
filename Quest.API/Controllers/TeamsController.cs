using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Quest.API.Helpers;
using Quest.API.Helpers.Errors;
using Quest.API.Services;
using Quest.API.ViewModels.Teams;
using Quest.API.ViewModels.Users;
using Quest.Application.Teams.Commands;
using Quest.Application.Teams.Queries;
using Quest.Application.Users.Queries;
using Quest.DAL.Data;
using Quest.Domain.Models;


namespace Quest.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TeamsController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMediator _mediator;

        public TeamsController(UserManager<ApplicationUser> userManager, IMediator mediator)
        {
            _userManager = userManager;
            _mediator = mediator;
        }


        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetTeamById(int id)
        {
            var team = await _mediator.Send(new GetTeamInfoQuery(id));

            if (team == null)
            {
                return NotFound();
            }

            return Json(new TeamWithCaptainAndMembersVM(team));
        }


        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Post(CreateTeamVM model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var userId = _userManager.GetUserId(User);

            var createTeamCommand = new CreateTeamCommand(model.Name, userId, model.QuestId);

            var response = await _mediator.Send(createTeamCommand);

            if (response.Result == null)
                return ApiError.ProblemDetails(HttpStatusCode.Forbidden, response.Message);

            return Created("/team/" + response.Result.Id, new
            {
                teamId = response.Result.Id,
                inviteLink = "/invites/" + response.Result.InviteTokenSecret
            });
        }


        [Authorize]
        [HttpPost("{teamId}/members")]
        public async Task<IActionResult> AddUserToTeam(int teamId, [FromBody] AddUserToTeamVM model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var userId = _userManager.GetUserId(User);

            var command = new AddUserToTeamCommand(userId, model.UserId, model.RequestSecret, teamId);

            var response = await _mediator.Send(command);

            if (!response.Result)
                return ApiError.ProblemDetails(HttpStatusCode.Forbidden, response.Message);

            var team = await _mediator.Send(new GetTeamInfoQuery(teamId));
            
            return Ok(new TeamVM(team));
        }


        [Authorize]
        [HttpDelete("{teamId}/members/{userToKickId}")]
        public async Task<IActionResult> KickUserFromTheTeam(int teamId, string userToKickId)
        {
            var userId = _userManager.GetUserId(User);

            var response = await _mediator.Send(new RemoveUserFromTeamCommand(teamId, userId, userToKickId));

            if (!response.Result)
                return ApiError.ProblemDetails(HttpStatusCode.Forbidden, response.Message);

            return Ok(response.Message);
        }

        [Authorize]
        [HttpDelete("{teamId}")]
        public async Task<IActionResult> RemoveTeam(int teamId)
        {
            var userId = _userManager.GetUserId(User);

            var response = await _mediator.Send(new RemoveTeamCommand(userId, teamId));

            if (!response.Result)
                return ApiError.ProblemDetails(HttpStatusCode.Forbidden, response.Message);

            return Ok(response.Message);
        }


        [Authorize]
        [HttpPost("{teamId}/moderator/")]
        public async Task<IActionResult> AssignModeratorToTeam(int teamId,
            [FromBody]AssignModeratorToTeamVM model)
        {
            var userId = _userManager.GetUserId(User);
            //todo add verification that user has privileges to assign moderator to quest
            var user = await _mediator.Send(new GetUserByIdQuery(userId));
            if (user.UserName != "admin_user")
                return ApiError.ProblemDetails(HttpStatusCode.Forbidden, 
                    "You don't have an access to task creation");

            var response = await _mediator.Send(
                new AssignModeratorToTeamCommand(teamId, userId, model.ModeratorId));

            if (!response.Result)
                return ApiError.ProblemDetails(HttpStatusCode.Forbidden, response.Message);

            return Ok(response.Message);
        }

        [Authorize]
        [HttpGet]
        [ExactQueryParam("action", "requestSecret")]
        public async Task<IActionResult> GetTeamBySecretCode([FromQuery]string action, [FromQuery]string requestSecret)
        {
            var userId = _userManager.GetUserId(User);
            if (action != "lookupByInviteCode" || string.IsNullOrEmpty(requestSecret))
                return BadRequest();
            
            var response = await _mediator.Send(
                new GetTeamBySecretQuery(requestSecret));

            if (response == null)
                return NotFound();

            return Ok(new TeamWithQuestAndModeratorVM(response));
        }
        
        [Authorize]
        [HttpGet("{id}/moderator")]
        public async Task<IActionResult> GetTeamModerator(int id)
        {
            var userId = _userManager.GetUserId(User);
            //todo add verification that user has privileges to do team lookup by invite code
            var user = await _mediator.Send(new GetUserByIdQuery(userId));
            if (user.UserName != "admin_user")
                return ApiError.ProblemDetails(HttpStatusCode.Forbidden, 
                    "You don't have an access to task creation");

            var response = await _mediator.Send(
                new GetTeamInfoQuery(id));

            if (response?.Moderator == null)
                return NotFound();

            return Ok(new ModeratorVM(response.Moderator));
        }
        
         
        [Authorize]
        [HttpGet("{id}/inviteCode")]
        public async Task<IActionResult> GetTeamInviteCode(int id)
        {
            //quick implementation, redesign to separate request later
            var userId = _userManager.GetUserId(User);

            var response = await _mediator.Send(
                new GetTeamInfoQuery(id));

            if (response == null)
                return NotFound();

            if (response.Members.All(x => x.UserId != userId))
                return ApiError.ProblemDetails(HttpStatusCode.Forbidden, "You are not a member of this team");
            
            return Ok(response.InviteTokenSecret);
        }
    }
}