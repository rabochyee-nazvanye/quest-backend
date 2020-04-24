using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore.Design;
using Quest.API.ViewModels.Hints;
using Quest.Application.DTOs;
using Quest.Domain.Enums;
using Quest.Domain.Models;

namespace Quest.API.ViewModels.Tasks
{
    public class TaskVM
    {
        public TaskVM(TeamTaskStatusDTO dto)
        {
            Id = dto.Task.Id;
            Name = dto.Task.Name;
            Reward = dto.Task.Reward;
            ManualVerificationEnabled = dto.Task.VerificationType == VerificationType.Manual;
            Question = dto.Task.Question;
            Group = dto.Task.Group;
            HintsCount = dto.Task.Hints.Count;
            Status = dto
                .Task.TaskAttempts.Any(x => x.Status == TaskAttemptStatus.Accepted)
                ? TaskAttemptStatus.Accepted.ToString().ToLowerInvariant()
                : dto.Task.TaskAttempts.Any(x => x.Status == TaskAttemptStatus.OnReview)
                    ? TaskAttemptStatus.OnReview.ToString().ToLowerInvariant()
                    : dto.Task.TaskAttempts.Any(x => x.Status == TaskAttemptStatus.Error)
                        ? TaskAttemptStatus.Error.ToString().ToLowerInvariant()
                        : "notsubmitted";

            var lastAttempt = dto.Task.TaskAttempts.OrderByDescending(x => x.SubmitTime).FirstOrDefault();
            AdminComment = lastAttempt?.AdminComment;
            LastSubmittedAnswer = lastAttempt?.Text;
            
            UsedHints = dto.UsedHints.Select(x => new HintVM(x)).ToList();
        }
        
        public int Id { get; set; }
        public string Name { get; set; }
        public int Reward { get; set; }
        public bool ManualVerificationEnabled { get; set; }
        public string Question { get; set; }
        public string Group { get; set; }
        public int HintsCount { get; set; }
        public List<HintVM> UsedHints { get; set; }
        public string LastSubmittedAnswer { get; set; }
        public string Status { get; set; }
        public string AdminComment { get; set; }
    }
}