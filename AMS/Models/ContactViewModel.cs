/// This is a legacy model that can be used to send emails without creating a ticket, if the user so desires. 
/// Adopted from online code such as this: https://dotnetcoretutorials.com/2017/11/02/using-mailkit-send-receive-email-asp-net-core/
/// Coded by Jack. 
using System.ComponentModel.DataAnnotations;

namespace AMS.Models {
    /// <summary>
    /// Just a simple ContactViewModel Class that can be utilized to send emails. 
    /// </summary>
    public class ContactViewModel {
        [Required]
        [StringLength(60, MinimumLength = 5)]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Subject { get; set; }
        [Required]
        public string Message { get; set; }
    }
}