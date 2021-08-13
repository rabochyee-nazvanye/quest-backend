using System;
using System.Collections.Generic;
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
            var quest = await _context.Quests.FirstOrDefaultAsync(x => x.Id == request.QuestId, cancellationToken: cancellationToken);
            if (quest == null)
                return BaseResponse.Failure<TaskEntity>("Quest was not found");

            if (quest.AuthorId != request.UserId)
                return BaseResponse.Failure<TaskEntity>("Only quest authors can add new tasks");
            
            var task = new TaskEntity
            {
                CorrectAnswer = request.CorrectAnswer,
                Group = request.Group,
                Name = request.Name,
                QuestId = request.QuestId,
                Question = request.Question,
                Reward = request.Reward,
                VerificationType = request.VerificationIsManual ? VerificationType.Manual : VerificationType.Automatic,
                Hints = new List<Hint>(),
                VideoUrl = request.VideoUrl
            };

            if (request.Hints != null && request.Hints.Any())
            {
                for (var idx = 0; idx < request.Hints.Count; idx++)
                {
                    var hintText = request.Hints[idx];
                    task.Hints.Add(
                        new Hint
                        {
                            Secret = hintText,
                            Sorting = idx,
                            TaskId = task.Id
                        });
                }
            }

            await _context.Tasks.AddAsync(task, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return BaseResponse.Success(task, "Success");
        }
    }
}