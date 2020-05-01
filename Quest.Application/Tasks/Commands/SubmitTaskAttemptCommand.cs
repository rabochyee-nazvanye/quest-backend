using MediatR;
using Quest.Application.DTOs;
using Quest.Domain.Models;

namespace Quest.Application.Tasks.Commands
{
    public class SubmitTaskAttemptCommand : IRequest<BaseResponse<TaskAndHintsDTO>>
    {
        public string AttemptText { get; set; }
        public string UserId { get; set; }
        public int TaskId { get; set; }
    }
}