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
using Quest.API.BindingModels.Teams;
using Quest.API.Helpers;
using Quest.API.Helpers.Errors;
using Quest.API.ResourceModels.Participants;
using Quest.API.ResourceModels.Teams;
using Quest.API.ResourceModels.Users;
using Quest.API.Services;
using Quest.Application.Participants.Commands;
using Quest.Application.Services;
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
            var team = await _mediator.Send(new GetParticipantInfoQuery(id));

            if (team == null)
            {
                return NotFound();
            }

            return Json(new TeamWithCaptainAndMembersRM(team as Team));
        }


        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Post(CreateTeamBM model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var userId = _userManager.GetUserId(User);

            var createTeamArgs = new TeamConstructorArgs
            {
                Name = model.Name,
                PrincipalUserId = userId,
                QuestId = model.QuestId
            };
            
            var createTeamCommand = new CreateParticipantCommand(createTeamArgs);

            var response = await _mediator.Send(createTeamCommand);

            if (response.Result == null)
                return ApiError.ProblemDetails(HttpStatusCode.Forbidden, response.Message);

            var team = response.Result as Team;
            
            return Created("/team/" + team.Id, new
            {
                teamId = response.Result.Id,
                inviteLink = "/invites/" + team.InviteTokenSecret
            });
        }


        [Authorize]
        [HttpPost("{teamId}/members")]
        public async Task<IActionResult> AddUserToTeam(int teamId, [FromBody] AddUserToTeamBM model)
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

            var team = await _mediator.Send(new GetParticipantInfoQuery(teamId));
            
            return Ok(new ParticipantRM(team));
        }


        [Authorize]
        [HttpDelete("{teamId}/members/{userToKickId}")]
        public async Task<IActionResult> KickUserFromTheTeam(int teamId, string userToKickId)
        {
            var userId = _userManager.GetUserId(User);

            if (userToKickId == "currentUser")
                userToKickId = userId;
                
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

            var response = await _mediator.Send(new RemoveParticipantCommand(userId, teamId));

            if (!response.Result)
                return ApiError.ProblemDetails(HttpStatusCode.Forbidden, response.Message);

            return Ok(response.Message);
        }


        [Authorize]
        [HttpPost("{teamId}/moderator/")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AssignModeratorToTeam(int teamId,
            [FromBody]AssignModeratorToTeamBM model)
        {
            var userId = _userManager.GetUserId(User);
            
            var response = await _mediator.Send(
                new AssignModeratorToParticipantCommand(teamId, userId, model.ModeratorId));

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

            return Ok(new ParticipantWithQuestAndModeratorRM(response));
        }
        
        [Authorize]
        [Authorize(Roles = "Admin")]
        [HttpGet("{id}/moderator")]
        public async Task<IActionResult> GetTeamModerator(int id)
        {
            var response = await _mediator.Send(
                new GetParticipantInfoQuery(id));

            if (response?.Moderator == null)
                return NotFound();

            return Ok(new ModeratorRm(response.Moderator));
        }
        
         
        [Authorize]
        [HttpGet("{id}/inviteCode")]
        public async Task<IActionResult> GetTeamInviteCode(int id)
        {
            //quick implementation, redesign to separate request later
            var userId = _userManager.GetUserId(User);

            var response = await _mediator.Send(
                new GetParticipantInfoQuery(id));

            if (response == null)
                return NotFound();
            var team = response as Team;
            
            if (team.Members.All(x => x.UserId != userId))
                return ApiError.ProblemDetails(HttpStatusCode.Forbidden, "You are not a member of this team");
            
            return Ok(team.InviteTokenSecret);
        }
    }
}