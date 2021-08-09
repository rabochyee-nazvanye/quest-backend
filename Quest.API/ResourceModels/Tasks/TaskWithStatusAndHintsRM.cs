using System.Collections.Generic;
using System.Linq;
using Quest.API.ResourceModels.Hints;
using Quest.Application.DTOs;
using Quest.Domain.Enums;

namespace Quest.API.ResourceModels.Tasks
{
    public class TaskWithStatusAndHintsRM
    {
        public TaskWithStatusAndHintsRM(TaskAndHintsDTO dto)
        {
            Id = dto.Task.Id;
            Name = dto.Task.Name;
            Reward = dto.Task.Reward;
            ManualVerificationEnabled = dto.Task.VerificationType == VerificationType.Manual;
            Question = dto.Task.Question;
            Group = dto.Task.Group;
            HintsCount = dto.Task.Hints.Count;
            
            var taskStatus =  dto.TaskAttempt?.Status ?? TaskAttemptStatus.NotSubmitted;
            Status = taskStatus.ToString().ToLowerInvariant();

            AdminComment = dto.TaskAttempt?.AdminComment;
            LastSubmittedAnswer = dto.TaskAttempt?.Text;
            UsedHints = dto.UsedHints.Select(x => new HintRM(x)).ToList();

            VideoUrl = dto.Task.VideoUrl;
        }
        
        public int Id { get; set; }
        public string Name { get; set; }
        public int Reward { get; set; }
        public bool ManualVerificationEnabled { get; set; }
        public string Question { get; set; }
        public string Group { get; set; }
        public int HintsCount { get; set; }
        public List<HintRM> UsedHints { get; set; }
        public string LastSubmittedAnswer { get; set; }
        public string Status { get; set; }
        public string AdminComment { get; set; }
        public string VideoUrl { get; set; }
    }
}