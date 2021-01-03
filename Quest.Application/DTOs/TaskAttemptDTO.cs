using Quest.Domain.Models;

namespace Quest.Application.DTOs
{
    public class TaskAttemptDTO
    {
        public TaskAttemptDTO(TaskAttempt taskAttempt)
        {
            TaskId = taskAttempt.TaskId;
            TaskName = $"{taskAttempt.TaskEntity.Group} / {taskAttempt.TaskEntity.Name}" ;
            ParticipantId = taskAttempt.ParticipantId;
            ParticipantName = taskAttempt.Participant.Name;
            Text = taskAttempt.Text;
            ModeratorId = taskAttempt.Participant.Moderator.Id;
            ModeratorTelegramId = taskAttempt.Participant.Moderator.TelegramId;
            CorrectAnswerText = taskAttempt.TaskEntity.CorrectAnswer;
        }
        public int TaskId { get; set; }
        public string TaskName { get; set; }
        
        public int ParticipantId { get; set; }
        public string ParticipantName { get; set; }

        public string Text { get; set; }
        public int ModeratorTelegramId { get; set; }
        public string ModeratorId { get; set; }
        public string CorrectAnswerText { get; set; }
    }
}