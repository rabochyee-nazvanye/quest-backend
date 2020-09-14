using System;
using Quest.Domain.Models;

namespace Quest.Application.Services
{
    public class TeamScheduledConstructorArgs : IQuestConstructorArgs
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime RegistrationDeadline { get; set; }
        public string AuthorId { get; set; }
        public int MaxTeamSize { get; set; }

        public BaseResponse<bool> Validate()
        {
            if (string.IsNullOrWhiteSpace(Name)
                || string.IsNullOrWhiteSpace(Description)
                || string.IsNullOrWhiteSpace(ImageUrl)
                || string.IsNullOrWhiteSpace(AuthorId))
                return BaseResponse.Failure<bool>("Arguments are invalid.");

            if (StartDate < DateTime.Now || EndDate < DateTime.Now ||
                RegistrationDeadline < DateTime.Now)
                return BaseResponse.Failure<bool>("Quest dates can't be in the past.");

            if (StartDate > EndDate)
                return BaseResponse.Failure<bool>("Quest start should be sooner than end.");

            if (RegistrationDeadline > StartDate)
                return BaseResponse.Failure<bool>("Quest register deadline should be sooner than start.");

            if (MaxTeamSize < 0)
                return BaseResponse.Failure<bool>("Max team size should be non-negative integer.");

            return BaseResponse.Success(true, "Arguments are valid");
        }
    }
}