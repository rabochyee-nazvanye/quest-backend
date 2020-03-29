using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Quest.API.Models.ViewModels.Teams;
using Quest.DAL.Data;
using Quest.Domain.Models;


namespace Quest.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TeamController : Controller
    {
        private readonly Db _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public TeamController(Db context, UserManager<ApplicationUser> userManager)
        {
            _db = context;
            _userManager = userManager;
        }


        [HttpGet("{id}")]
        [AllowAnonymous]
        public IActionResult GetTeamById(int id)
        {
            var team = _db.Teams
                .Where(x => x.Id == id)
                .Include(x => x.TeamUsers)
                .ThenInclude(x => x.User)
                .Include(x => x.TeamUsers)
                .FirstOrDefault((x) => x.Id == id);

            if (team == null)
            {
                return NotFound();
            }

            return Json(new TeamInfoVM(team));
        }


        [Authorize]
        [HttpPost("create")]
        public async Task<IActionResult> Post(CreateTeamInfoVM model)
        {
            var userId = _userManager.GetUserId(User);

            var capitan = await _db.Users.FirstOrDefaultAsync(x => x.UserName == userId);
            if (!ModelState.IsValid || capitan == null)
            {
                return BadRequest();
            }

            var quest = _db.Quests.FirstOrDefault(x => x.Id == model.QuestId);

            if (quest == null)
            {
                return BadRequest("Couldn't find quest with that ID");
            }

            var capitanTeams = quest.Teams.Where(x => x.TeamUsers.Any(y => y.UserId == capitan.Id));
            if (capitanTeams.Any())
            {
                return BadRequest("This user is currently tied with one team");
            }

            if (quest.RegistrationDeadline < DateTime.Now)
            {
                return BadRequest("You can't register to that event no more");
            }

            if (_db.Teams.Any(x => x.Name == model.Name))
            {
                return BadRequest("The team with that name already exists");
            }

            var team = new Team()
            {
                Name = model.Name,
                TeamUsers = new List<TeamUser>()
            };

            team.TeamUsers.Add(new TeamUser()
            {
                Team = team,
                User = capitan,
                UserId = capitan.Id
            });

            if (quest.Teams == null)
            {
                quest.Teams = new List<Team>();
            }

            quest.Teams.Add(team);

            await _db.Teams.AddAsync(team);
            await _db.SaveChangesAsync();

            return Created("/team/" + team.Id, team.Id);
        }


        [Authorize]
        [HttpPost("add/{requestSecret}")]
        public async Task<IActionResult> AddUserToTeam(string requestSecret)
        {
            var userId = _userManager.GetUserId(User);

            var requestTeamParsed = requestSecret.Split('-');
            int.TryParse(requestTeamParsed.First(), out var teamId);
            var requestTeamSecret = requestTeamParsed.Last();

            var user = await _db.Users.FirstOrDefaultAsync(x => x.UserName == userId);
            if (user == null)
            {
                return BadRequest("Couldn't find quest with that ID");
            }

            var team = await _db.Teams.FirstOrDefaultAsync(x => x.Id == teamId);
            if (team == null)
            {
                return BadRequest("Couldn't find team with that ID");
            }

            if (team.InviteTokenSecret != requestTeamSecret)
            {
                return BadRequest("Bad secret");
            }

            team.TeamUsers.Add(new TeamUser()
            {
                User = user,
                UserId = userId,
                Team = team,
                TeamId = teamId
            });

            await _db.SaveChangesAsync();

            return Ok("User was successfully added");
        }
        
        
        [Authorize]
        [HttpDelete("kick/{requestSecret}")]
        public async Task<IActionResult> KickUserFromTheTeam()
        {
            throw new NotImplementedException();
        }
    }
}