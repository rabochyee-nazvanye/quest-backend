using System.ComponentModel.DataAnnotations;

namespace Quest.API.BindingModels.TaskAttempts
{
    public class TaskAttemptRM
    { 
        [Required] public string AttemptText { get; set; }
    }
}