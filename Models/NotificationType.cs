using System.ComponentModel.DataAnnotations;

namespace Guardian_BugTracker_23.Models
{
    public class NotificationType
    {
        public int Id { get; set; }
        // Add attributes for StringLength
        [Display(Name = "Notification Type Name")]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} and max {1} characters long.", MinimumLength = 2)]
        public string? Name { get; set; }
    }
}
