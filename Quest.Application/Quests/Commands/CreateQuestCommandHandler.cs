using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Quest.Application.Services;
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
            var validationResult = request.QuestConstructorArgs.Validate();

            if (!validationResult.Result)
                return BaseResponse.Failure<QuestEntity>(validationResult.Message);

            var quest = QuestFactory.Create(request.QuestConstructorArgs);

            await _context.Quests.AddAsync(quest, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return BaseResponse.Success(quest);
        }
    }
}
