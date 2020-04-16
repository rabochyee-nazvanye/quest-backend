using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Quest.DAL.Data;
using Quest.Domain.Models;

namespace Quest.Application.Quests.Commands
{
    public class CreateQuestCommandHandler : IRequestHandler<CreateQuestCommand, BaseResponse<QuestEntity>>
    {
        private readonly Db _context;
        private const int DefaultTeamSize = 100;

        public CreateQuestCommandHandler(Db context)
        {
            _context = context;
        }

        public async Task<BaseResponse<QuestEntity>> Handle(CreateQuestCommand request, CancellationToken cancellationToken)
        {
            var author = await _context.Users.FirstOrDefaultAsync(x =>
                x.Id == request.AuthorId, cancellationToken: cancellationToken);
            if (author == null)
                return BaseResponse.Failure<QuestEntity>("User not found.");
            
            if (request.StartDate < DateTime.Now || request.EndDate < DateTime.Now ||
                request.RegistrationDeadline < DateTime.Now)
                return BaseResponse.Failure<QuestEntity>("Quest dates can't be in the past.");

            if (request.StartDate > request.EndDate)
                return BaseResponse.Failure<QuestEntity>("Quest start should be sooner than end.");
            
            if (request.RegistrationDeadline > request.StartDate)
                return BaseResponse.Failure<QuestEntity>("Quest register deadline should be sooner than start.");

            if (request.MaxTeamSize < 0)
                return BaseResponse.Failure<QuestEntity>("Max team size should be non-negative integer.");

            var quest = new QuestEntity
            {
                Name = request.Name,
                Description = request.Description,
                ImageUrl = request.ImageUrl,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                RegistrationDeadline = request.RegistrationDeadline,
                AuthorId = author.Id,
                MaxTeamSize = request.MaxTeamSize != 0 ? request.MaxTeamSize : DefaultTeamSize
            };

            await _context.Quests.AddAsync(quest, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return BaseResponse.Success(quest);
        }
    }
}
