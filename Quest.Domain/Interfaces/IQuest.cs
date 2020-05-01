using System.Collections.Generic;
using Quest.Domain.Models;

namespace Quest.Domain.Interfaces
{
    public interface IQuest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public string AuthorId { get; set; }
        public ApplicationUser Author { get; set; }
        public List<Participant> Participants { get; set; }
    }
}