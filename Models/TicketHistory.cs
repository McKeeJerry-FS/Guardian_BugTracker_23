using System.ComponentModel.DataAnnotations;

namespace Guardian_BugTracker_23.Models
{
    public class TicketHistory
    {
        private DateTimeOffset _created;

        public int Id { get; set; }
        //Foreign Key
        public int TicketId { get; set; }
        [Display(Name = "Property Name")]
        public string? PropertyName { get; set; }
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
        public string? OldValue { get; set; }
        public string? NewValue { get; set; }
        [Required]
        public string? UserId { get; set; }

        // Navigation Props
        public virtual Ticket? Ticket { get; set; }
        public virtual BTUser? User { get; set; }
    }
}
