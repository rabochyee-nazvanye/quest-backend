using System;
using System.Collections.Generic;
using System.Linq;
using Quest.Domain.Interfaces;

namespace Quest.Domain.Models
{
    public class TeamScheduledQuest: QuestEntity, ITeamQuest, IScheduledQuest
    {
        public DateTime RegistrationDeadline { get; set; }
        public bool IsRegistrationLimited { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int MaxTeamSize { get; set; }
        public List<Team> GetTeams() => Participants.Select(x => x as Team).ToList();
        public bool ResultsAvailable { get; set; }
        public QuestStatus GetQuestStatus()
        {
            
            if (ResultsAvailable)
                return QuestStatus.ResultsAvailable;
            
            var timeNow = DateTime.Now;
            
            if (EndDate < timeNow)
                return QuestStatus.Finished;
            
            if (StartDate < timeNow && timeNow < EndDate)
                return QuestStatus.InProgress;
            
            if (RegistrationDeadline < timeNow && IsRegistrationLimited)
                return QuestStatus.RegistrationOver;
            
            return QuestStatus.Scheduled;
        }
        public TimeSpan TimeToComplete { get; set; }


        public enum QuestStatus
        {
            Scheduled = 0,
            RegistrationOver = 1,
            InProgress = 2,
            Finished = 3,
            ResultsAvailable = 4
        }

        public override bool IsReadyToShowResults() => GetQuestStatus() == QuestStatus.ResultsAvailable;

        public override bool IsReadyToReceiveTaskAttempts()
        {
            return GetQuestStatus() == QuestStatus.InProgress;
        }

        public override bool RegistrationIsAvailable() => EndDate > DateTime.Now;

        public override Participant FindParticipant(string userId)
        {
            return GetTeams().FirstOrDefault(x => x.Members.Any(m => m.UserId == userId));
        }
    }
}