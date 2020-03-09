using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Quest.DAL.Data;
using Quest.Domain.Models;


namespace Quest.API.Controllers
{
    public class QuestController : Controller
    {
        private readonly Db _db;

        public QuestController(Db context)
        {
            _db = context;
        }
        
        [AllowAnonymous]
        [Route("[controller]")]
        public IActionResult ListAvailableQuests()
        {
            var data = _db.Quests.ToList();
            return Ok(data);
        }
        
        [AllowAnonymous]
        [Route("[controller]/{id}")]
        public IActionResult GetQuest(int id)
        {
            var quest = _db.Quests.FirstOrDefault((x) => x.Id == id);
            if (quest == null)
            {
                return NotFound();
            }
            return Ok(quest);
        }
        
        [HttpPost]
        [Route("[controller]/new")]
        public async Task<IActionResult> Create(QuestEntity quest)
        {
            var name = Request.Query.FirstOrDefault(x => x.Key == "Name").Value;
            var description = Request.Query.FirstOrDefault(x => x.Key == "Description").Value;
            var date = DateTime.Parse(Request.Query.FirstOrDefault(x => x.Key == "Date").Value);
            var authorId = Request.Query.FirstOrDefault(x => x.Key == "AuthorId").Value;

            // quest = new QuestEntity
            // {
            //     Name = name,
            //     Description = description,
            //     AuthorId = authorId,
            //     Date = date
            // };

            if (quest == null)
            {
                return BadRequest();
            }

            _db.Quests.Add(quest);
            await _db.SaveChangesAsync();
            return Ok("SuccessfullySaved");
        }
    }
}