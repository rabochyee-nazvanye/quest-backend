using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Quest.DAL.Data;
using Quest.Domain.Interfaces;
using Quest.Domain.Models;

namespace Quest.Application.Teams.Queries
{
    public class GetParticipantsByUserAndQuestQueryHandler : IRequestHandler<GetParticipantsByUserAndQuestQuery, BaseResponse<List<Participant>>>
    {
        private readonly Db _context;

        public GetParticipantsByUserAndQuestQueryHandler(Db context)
        {
            _context = context;
        }

        public async Task<BaseResponse<List<Participant>>> Handle(GetParticipantsByUserAndQuestQuery request, CancellationToken cancellationToken)
        {
            if (!request.MemberIds.Any())
                return null;
            
            var quest = await _context.Quests
                .Where(x => x.Id == request.QuestId)
                .Include(x => x.Participants)
                .ThenInclude(x => (x as Team).Members)
                .ThenInclude(x => x.User)
                .Include(x => x.Participants)
                //need to cast below because of ef core 3 bug https://github.com/dotnet/efcore/issues/18248
                .ThenInclude(x => (x as Team).Principal)
                .FirstOrDefaultAsync(cancellationToken);

            if (quest == null)
                return BaseResponse.Failure<List<Participant>>("Quest not found");
            
            var participants = request.MemberIds
                .Select(m => quest.FindParticipant(m))
                .Where(p => p!= null)
                .ToList();
            
            return BaseResponse.Success(participants, "Success");
        }
    }
}