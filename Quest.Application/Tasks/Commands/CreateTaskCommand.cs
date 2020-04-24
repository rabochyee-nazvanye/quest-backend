using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using Quest.Domain.Enums;
using Quest.Domain.Models;

namespace Quest.Application.Tasks.Commands
{
    public class CreateTaskCommand : IRequest<BaseResponse<TaskEntity>>
    {
        public string Name { get; set; }
        public int Reward { get; set; }
        public bool VerificationIsManual  { get; set; }
        public string Question { get; set; }
        public string CorrectAnswer { get; set; }
        public string Group { get; set; }
        public int QuestId { get; set; }
        public List<string> Hints { get; set; }
    }
}
