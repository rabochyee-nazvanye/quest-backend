using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Quest.Application.Teams.Commands
{
    public class RemoveUserFromTeamCommandHandler : IRequestHandler<RemoveUserFromTeamCommand, BaseResponse<bool>>
    {
        public async Task<BaseResponse<bool>> Handle(RemoveUserFromTeamCommand request, CancellationToken cancellationToken)
        {
            //todo implement user kick logic
            throw new NotImplementedException();
        }
    }
}
