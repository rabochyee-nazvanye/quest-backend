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
using Quest.API.ViewModels.TaskAttempts;
using Quest.API.ViewModels.Tasks;
using Quest.API.ViewModels.Teams;
using Quest.Application.Quests.Commands;
using Quest.Application.Quests.Queries;
using Quest.Application.Tasks.Commands;
using Quest.Application.Tasks.Queries;
using Quest.Application.Teams.Queries;
using Quest.DAL.Data;
using Quest.Domain.Models;


namespace Quest.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TasksController : Controller
    {
        private readonly Db _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMediator _mediator;

        public TasksController(Db context, UserManager<ApplicationUser> userManager, IMediator mediator)
        {
            _db = context;
            _userManager = userManager;
            _mediator = mediator;
        }

        [Authorize]
        [HttpPost("{id}/attempts")]
        public async Task<IActionResult> SubmitTaskAttempt(int id, [FromBody]TaskAttemptVM model)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            
            var userId = _userManager.GetUserId(User);
            
            var response = await _mediator.Send(new SubmitTaskAttemptCommand
            {
                AttemptText = model.AttemptText,
                TaskId = id,
                TeamId = model.TeamId,
                UserId = userId
            });

            if (response.Result == null)
                return ApiError.ProblemDetails(HttpStatusCode.Forbidden, response.Message);

            return Ok(new TaskVM(response.Result));
        }
        
        [Authorize]
        [HttpGet("{id}/hints/{hintNumber}")]
        public async Task<IActionResult> GetQuestHint(int id, int hintNumber)
        {
            var userId = _userManager.GetUserId(User);
            
            //todo
            throw new NotImplementedException();
            //
            // if (response.Result == null)
            //     return ApiError.ProblemDetails(HttpStatusCode.Forbidden, response.Message);
            //
            // return Ok(response.Result.Select(x => new TaskVM(x)));
        }
    }
}