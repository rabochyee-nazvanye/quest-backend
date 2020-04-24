using System.ComponentModel.DataAnnotations;
using MediatR;
using Quest.Application;

namespace Quest.API.ViewModels.TaskAttempts
{
    public class TaskAttemptVM
    { 
        [Required] public string AttemptText { get; set; }
    }
}