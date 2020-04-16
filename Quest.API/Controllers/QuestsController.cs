using System;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Quest.API.ViewModels.Quests;
using Quest.Application.Quests.Commands;
using Quest.Application.Quests.Queries;
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
            return Json(data.Select(x => new QuestVM(x)));
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
            return Json(new QuestVM(quest));
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
                return BadRequest(response.Message);

            return Created("/quests/" + response.Result.Id, response.Result.Id);
        }
    }
}