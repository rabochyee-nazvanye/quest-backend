using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Quest.Domain.Models;

namespace Quest.API.Models.ViewModels.Profiles
{
    public class ProfileVM
    {
        public ProfileVM(ApplicationUser row)
        {
            Id = row.Id;
            Name = row.UserName;
            Email = row.Email;
            AvatarUrl = row.AvatarUrl;
        }
        
        public string Id { get; }
        public string Name { get; }
        public string Email { get; }
        public string AvatarUrl { get; }
    }
}