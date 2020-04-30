using Quest.Domain.Models;

namespace Quest.Application.DTOs
{
    public class TaskAttemptDTO
    {
        public TaskAttemptDTO(TaskAttempt taskAttempt)
        {
            Id = taskAttempt.Id;
            TaskId = taskAttempt.TaskId;
            TaskName = $"{taskAttempt.TaskEntity.Group} / {taskAttempt.TaskEntity.Name}" ;
            TeamId = taskAttempt.TeamId;
            TeamName = taskAttempt.Team.Name;
            Text = taskAttempt.Text;
            ModeratorId = taskAttempt.Team.Moderator.Id;
            ModeratorTelegramId = taskAttempt.Team.Moderator.TelegramId;
            CorrectAnswerText = taskAttempt.TaskEntity.CorrectAnswer;
        }
        public int Id { get; set; }

        public int TaskId { get; set; }
        public string TaskName { get; set; }
        
        public int TeamId { get; set; }
        public string TeamName { get; set; }

        public string Text { get; set; }
        public int ModeratorTelegramId { get; set; }
        public string ModeratorId { get; set; }
        public string CorrectAnswerText { get; set; }
    }
}