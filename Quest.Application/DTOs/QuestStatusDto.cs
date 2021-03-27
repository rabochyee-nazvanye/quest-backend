using System;
using Quest.Domain.Models;

namespace Quest.Application.DTOs
{
    public class QuestStatusDto
    {
        public QuestStatusDto(Team row)
        {
            TasksRead = row.TasksOpenTime != DateTime.MinValue;
            Deadline = row.GetDeadline();
        }
        public bool TasksRead { get; set; }
        public DateTime Deadline { get; set; }
    }
}