﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Quest.API.Services;
using Quest.API.ViewModels.Teams;
using Quest.Application.Teams.Commands;
using Quest.Application.Teams.Queries;
using Quest.DAL.Data;
using Quest.Domain.Models;


namespace Quest.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TeamsController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMediator _mediator;

        public TeamsController(UserManager<ApplicationUser> userManager, IMediator mediator)
        {
            _userManager = userManager;
            _mediator = mediator;
        }


        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetTeamById(int id)
        {
            var team = await _mediator.Send(new GetTeamInfoQuery(id));
          
            if (team == null)
            {
                return NotFound();
            }

            return Json(new TeamVM(team));
        }


        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Post(CreateTeamVM model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var userId = _userManager.GetUserId(User);

            var createTeamCommand = new CreateTeamCommand(model.Name, userId, model.QuestId);

            var response = await _mediator.Send(createTeamCommand);

            if (response.Result == null)
                return BadRequest(response.Message);

            return Created("/team/" + response.Result.Id, response.Result.Id);
        }


        [Authorize]
        [HttpPost("{teamId}/join/{requestSecret}")]
        public async Task<IActionResult> AddUserToTeam(int teamId, string requestSecret)
        {
            var userId = _userManager.GetUserId(User);

            var command = new AddUserToTeamCommand(userId, requestSecret, teamId);

            var response = await _mediator.Send(command);

            if (response.Result)
                return Ok(response.Message);

            return BadRequest(response.Message);
        }
        
        
        [Authorize]
        [HttpDelete("{teamId}/kick/{userToKickId}")]
        public async Task<IActionResult> KickUserFromTheTeam(int teamId, string userToKickId)
        {
            var userId = _userManager.GetUserId(User);

            var response = await _mediator.Send(new RemoveUserFromTeamCommand(teamId, userId, userToKickId));

            if (!response.Result)
                return BadRequest(response.Message);

            return Ok(response.Message);
        }

        [Authorize]
        [HttpDelete("{teamId}")]
        public async Task<IActionResult> RemoveTeam(int teamId)
        {
            var userId = _userManager.GetUserId(User);

            var response = await _mediator.Send(new RemoveTeamCommand(userId, teamId));

            if (!response.Result)
                return BadRequest(response.Message);

            return Ok(response.Message);
        }
    }
}