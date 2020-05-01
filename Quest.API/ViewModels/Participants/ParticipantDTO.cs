using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Quest.Domain.Models;

namespace Quest.API.ViewModels.Teams
{
    public class ParticipantDTO
    {
        public ParticipantDTO(Participant row)
        {
            Id = row.Id;
            Name = row.Name;
        }
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
