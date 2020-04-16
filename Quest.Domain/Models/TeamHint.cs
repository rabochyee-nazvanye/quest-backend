namespace Quest.Domain.Models
{
    public class TeamHint
    {
        public int HintId { get; set; }
        public Hint Hint { get; set; }
        public int TeamId { get; set; }
        public Team Team { get; set; }
    }
}