using MediatR;
using Quest.Domain.Models;

namespace Quest.Application.Tasks.Commands
{
    public class SubmitHintRequestCommand : IRequest<BaseResponse<Hint>>
    {
        public string UserId { get; set; }
        public int TaskId { get; set; }
        public int HintNumber { get; set; }
    }
}