using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Quest.API.Helpers.Errors;
using Quest.API.ResourceModels.Participants;
using Quest.Application.Participants.Commands;
using Quest.Application.Teams.Queries;
using Quest.DAL.Data;
using Quest.Domain.Models;
using Quest.API.BindingModels.Emails;
using SendGrid;
using SendGrid.Extensions.DependencyInjection;
using SendGrid.Helpers.Mail;
using Microsoft.Extensions.Configuration;


namespace Quest.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ContactFormController : Controller
    {
        private readonly ISendGridClient _client;
        private readonly IConfiguration _config;


        public ContactFormController(IConfiguration config, ISendGridClient sendGridClient)
        {
            _config = config;
            _client = sendGridClient;
        }

        [HttpPost]
        public async Task<IActionResult> SendContactFormEmail(ContactFormEmailBM model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var from = new EmailAddress(_config.GetValue("SendGrid:From", ""), "Contact form");
            var to = new EmailAddress(_config.GetValue("SendGrid:To", ""), "Questspace");
            var msg = new SendGridMessage
            {
                From = from,
                Subject = "New email from contact form"
            };
            msg.AddContent(MimeType.Text, model.Content);
            msg.AddTo(to);
            var response = await _client.SendEmailAsync(msg).ConfigureAwait(false);
            return Ok(response);
        }
    }
}
