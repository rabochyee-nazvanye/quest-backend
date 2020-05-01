using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Quest.DAL.Data;

namespace Quest.Application.Teams.Commands
{
    public class RemoveParticipantCommandHandler : IRequestHandler<RemoveParticipantCommand, BaseResponse<bool>>
    {
        private readonly Db _context;

        public RemoveParticipantCommandHandler(Db context)
        {
            _context = context;
        }

        public async Task<BaseResponse<bool>> Handle(RemoveParticipantCommand request, CancellationToken cancellationToken)
        {
            var participant = await _context.Participants
                .FirstOrDefaultAsync(x => x.Id == request.ParticipantId,
                cancellationToken: cancellationToken);
            if (participant == null)
                return BaseResponse.Failure<bool>("Participant not found.");

            var user = await _context.Users.FindAsync(request.UserId);

            if (participant.PrincipalUserId != request.UserId)
                return BaseResponse.Failure<bool>("Only participant principal can remove the team.");

            if (user == null)
                return BaseResponse.Failure<bool>("User not found.");

            _context.Participants.Remove(participant);
            await _context.SaveChangesAsync(cancellationToken);

            return BaseResponse.Success(true, "Successfully removed participant");
        }
    }
}
