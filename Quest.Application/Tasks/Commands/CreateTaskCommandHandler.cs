using System;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Quest.DAL.Data;
using Quest.Domain.Enums;
using Quest.Domain.Models;

namespace Quest.Application.Tasks.Commands
{
    public class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommand, BaseResponse<TaskEntity>>
    {
        private readonly Db _context;

        public CreateTaskCommandHandler(Db context)
        {
            _context = context;
        }

        public async Task<BaseResponse<TaskEntity>> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
        {
            var questExists = await _context.Quests.AnyAsync(x => x.Id == request.QuestId, cancellationToken: cancellationToken);
            if (!questExists)
                return BaseResponse.Failure<TaskEntity>("Quest was not found");

            var task = new TaskEntity
            {
                CorrectAnswer = request.CorrectAnswer,
                Group = request.Group,
                Name = request.Name,
                QuestId = request.QuestId,
                Question = request.Question,
                Reward = request.Reward,
                VerificationType = request.VerificationIsManual ? VerificationType.Manual : VerificationType.Automatic
            };

            await _context.Tasks.AddAsync(task, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return BaseResponse.Success(task, "Success");
        }
    }
}