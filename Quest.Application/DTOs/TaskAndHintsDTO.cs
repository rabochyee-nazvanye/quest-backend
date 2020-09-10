using System.Collections.Generic;
using System.Threading.Tasks;
using Quest.Domain.Models;

namespace Quest.Application.DTOs
{
    public class TaskAndHintsDTO
    {
        public TaskAndHintsDTO(TaskEntity task, TaskAttempt taskAttempt, List<Hint> usedHints)
        {
            Task = task;
            TaskAttempt = taskAttempt;
            UsedHints = usedHints;
        }
        public TaskEntity Task { get; set; }
        public TaskAttempt TaskAttempt { get; set; }
        public List<Hint> UsedHints { get; set; }
    }
}