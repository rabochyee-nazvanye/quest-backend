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

        public ICollection<TeamUser> TeamUsers { get; set; }
        public ICollection<Moderator> Moderators { get; set; }
    }
}
