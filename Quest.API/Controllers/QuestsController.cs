using System;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Quest.API.BindingModels.Quests;
using Quest.API.BindingModels.Tasks;
using Quest.API.Helpers;
using Quest.API.Helpers.Errors;
using Quest.API.ResourceModels.Participants;
using Quest.API.ResourceModels.Quests;
using Quest.API.ResourceModels.Quests.Results;
using Quest.API.ResourceModels.Tasks;
using Quest.API.ResourceModels.Teams;
using Quest.API.Services;
using Quest.Application.Quests.Commands;
using Quest.Application.Quests.Queries;
using Quest.Application.Services;
using Quest.Application.Tasks.Commands;
using Quest.Application.Tasks.Queries;
using Quest.Application.Teams.Queries;
using Quest.Application.Users.Queries;
using Quest.DAL.Data;
using Quest.Domain.Models;

namespace Quest.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class QuestsController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMediator _mediator;

        public QuestsController(UserManager<ApplicationUser> userManager, IMediator mediator)
        {
            _userManager = userManager;
            _mediator = mediator;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Get()
        {
            var userId = _userManager.GetUserId(User);
            var data = await _mediator.Send(new GetAllQuestsQuery(userId));
            return Ok(data.Select(QuestRMFactory.CreateBasic));
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> Get(int id)
        {
            var userId = _userManager.GetUserId(User);
            var quest = await _mediator.Send(new GetQuestByIdQuery(id, userId));

            if (quest == null)
                return NotFound();
            
            return Ok(QuestRMFactory.CreateDetailed(quest));
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateQuest(CreateQuestBM model)
        {
            var userId = _userManager.GetUserId(User);
            if (!ModelState.IsValid)
                return BadRequest();
            IQuestConstructorArgs questConstructorArgs;
            
            try
            {
                questConstructorArgs = CreateQuestArgsFactory.Create(model, userId);
            }
            catch (ArgumentOutOfRangeException exception)
            {
                return ApiError.ProblemDetails(HttpStatusCode.Forbidden, exception.Message);
            }
            
            var response = await _mediator.Send(new CreateQuestCommand(questConstructorArgs));
            if (response.Result == null)
                return ApiError.ProblemDetails(HttpStatusCode.Forbidden, response.Message);

            return Created("/quests/" + response.Result.Id, response.Result.Id);
        }

        [Authorize]
        [HttpGet("{id}/participants")]
        [ExactQueryParam("members")]
        public async Task<IActionResult> GetQuestTeamByUserId(int id, [FromQuery]string members)
        {
            var userId = _userManager.GetUserId(User);
            
            if (string.IsNullOrEmpty(members))
                return BadRequest();

            var memberIds = members.Split("|")
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(x => x != "currentUser" ? x : userId)
                .Distinct()
                .ToList();

            if (!memberIds.Any())
                return BadRequest();
            
            var response = await _mediator.Send(new GetParticipantsByUserAndQuestQuery(id, memberIds));

            if (response.Result == null)
                return ApiError.ProblemDetails(HttpStatusCode.Forbidden, response.Message);

            return Ok(response.Result.Select(ParticipantRMFactory.Create));
        }
        
        [Authorize]
        [HttpGet("{id}/tasks")]
        public async Task<IActionResult> GetQuestTasks(int id)
        {
            var userId = _userManager.GetUserId(User);
            
            var response = await _mediator.Send(new GetQuestTasksQuery(userId, id));

            if (response.Result == null)
                return ApiError.ProblemDetails(HttpStatusCode.Forbidden, response.Message);

            return Ok(response.Result.Select(x => new TaskWithStatusAndHintsRM(x)));
        }
        
        [Authorize]
        [HttpPost("{id}/tasks")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateTask(int id, [FromBody]CreateTaskBM model)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            
            var response = await _mediator.Send(new CreateTaskCommand()
            {
                CorrectAnswer =  model.CorrectAnswer,
                Group = model.Group,
                Name = model.Name,
                QuestId = id,
                Question = model.Question,
                Reward =  model.Reward,
                VerificationIsManual = model.VerificationIsManual,
                Hints = model.Hints
            });

            if (response.Result == null)
                return ApiError.ProblemDetails(HttpStatusCode.Forbidden, response.Message);

            return Created($"/quests/{id}/tasks/{response.Result.Id}",response.Result.Id);
        }
        
        [AllowAnonymous]
        [HttpGet("{id}/scoreboard")]
        public async Task<IActionResult> GetQuestScoreboard(int id)
        {
            var response = await _mediator.Send(new GetQuestScoreboardQuery(id));

            if (response.Result == null)
                return ApiError.ProblemDetails(HttpStatusCode.Forbidden, response.Message);

            return Ok(new QuestScoreboardInvertedScoreRM(response.Result));
        }
        
        [Authorize]
        [HttpGet("{id}/progressboard")] 
        public async Task<IActionResult> GetQuestModeratorBoard(int id)
        {
            var userId = _userManager.GetUserId(User);

            var response = await _mediator.Send(new GetQuestProgressboardQuery(userId, id));

            if (response.Result == null)
                return ApiError.ProblemDetails(HttpStatusCode.Forbidden, response.Message);

            return Ok(new QuestProgressboardRM(response.Result));
        }
    }
}