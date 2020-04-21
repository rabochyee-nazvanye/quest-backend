using System.ComponentModel.DataAnnotations;
using MediatR;
using Quest.Application;

namespace Quest.API.ViewModels.TaskAttempts
{
    public class TaskAttemptVM
    {
        [Required] public int TeamId { get; set; }
        [Required] public string AttemptText { get; set; }
    }
}