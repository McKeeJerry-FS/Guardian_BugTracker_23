using Guardian_BugTracker_23.Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace Guardian_BugTracker_23.Models
{
    public class Notification
    {
        private DateTimeOffset _created;


        public int Id { get; set; }
        public int? ProjectId { get; set; }
        public int? TicketId { get; set; }
        // Add attributes for StringLength, Display
        [Required]
        [Display(Name = "Notification Title")]
        public string? Title { get; set; }
        [Required]
        public string? Message { get; set; }
        
        [DataType(DataType.Date)]
        [Display(Name = "Date Created")]
        public DateTimeOffset Created 
        { get => _created;
          set 
            {
                _created = value.ToUniversalTime();
            } 
        }
        [Required]
        public string? SenderId { get; set; }
        [Required]
        public string? RecipientId { get; set;}
        //public int NotificationTypeId { get; set; }
        public bool HasBeenViewed { get; set; }
        public BTNotificationType NotificationType { get; set; }

        // Navigation
        //public virtual NotificationType? NotificationType { get; set; }
        public virtual Project? Project { get; set; }
        public virtual Ticket? Ticket { get; set; }
        public virtual BTUser? Sender { get; set; }
        public virtual BTUser? Recipient { get; set; }
    }

}
