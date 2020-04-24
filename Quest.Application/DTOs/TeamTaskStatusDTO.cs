using System.Collections.Generic;
using System.Threading.Tasks;
using Quest.Domain.Models;

namespace Quest.Application.DTOs
{
    public class TeamTaskStatusDTO
    {
        public TeamTaskStatusDTO(TaskEntity task, List<Hint> usedHints)
        {
            Task = task;
            UsedHints = usedHints;
        }
        public TaskEntity Task { get; set; }
        public List<Hint> UsedHints { get; set; }
    }
}