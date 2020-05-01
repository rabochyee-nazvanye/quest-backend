using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Quest.DAL.Data;

namespace Quest.Application.Participants.Commands
{
    public class AssignModeratorToParticipantCommandHandler : IRequestHandler<AssignModeratorToParticipantCommand, BaseResponse<bool>>
    {
        private readonly Db _context;

        public AssignModeratorToParticipantCommandHandler(Db context)
        {
            _context = context;
        }

        public async Task<BaseResponse<bool>> Handle(AssignModeratorToParticipantCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == request.UserId,
                cancellationToken: cancellationToken);

            if (user == null)
                return BaseResponse.Failure<bool>("User does not exist");

            var participant = await _context.Participants
                .Where(x => x.Id == request.ParticipantId)
                .Include(x => x.Moderator)
                .FirstOrDefaultAsync(cancellationToken: cancellationToken);

            if (participant == null)
                return BaseResponse.Failure<bool>("Specified participant does not exist.");

            var moderator = await _context.Users
                .FirstOrDefaultAsync(x => x.Id == request.ModeratorId,
                cancellationToken: cancellationToken);

            if (moderator == null)
                return BaseResponse.Failure<bool>("Specified moderator does not exist.");

            participant.Moderator = moderator;

            await _context.SaveChangesAsync(cancellationToken);

            return BaseResponse.Success(true, "Successfully assigned moderator to participant");
        }
    }
}
