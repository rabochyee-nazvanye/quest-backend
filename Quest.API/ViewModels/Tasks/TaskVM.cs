using System.Linq;
using Quest.Domain.Enums;
using Quest.Domain.Models;

namespace Quest.API.ViewModels.Tasks
{
    public class TaskVM
    {
        public TaskVM(TaskEntity row)
        {
            Id = row.Id;
            Name = row.Name;
            Reward = row.Reward;
            ManualVerificationEnabled = (row.VerificationType == VerificationType.Manual).ToString();
            Question = row.Question;
            Group = row.Group;
            HintsCount = row.Hints.Count;
            Status = row
                .TaskAttempts.Any(x => x.Status == TaskAttemptStatus.Accepted)
                ? TaskAttemptStatus.Accepted.ToString().ToLowerInvariant()
                : row.TaskAttempts.Any(x => x.Status == TaskAttemptStatus.OnReview)
                    ? TaskAttemptStatus.OnReview.ToString().ToLowerInvariant()
                    : row.TaskAttempts.Any(x => x.Status == TaskAttemptStatus.Error)
                        ? TaskAttemptStatus.Error.ToString().ToLowerInvariant()
                        : "notsubmitted";
        }
        
        public int Id { get; set; }
        public string Name { get; set; }
        public int Reward { get; set; }
        public string ManualVerificationEnabled { get; set; }
        public string Question { get; set; }
        public string Group { get; set; }
        public int HintsCount { get; set; }
        public string Status { get; set; }
    }
}