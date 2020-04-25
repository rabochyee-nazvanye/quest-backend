using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Quest.Domain.Models;

namespace Quest.Application
{
    public class AttemptsHub : Hub
    {
        public async Task Send(TaskAttempt taskAttempt)
        {
            await this.Clients.All.SendAsync("Send", taskAttempt);
        }
    }
}