using Quest.Domain.Models;

namespace Quest.API.ViewModels.Users
{
    public class UserVM
    {
        public UserVM(ApplicationUser row)
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