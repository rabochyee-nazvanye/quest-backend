using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Quest.Domain.Models
{
    public class Team
    {
        //ef core needs this
        public Team() { }

        public Team(string name, string captainUserId, int questId, string inviteTokenSecret)
        {
            Name = name;
            CaptainUserId = captainUserId;
            QuestId = questId;
            InviteTokenSecret = inviteTokenSecret;
        }

        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        public string CaptainUserId { get; set; }
        [ForeignKey("CaptainUserId")]
        public ApplicationUser Captain { get; set; }
        

        public int QuestId { get; set; }
        [ForeignKey("QuestId")]
        public QuestEntity Quest { get; set; }
        
        private string InviteTokenSecret { get; set; }
        public bool ValidateSecret(string secret) => InviteTokenSecret == secret;

        public ICollection<TeamUser> Members { get; set; }
        public ICollection<TaskAttemptTeam> TaskAttempts { get; set; }
        public ICollection<TeamHint> UsedHints { get; set; }
    }
}
