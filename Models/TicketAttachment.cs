using Microsoft.AspNetCore.Http.HttpResults;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Guardian_BugTracker_23.Models
{
    public class TicketAttachment
    {
        private DateTimeOffset _created;

        public int Id { get; set; }
        public string? Description { get; set; }
        // Format for PostgreSQL
        [DataType(DataType.Date)]
        [Display(Name = "Date Created")]
        public DateTimeOffset Created
        {
            get => _created;
            set
            {
                _created = value.ToUniversalTime();
            }
        }

        // Foreign Key Props
        public int TicketId { get; set; }
        [Required]
        public string? BTUserId { get; set; }

        // File Data Props
        [NotMapped]
        public IFormFile? FormFile { get; set; }
        public byte[]? FileData { get; set; }
        public string? FileType { get; set; }

        // Navigation
        public virtual Ticket? Ticket { get; set; }
        public virtual BTUser? BTUser { get; set; }
    }
}
