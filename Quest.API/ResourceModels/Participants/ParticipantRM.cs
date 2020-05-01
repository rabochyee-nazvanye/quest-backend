using Quest.Domain.Models;

namespace Quest.API.ResourceModels.Participants
{
    public class ParticipantRM
    {
        public ParticipantRM(Participant row)
        {
            Id = row.Id;
            Name = row.Name;
        }
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
