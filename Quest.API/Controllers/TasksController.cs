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
using Quest.API.BindingModels.TaskAttempts;
using Quest.API.Helpers;
using Quest.API.Helpers.Errors;
using Quest.API.ResourceModels.Hints;
using Quest.API.ResourceModels.Tasks;
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
        public async Task<IActionResult> SubmitTaskAttempt(int id, [FromBody]TaskAttemptRM model)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            
            var userId = _userManager.GetUserId(User);
            
            var response = await _mediator.Send(new SubmitTaskAttemptCommand
            {
                AttemptText = model.AttemptText,
                TaskId = id,
                UserId = userId
            });

            if (response.Result == null)
                return ApiError.ProblemDetails(HttpStatusCode.Forbidden, response.Message);
            
            return Ok(new TaskWithStatusAndHintsRM(response.Result));
        }
        
        [Authorize]
        [HttpPost("{id}/hintrequests/{hintNumber}")]
        public async Task<IActionResult> SubmitHintRequest(int id, int hintNumber)
        {
            var userId = _userManager.GetUserId(User);

            var response = await _mediator.Send(new SubmitHintRequestCommand
            {
                UserId = userId,
                TaskId = id,
                HintNumber = hintNumber
            });
            
            if (response.Result == null)
                return ApiError.ProblemDetails(HttpStatusCode.Forbidden, response.Message);
            
            return Ok(new HintRM(response.Result));
        }
        
        [Authorize(Roles="Admin")]
        [HttpPost("{id}/attempts/{attemptId}")]
        public async Task<IActionResult> UpdateTaskAttempt(int id, int attemptId, [FromBody]AttemptFeedbackBM attemptFeedback)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            
            var response = await _mediator.Send(new VerifyTaskAttemptCommand
            {
                AttemptId = attemptId,
                Message = attemptFeedback.Comment,
                IsCorrect = attemptFeedback.IsCorrect
            });

            if (!response.Result)
                return ApiError.ProblemDetails(HttpStatusCode.Forbidden, response.Message);
            
            return Ok();
        }
    }
}