using System;
using System.Collections.Generic;
using System.Text;

namespace Quest.Domain.Models
{
    public class TeamUser
    {
        public int TeamId { get; set; }
        public Team Team { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
