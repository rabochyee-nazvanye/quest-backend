using Quest.Domain.Models;

namespace Quest.Application.Services
{
    public interface IQuestConstructorArgs
    {
        BaseResponse<bool> Validate();
    }
}