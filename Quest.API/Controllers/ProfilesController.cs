using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Quest.API.Models.ViewModels.Profiles;
using Quest.DAL.Data;


namespace Quest.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProfilesController : Controller
    {
        private readonly Db _db;

        public ProfilesController(Db context)
        {
            _db = context;
        }
        
        
        // TODO auth by name -> auth by ID, for some reason .GetUserId(User) returns null /shrug
        [HttpGet]
        [Authorize]
        public IActionResult GetLoggedInUser()
        {
            var userName= User.Identity.Name;
            return GetUserByName(userName);
        }
        

        [HttpGet("{name}")]
        [AllowAnonymous]
        public IActionResult GetUserByName(string name)
        {
            var user = _db.Users.FirstOrDefault(x => x.UserName == name);
            if (user == null)
            {
                return BadRequest("User with that username not found.");
            }

            return Json(new ProfileVM(user));
        }
    }
}