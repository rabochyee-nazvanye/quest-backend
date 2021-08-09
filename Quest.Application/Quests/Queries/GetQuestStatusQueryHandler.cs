using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Quest.Application.DTOs;
using Quest.Application.Services;
using Quest.DAL.Data;
using Quest.Domain.Enums;
using Quest.Domain.Models;

namespace Quest.Application.Quests.Queries
{
    public class GetQuestStatusQueryHandler : IRequestHandler<GetQuestStatusQuery, BaseResponse<QuestStatusDto>>
    {
        private readonly Db _context;

        public GetQuestStatusQueryHandler(Db context)
        {
            _context = context;
        }

        public async Task<BaseResponse<QuestStatusDto>> Handle(GetQuestStatusQuery request, CancellationToken cancellationToken)
        {
            var quest = await _context.Quests
                .Include(x => x.Participants)
                .ThenInclude(x => (x as Team).Members)
                .ThenInclude(x => x.User)
                .FirstOrDefaultAsync(x => x.Id == request.QuestId,
                    cancellationToken: cancellationToken);

            if (quest == null)
                return BaseResponse.Failure<QuestStatusDto>("Could not find quest with provided id.");

            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == request.UserId,
                cancellationToken: cancellationToken);

            if (user == null)
                return BaseResponse.Failure<QuestStatusDto>("Internal: request user does not exist.");

            var participant = quest.FindParticipant(request.UserId);
            
            if (participant == null)
                return BaseResponse.Failure<QuestStatusDto>("Could not find participant of this user.");

            return participant switch // temporary
            {
                Team team => new BaseResponse<QuestStatusDto>(new QuestStatusDto(team), "Success"),
                _ => BaseResponse.Failure<QuestStatusDto>("Status is available for teams only.")
            };
        }
    }
}