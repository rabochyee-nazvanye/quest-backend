using Quest.Domain.Models;

namespace Quest.API.Models.ViewModels.Users
{
    public class UserBasicInfoVM
    {
        public UserBasicInfoVM(ApplicationUser row)
        {
            Id = row.Id;
            UserName = row.UserName;
            ImageUrl = row.AvatarUrl;
        }
        
        public string Id { get; set; }
        public string UserName { get; set; }
        public string ImageUrl { get; set; }
    }
}