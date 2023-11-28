using System.ComponentModel.DataAnnotations;

namespace Guardian_BugTracker_23.Models
{
    public class TicketComment
    {
        public int Id { get; set; }
        [Required]
        [StringLength(2000, ErrorMessage = "The {0} must be at least {2} and max {1} characters long.", MinimumLength = 2)]
        public string? Comment { get; set; }
        // Format for PostgreSQL
        public DateTimeOffset Created { get; set; }

        // Foreign Key Props
        public int TicketId { get; set; }
        [Required]
        public string? UserId { get; set; }

        // Navigation
        public virtual Ticket? Ticket { get; set; }
        public virtual BTUser? User { get; set; }
    }
}
