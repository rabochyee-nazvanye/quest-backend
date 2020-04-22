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
using Quest.API.Helpers;
using Quest.API.Helpers.Errors;
using Quest.API.ViewModels.Quests;
using Quest.API.ViewModels.Tasks;
using Quest.API.ViewModels.Teams;
using Quest.Application.Quests.Commands;
using Quest.Application.Quests.Queries;
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
        private readonly Db _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMediator _mediator;

        public QuestsController(Db context, UserManager<ApplicationUser> userManager, IMediator mediator)
        {
            _db = context;
            _userManager = userManager;
            _mediator = mediator;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Get()
        {
            var data = await _mediator.Send(new GetAllQuestsQuery());
            return Ok(data.Select(x => new QuestWithTeamsAndAuthorVM(x)));
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> Get(int id)
        {
            var quest = await _mediator.Send(new GetQuestByIdQuery(id));

            if (quest == null)
            {
                return NotFound();
            }
            return Json(new QuestWithTeamsAndAuthorVM(quest));
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Post(CreateQuestVM model)
        {
            var userId = _userManager.GetUserId(User);
            if (!ModelState.IsValid)
                return BadRequest();

            var response = await _mediator.Send(new CreateQuestCommand
            {
                AuthorId = userId,
                Description = model.Description,
                ImageUrl = model.ImageUrl,
                Name = model.Name,
                RegistrationDeadline = model.RegistrationDeadline,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                MaxTeamSize = model.MaxTeamSize
            });

            if (response.Result == null)
                return ApiError.ProblemDetails(HttpStatusCode.Forbidden, response.Message);

            return Created("/quests/" + response.Result.Id, response.Result.Id);
        }
        
        [Authorize]
        [HttpGet("{id}/teams")]
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
            
            var response = await _mediator.Send(new GetTeamByUserAndQuestQuery(id, memberIds));

            if (response.Result == null)
                return ApiError.ProblemDetails(HttpStatusCode.Forbidden, response.Message);

            return Ok(response.Result.Select(x => new TeamWithCaptainAndMembersVM(x)));
        }
        
        [Authorize]
        [HttpGet("{id}/tasks")]
        public async Task<IActionResult> GetQuestTasks(int id)
        {
            var userId = _userManager.GetUserId(User);
            
            var response = await _mediator.Send(new GetQuestTasksQuery(userId, id));

            if (response.Result == null)
                return ApiError.ProblemDetails(HttpStatusCode.Forbidden, response.Message);

            return Ok(response.Result.Select(x => new TaskVM(x)));
        }
        
        [Authorize]
        [HttpPost("{id}/tasks")]
        public async Task<IActionResult> CreateTask(int id, [FromBody]CreateTaskVM model)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            
            //todo add proper verification for user priveleges later
            var userId = _userManager.GetUserId(User);
            var user = await _mediator.Send(new GetUserByIdQuery(userId));
            if (user.UserName != "admin_user")
                return ApiError.ProblemDetails(HttpStatusCode.Forbidden, 
                    "You don't have an access to task creation");
            
            var response = await _mediator.Send(new CreateTaskCommand()
            {
                CorrectAnswer =  model.CorrectAnswer,
                Group = model.Group,
                Name = model.Name,
                QuestId = id,
                Question = model.Question,
                Reward =  model.Reward,
                VerificationIsManual = model.VerificationIsManual
            });

            if (response.Result == null)
                return ApiError.ProblemDetails(HttpStatusCode.Forbidden, response.Message);

            return Created($"/quests/{id}/tasks/{response.Result.Id}",response.Result.Id);
        }
    }
}