using System.ComponentModel.DataAnnotations;

namespace Guardian_BugTracker_23.Models
{
    public class TicketAttachment
    {
        public int Id { get; set; }
        public string? Description { get; set; }
        // Format for PostgreSQL
        public DateTimeOffset Created { get; set; }

        // Foreign Key Props
        public int TicketId { get; set; }
        [Required]
        public string? BTUserId { get; set; }

        // File Data Props
        public IFormFile? FormFile { get; set; }
        public byte[]? FileData { get; set; }
        public string? FileType { get; set; }

        // Navigation
        public virtual Ticket? Ticket { get; set; }
        public virtual BTUser? BTUser { get; set; }
    }
}
