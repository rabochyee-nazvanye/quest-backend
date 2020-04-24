using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Quest.API.ViewModels.Tasks
{
    public class CreateTaskVM
    {
        [Required] public string Name { get; set; }
        [Required] public int Reward { get; set; }
        [Required] public bool VerificationIsManual  { get; set; }
        [Required] public string Question { get; set; }
        [Required] public string CorrectAnswer { get; set; }
        [Required] public string Group { get; set; }
        [Required] public List<string> Hints { get; set; }
    }
}