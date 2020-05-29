using System;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Quest.Application.DTOs;
using Quest.Application.Users.Commands;
using Quest.DAL.Data;
using Quest.Domain.Models;

namespace Quest.Application.Services.ExternalAuth
{
    public class ExternalAuthService : IExternalAuthService
    {
        private readonly Db _context;
        private readonly IMediator _mediator;

        public ExternalAuthService(Db context, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }

        public async Task<BaseResponse<ApplicationUser>> GetUserAsync(ExternalAuthCredentials credentials)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.UserName == credentials.Username);
            if (user != null)
                return BaseResponse.Success(user);

            return await _mediator.Send(
                new CreateUserCommand(credentials.Username, Guid.NewGuid().ToString()));
        }
    }
    
    public interface IExternalAuthService
    {
        Task<BaseResponse<ApplicationUser>> GetUserAsync(ExternalAuthCredentials credentials);
    }
}