namespace Quest.Domain.Models
{
    public class CompletedTask
    {
        public int TeamId { get; set; }
        public Team Team { get; set; }
        public int TaskId { get; set; }
        public Task Task { get; set; }
    }
}