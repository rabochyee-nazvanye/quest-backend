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

            var quest = new QuestEntity
            {
                Name = request.Name,
                Description = request.Description,
                ImageUrl = request.ImageUrl,
                StartDate = DateTime.Now + TimeSpan.FromDays(7),
                RegistrationDeadline = DateTime.Now + TimeSpan.FromDays(3),
                AuthorId = author.Id
            };

            await _context.Quests.AddAsync(quest, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return BaseResponse.Success(quest);
        }
    }
}
