using System.ComponentModel.DataAnnotations;

namespace Quest.API.BindingModels.Emails
{
    public class ContactFormEmailBM
    {
        [Required]
        public string Content { get; set; }
    }
}