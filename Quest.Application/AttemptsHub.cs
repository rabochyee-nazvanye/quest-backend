using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Quest.Domain.Models;

namespace Quest.Application
{
    [Authorize(Roles = "Admin")]
    public class AttemptsHub : Hub
    {
        public async Task Send(TaskAttempt taskAttempt)
        {
            await this.Clients.All.SendAsync("Send", taskAttempt);
        }
    }
}