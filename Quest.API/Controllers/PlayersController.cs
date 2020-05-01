using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Quest.API.BindingModels.SoloPlayers;
using Quest.API.BindingModels.Teams;
using Quest.API.Helpers.Errors;
using Quest.API.ResourceModels.Participants;
using Quest.API.ResourceModels.Users;
using Quest.Application.Participants.Commands;
using Quest.Application.Services;
using Quest.Application.Teams.Commands;
using Quest.Application.Teams.Queries;
using Quest.Domain.Models;

namespace Quest.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PlayersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMediator _mediator;

        public PlayersController(UserManager<ApplicationUser> userManager, IMediator mediator)
        {
            _userManager = userManager;
            _mediator = mediator;
        }


        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetPlayerById(int id)
        {
            var player = await _mediator.Send(new GetParticipantInfoQuery(id));

            if (player == null)
            {
                return NotFound();
            }

            return Json(new ParticipantRM(player));
        }


        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Post(CreatePlayerBM model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var user = await _userManager.GetUserAsync(User);

            var createTeamArgs = new SoloPlayerConstructorArgs
            {
                Name = user.UserName,
                PrincipalUserId = user.Id,
                QuestId = model.QuestId
            };
            
            var createPlayerCommand = new CreateParticipantCommand(createTeamArgs);

            var response = await _mediator.Send(createPlayerCommand);

            if (response.Result == null)
                return ApiError.ProblemDetails(HttpStatusCode.Forbidden, response.Message);

            var player = response.Result as SoloPlayer;
            
            return Created("/player/" + player.Id, new
            {
                playerId = response.Result.Id
            });
        }

        [Authorize]
        [HttpDelete("{playerId}")]
        public async Task<IActionResult> RemovePlayer(int playerId)
        {
            var userId = _userManager.GetUserId(User);

            var response = await _mediator.Send(new RemoveParticipantCommand(userId, playerId));

            if (!response.Result)
                return ApiError.ProblemDetails(HttpStatusCode.Forbidden, response.Message);

            return Ok(response.Message);
        }


        [Authorize]
        [HttpPost("{playerId}/moderator/")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AssignModeratorToPlayer(int playerId,
            [FromBody]AssignModeratorToTeamBM model)
        {
            var userId = _userManager.GetUserId(User);
            
            var response = await _mediator.Send(
                new AssignModeratorToParticipantCommand(playerId, userId, model.ModeratorId));

            if (!response.Result)
                return ApiError.ProblemDetails(HttpStatusCode.Forbidden, response.Message);

            return Ok(response.Message);
        }

        [Authorize]
        [Authorize(Roles = "Admin")]
        [HttpGet("{id}/moderator")]
        public async Task<IActionResult> GetPlayerModerator(int id)
        {
            var response = await _mediator.Send(
                new GetParticipantInfoQuery(id));

            if (response?.Moderator == null)
                return NotFound();

            return Ok(new ModeratorRm(response.Moderator));
        }
    }
}