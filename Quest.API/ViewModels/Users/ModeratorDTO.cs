using Quest.Domain.Models;

namespace Quest.API.ViewModels.Users
{
    public class ModeratorDTO : UserDTO
    {
        public ModeratorDTO(ApplicationUser row) : base(row)
        {
            TelegramId = row.TelegramId;
        }
        
        public int TelegramId { get; set; }
    }
}