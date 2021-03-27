using System;
using System.Collections.Generic;
using Quest.Domain.Models;

namespace Quest.Domain.Interfaces
{
    public interface IScheduledQuest : IQuest
    {
        public DateTime RegistrationDeadline { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool ResultsAvailable { get; set; }
        public TeamScheduledQuest.QuestStatus GetQuestStatus();
        public TimeSpan TimeToComplete { get; set; }
    }
}