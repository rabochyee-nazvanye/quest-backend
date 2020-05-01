using Quest.Domain.Models;

namespace Quest.API.ViewModels.Users
{
    public class UserDTO
    {
        public UserDTO(ApplicationUser row)
        {
            Id = row.Id;
            Name = row.UserName;
            AvatarUrl = row.AvatarUrl;
        }
        
        public string Id { get; }
        public string Name { get; }
        public string AvatarUrl { get; }
    }
}