using System.Collections.Generic;
using System.Linq;
using Quest.Domain.Models;

namespace Quest.Application.DTOs
{
    public class QuestParticipantProgressAndTasksDTO
    {
        public QuestParticipantProgressAndTasksDTO(List<ParticipantProgressDTO> participantProgress, ILookup<string, TaskEntity> tasksByGroupName)
        {
            ParticipantProgress = participantProgress;
            TasksByGroupName = tasksByGroupName;
        }
     public List<ParticipantProgressDTO> ParticipantProgress { get; set; }
     public ILookup<string, TaskEntity> TasksByGroupName { get; set; }
    }
}