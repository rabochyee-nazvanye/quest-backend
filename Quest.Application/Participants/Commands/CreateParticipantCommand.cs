using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using MediatR;
using Quest.Application.Services;
using Quest.Domain.Models;

namespace Quest.Application.Teams.Commands
{
    public class CreateParticipantCommand : IRequest<BaseResponse<Participant>>
    {
        public CreateParticipantCommand(IParticipantConstructorArgs participantConstructorArgs)
        {
            ParticipantConstructorArgs = participantConstructorArgs;
        }

        public IParticipantConstructorArgs ParticipantConstructorArgs { get; set; }
    }
}
