using Quest.Domain.Models;

namespace Quest.API.ResourceModels.Users
{
    public class ModeratorRm : UserRM
    {
        public ModeratorRm(ApplicationUser row) : base(row)
        {
            TelegramId = row.TelegramId;
        }
        
        public int TelegramId { get; set; }
    }
}