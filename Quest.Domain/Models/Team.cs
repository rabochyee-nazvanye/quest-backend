using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Quest.Domain.Models
{
    public class Team
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<TeamUser> TeamUsers { get; set; }
    }
}
