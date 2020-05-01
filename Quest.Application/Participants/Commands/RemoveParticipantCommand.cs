using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using Quest.Domain.Models;

namespace Quest.Application.Teams.Commands
{
    public class RemoveParticipantCommand : IRequest<BaseResponse<bool>>
    {
        public RemoveParticipantCommand(string userId, int participantId)
        {
            UserId = userId;
            ParticipantId = participantId;
        }

        public string UserId { get; set; }
        public int ParticipantId { get; set; }
    }
}
