namespace Quest.Domain.Models
{
    public class UsedTeamHint
    {
        public int HintId { get; set; }
        public Hint Hint { get; set; }
        public int TeamId { get; set; }
        public Team Team { get; set; }
    }
}