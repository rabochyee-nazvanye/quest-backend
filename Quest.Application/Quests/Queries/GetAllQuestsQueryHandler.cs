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
    public class GetAllQuestsQueryHandler : IRequestHandler<GetAllQuestsQuery, List<QuestEntity>>
    {
        private readonly Db _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public GetAllQuestsQueryHandler(Db context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<List<QuestEntity>> Handle(GetAllQuestsQuery request, CancellationToken cancellationToken)
        {
            var allQuests = await _context.Quests
                .Include(x => x.Author)
                .Include(x => x.Participants)
                .ThenInclude(x => (x as Team).Members)
                .ThenInclude(x => x.User)
                .ToListAsync(cancellationToken: cancellationToken);

            var requestUser = await _context.Users.FirstOrDefaultAsync(x => x.Id == request.UserId, cancellationToken);
            var userIsAdmin = requestUser != null && await _userManager.IsInRoleAsync(requestUser, "Admin");

            if (!userIsAdmin)
            {
                allQuests = allQuests.Where(q => !q.IsHidden).ToList();
            }

            return allQuests;
        }
    }
}
