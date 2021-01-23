using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Quest.DAL.Data;
using Quest.Domain.Models;

namespace Quest.Application.Quests.Queries
{
    public class GetQuestByIdQueryHandler : IRequestHandler<GetQuestByIdQuery, QuestEntity>
    {
        private readonly Db _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public GetQuestByIdQueryHandler(Db context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<QuestEntity> Handle(GetQuestByIdQuery request, CancellationToken cancellationToken)
        {
            var quest = await _context.Quests
                .Where(x => x.Id == request.QuestId)
                .Include(x => x.Author)
                .Include(x => x.Participants)
                .ThenInclude(x => (x as Team).Members)
                .ThenInclude(x => x.User)
                .FirstOrDefaultAsync(cancellationToken: cancellationToken);

            if (quest == null)
                return null;

            if (!quest.IsHidden)
                return quest;
            
            var requestUser =
                await _context.Users.FirstOrDefaultAsync(x => x.Id == request.UserId, cancellationToken);
            var userIsAdmin = requestUser != null && await _userManager.IsInRoleAsync(requestUser, "Admin");
            return userIsAdmin ? quest : null;
        }
    }
}
