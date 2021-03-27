using System;
using Quest.Domain.Enums;
using Quest.Domain.Models;

namespace Quest.Application.Services
{
    public static class QuestFactory
    {
        public static QuestEntity Create(IQuestConstructorArgs type)
        {
            switch (type)
            {
                case TeamScheduledConstructorArgs args:
                    return new TeamScheduledQuest
                    {
                        Name = args.Name,
                        Description = args.Description,
                        ImageUrl = args.ImageUrl,
                        StartDate = args.StartDate,
                        EndDate = args.EndDate,
                        RegistrationDeadline = args.RegistrationDeadline,
                        AuthorId = args.AuthorId,
                        MaxTeamSize = args.MaxTeamSize,
                        TimeToComplete = args.TimeToComplete
                    };
                case SoloInfiniteConstructorArgs args:
                    return new SoloInfiniteQuest
                    {
                        Name = args.Name,
                        Description = args.Description,
                        ImageUrl = args.ImageUrl,
                        AuthorId = args.AuthorId
                    };
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, $"{type} is not supported");
            }
        }
    }
}