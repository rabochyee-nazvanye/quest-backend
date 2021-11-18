using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Quest.Application.Services;
using Quest.DAL.Data;
using Quest.Domain.Models;

namespace Quest.Application.Quests.Commands
{
    public class UpdateQuestInfoCommandHandler : IRequestHandler<UpdateQuestInfoCommand, BaseResponse<QuestEntity>>
    {
        private readonly Db _context;
                private const int DefaultTeamSize = 100;
        
                public UpdateQuestInfoCommandHandler(Db context)
                {
                    _context = context;
                }

                public async Task<BaseResponse<QuestEntity>> Handle(UpdateQuestInfoCommand request, CancellationToken cancellationToken)
                {
                    var validationResult = request.QuestConstructorArgs.Validate();
        
                    if (!validationResult.Result)
                        return BaseResponse.Failure<QuestEntity>(validationResult.Message);
        
                    var quest = QuestFactory.Create(request.QuestConstructorArgs);
        
                    _context.Quests.Update(quest);
                    await _context.SaveChangesAsync(cancellationToken);
        
                    return BaseResponse.Success(quest);
                }
    }
}