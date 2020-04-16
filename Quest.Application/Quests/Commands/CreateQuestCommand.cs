using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using Quest.Domain.Models;

namespace Quest.Application.Quests.Commands
{
    public class CreateQuestCommand : IRequest<BaseResponse<QuestEntity>>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }

        public DateTime RegistrationDeadline { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string AuthorId { get; set; }
        public int MaxTeamSize { get; set; }
    }
}
