﻿using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using Quest.Application.Services;
using Quest.Domain.Models;

namespace Quest.Application.Quests.Commands
{
    public class CreateQuestCommand : IRequest<BaseResponse<QuestEntity>>
    {
        public CreateQuestCommand(IQuestConstructorArgs constructorArgs)
        {
            QuestConstructorArgs = constructorArgs;
        }
        public IQuestConstructorArgs QuestConstructorArgs { get; set; }
    }
}
