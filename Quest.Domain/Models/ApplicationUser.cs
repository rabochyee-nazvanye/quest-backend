using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;

namespace Quest.Domain.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string AvatarUrl { get; set; }
        public List<QuestEntity> Quests { get; set; }

        public List<TeamUser> TeamUsers { get; set; }
        public List<AppUserQuest> AppUserQuests { get; set; }
    }
}
