namespace Quest.Domain.Models
{
    public class Moderator
    {
        public int QuestId { get; set; }
        public QuestEntity Quest { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}