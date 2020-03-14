using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Quest.API.Models.ViewModels.Quests;
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

        public QuestsController(Db context, UserManager<ApplicationUser> userManager)
        {
            _db = context;
            _userManager = userManager;
        }
        
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Get()
        {
            var data = _db.Quests.ToList();
            return Ok(data);
        }
        
        [HttpGet("{id}")]
        [AllowAnonymous]
        public IActionResult Get(int id)
        {
            var quest = _db.Quests
                .Where(x => x.Id == id)
                .Include(x => x.Author)
                .Include(x => x.AppUserQuests)
                .ThenInclude(x=> x.User)
                .Include(x => x.Teams)
                .FirstOrDefault((x) => x.Id == id);

            if (quest == null)
            {
                return NotFound();
            }
            return Json(new QuestInfoVM(quest));
        }
        
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Post(CreateQuestVM model)
        {
            var userId = _userManager.GetUserId(User);

            var author = await _db.Users.FirstOrDefaultAsync(x => x.UserName == userId);
            if (ModelState.IsValid && author != null)
            {
                var quest = new QuestEntity
                {
                    Name = model.Name,
                    Description = model.Description,
                    ImageUrl = model.ImageUrl,
                    StartDate = DateTime.Now + TimeSpan.FromDays(7),
                    RegistrationDeadline = DateTime.Now + TimeSpan.FromDays(3),
                    AuthorId = author.Id
                };

                await _db.Quests.AddAsync(quest);
                await _db.SaveChangesAsync();

                return Created("/quests/" + quest.Id, quest.Id);
            }

            return BadRequest();
        }
    }
}