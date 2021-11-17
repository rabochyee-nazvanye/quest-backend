using System;
using Quest.API.Enums;
using Quest.DAL.Migrations;
using Quest.Domain.Interfaces;

namespace Quest.API.ResourceModels.Quests
{
    public class TeamScheduledQuestRM : QuestRM
    {
        public TeamScheduledQuestRM(IScheduledQuest row) : base(row)
        {
            RegistrationDeadline = row.RegistrationDeadline;
            StartDate = row.StartDate;
            EndDate = row.EndDate;
            Status = row.GetQuestStatus().ToString().ToLowerInvariant();
            TimeToCompleteInMinutes = (int)row.TimeToComplete.TotalMinutes;
            IsRegistrationLimited = row.IsRegistrationLimited;
        }
        public DateTime RegistrationDeadline { get; set; }
        public bool IsRegistrationLimited { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; }
        protected override bool CheckIsInfinite() => false;
        protected override string GetQuestType() => QuestParticipantType.Team.ToString().ToLowerInvariant();
        public int TimeToCompleteInMinutes { get; set; }
    }
}