using Quest.Domain.Models;

namespace Quest.API.ResourceModels.Users
{
    public class UserRM
    {
        public UserRM(ApplicationUser row)
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