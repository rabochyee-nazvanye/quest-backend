using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Quest.DAL.Data;
using Quest.Domain.Interfaces;
using Quest.Domain.Models;

namespace Quest.Application.Tasks.Commands
{
    public class SubmitHintRequestCommandHandler : IRequestHandler<SubmitHintRequestCommand, BaseResponse<Hint>>
    {
        private readonly Db _context;

        public SubmitHintRequestCommandHandler(Db context)
        {
            _context = context;
        }

        public async Task<BaseResponse<Hint>> Handle(SubmitHintRequestCommand request, CancellationToken cancellationToken)
        {
            if (request.HintNumber < 0)
                return BaseResponse.Failure<Hint>("Hint number should be non-negative value");
            
            var userExists = await _context.Users.AnyAsync(x => x.Id == request.UserId,
                cancellationToken: cancellationToken);

            if (!userExists)
                return BaseResponse.Failure<Hint>("Internal: user not found");
            
            var task = await _context.Tasks.Where(x => x.Id == request.TaskId)
                .Include(x => x.TaskAttempts)
                .Include(x => x.Quest)
                    .ThenInclude(x =>x.Participants)
                        .ThenInclude(x => (x as Team).Members)
                            .ThenInclude(x => x.User)
                .Include(x => x.Quest)
                    .ThenInclude(x => x.Participants)
                        .ThenInclude(x => x.UsedHints)
                .Include(x => x.Hints)
                .FirstOrDefaultAsync(cancellationToken);

            if (task == null)
                return BaseResponse.Failure<Hint>("Task was not found");

            var participant = task.Quest.FindParticipant(request.UserId);
            
            if (participant == null)
                return BaseResponse.Failure<Hint>("Team was not found");

            if (!task.Quest.IsReadyToReceiveTaskAttempts())
                return BaseResponse.Failure<Hint>("Quest is not in active state yet.");

            if (!task.Hints.Any() || request.HintNumber >= task.Hints.Count)
                return BaseResponse.Failure<Hint>("Task doesn't have hint with provided number.");

            var orderedHints = task.Hints.OrderBy(x => x.Sorting).ToList();
            var requestedHint = orderedHints[request.HintNumber];

            if (participant.UsedHints.Any(x => x.HintId == requestedHint.Id))
                return BaseResponse.Success(requestedHint, "Success");

            participant.UsedHints.Add(new ParticipantHint
            {
                ParticipantId = participant.Id,
                HintId = requestedHint.Id
            });
            
            await _context.SaveChangesAsync(cancellationToken);
            return BaseResponse.Success(requestedHint, "Success");
        }
    }
}