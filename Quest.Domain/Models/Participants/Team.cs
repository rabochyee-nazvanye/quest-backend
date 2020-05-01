using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using Quest.Domain.Interfaces;

namespace Quest.Domain.Models
{
    public class Team : Participant
    {
        public ITeamQuest GetQuest() => (ITeamQuest) Quest;
        public string InviteTokenSecret { get; set; }
        public bool ValidateSecret(string secret) => InviteTokenSecret == secret;
        public List<TeamUser> Members { get; set; }
    }
}
