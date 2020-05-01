using System;
using Quest.Domain.Interfaces;
using Quest.Domain.Models;

namespace Quest.API.ViewModels.Quests
{
    public class TeamScheduledQuestDTO : QuestDTO
    {
        public TeamScheduledQuestDTO(IScheduledQuest row) : base(row)
        {
            RegistrationDeadline = row.RegistrationDeadline;
            StartDate = row.StartDate;
            EndDate = row.EndDate;
            Status = row.GetQuestStatus().ToString().ToLowerInvariant();
            IsInfinite = false;
        }
        public DateTime RegistrationDeadline { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; }
        public bool IsInfinite { get; set; }
    }
}