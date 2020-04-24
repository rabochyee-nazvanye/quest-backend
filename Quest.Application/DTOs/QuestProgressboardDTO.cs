using System.Collections.Generic;
using System.Linq;
using Quest.Domain.Models;

namespace Quest.Application.DTOs
{
    public class QuestProgressboardDTO
    {
        public QuestProgressboardDTO(List<TeamProgressDTO> teamProgresses, ILookup<string, TaskEntity> tasksByGroupName)
        {
            TeamProgresses = teamProgresses;
            TasksByGroupName = tasksByGroupName;
        }
     public List<TeamProgressDTO> TeamProgresses { get; set; }
     public ILookup<string, TaskEntity> TasksByGroupName { get; set; }
    }
}