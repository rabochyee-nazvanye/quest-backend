using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using MediatR;
using Quest.Domain.Models;

namespace Quest.Application.Teams.Commands
{
    public class CreateTeamCommand : IRequest<BaseResponse<Team>>
    {
        public CreateTeamCommand(string name, string userId, int questId)
        {
            Name = name;
            UserId = userId;
            QuestId = questId;
        }

        public string UserId { get; set; }
        public string Name { get; set; }
        public int QuestId { get; set; }
    }
}
