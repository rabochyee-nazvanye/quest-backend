using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Identity;

namespace Quest.Domain.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string AvatarUrl { get; set; }
        public List<QuestEntity> CreatedQuests { get; set; }

        public List<TeamUser> JoinedTeams { get; set; }
        
        public List<Participant> OwnedParticipants { get; set; }

        public List<Participant> ModeratedParticipants { get; set; }
        public int TelegramId { get; set; }
    }
}
