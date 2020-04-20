using Quest.Domain.Models;

namespace Quest.API.ViewModels.Users
{
    public class ModeratorVM : UserVM
    {
        public ModeratorVM(ApplicationUser row) : base(row)
        {
            TelegramId = row.TelegramId;
        }
        
        public int TelegramId { get; set; }
    }
}