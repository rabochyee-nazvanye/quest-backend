using System;
using Quest.API.BindingModels.Quests;
using Quest.API.Enums;
using Quest.Application.Services;

namespace Quest.API.Services
{
    public static class CreateQuestArgsFactory
    {
        public static IQuestConstructorArgs Create(CreateQuestBM model, string userId)
        {
            if (model.IsInfinite)
                switch (model.ParticipantType)
                {
                    case QuestParticipantType.Solo:
                        return new SoloInfiniteConstructorArgs
                        {
                            AuthorId = userId,
                            Description = model.Description,
                            ImageUrl = model.ImageUrl,
                            Name = model.Name,
                        };
                    default:
                        throw new ArgumentOutOfRangeException(nameof(model.ParticipantType), model.ParticipantType, 
                            $"Could not create infinite quest with participant type {model.ParticipantType}");
                }

            switch (model.ParticipantType)
            {
                case QuestParticipantType.Team:
                    return new TeamScheduledConstructorArgs
                    {
                        AuthorId = userId,
                        Description = model.Description,
                        ImageUrl = model.ImageUrl,
                        Name = model.Name,
                        RegistrationDeadline = model.RegistrationDeadline,
                        StartDate = model.StartDate,
                        EndDate = model.EndDate,
                        MaxTeamSize = model.MaxTeamSize,
                        TimeToComplete = TimeSpan.FromMinutes(model.TimeToCompleteInMinutes),
                        IsRegistrationLimited = model.IsRegistrationLimited
                    };
                default:
                    throw new ArgumentOutOfRangeException(nameof(model.ParticipantType), model.ParticipantType,          
                        $"Could not create scheduled quest with participant type {model.ParticipantType}");
            }
        }
    }
}