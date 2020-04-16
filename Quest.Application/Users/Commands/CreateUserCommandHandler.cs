using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Internal;
using Quest.Domain.Models;

namespace Quest.Application.Users.Commands
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, BaseResponse<ApplicationUser>>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public CreateUserCommandHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }


        public async Task<BaseResponse<ApplicationUser>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var user = new ApplicationUser
            {
                AvatarUrl = request.AvatarUrl,
                UserName = request.Username
            };

            var result = await _userManager.CreateAsync(user, request.Password);
            if (result.Succeeded)
            {
                return BaseResponse.Success(user, "Successfully created a new user");
            }

            return BaseResponse.Failure<ApplicationUser>($"Error creating new user: {string.Join(' ',result.Errors.Select(x => x.Description))}");
        }
    }
}
