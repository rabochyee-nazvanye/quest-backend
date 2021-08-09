using System;
using Quest.Application.DTOs;
using Quest.Domain.Models;

namespace Quest.API.ResourceModels.Quests
{
    public class QuestStatusRM
    {
        public QuestStatusRM(QuestStatusDto row)
        {
            TasksRead = row.TasksRead;
            Deadline = row.Deadline;
        }
        public bool TasksRead { get; set; }
        public DateTime Deadline { get; set; }
    }
}