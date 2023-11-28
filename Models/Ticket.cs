using Guardian_BugTracker_23.Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace Guardian_BugTracker_23.Models
{
    public class Ticket
    {
        private DateTimeOffset _created;
        private DateTimeOffset? _updated;

        public int Id { get; set; }
        [Required]
        [Display(Name = "Ticket Title")]
        [StringLength(200, ErrorMessage = "The {0} must be at least {2} and max {1} characters long.", MinimumLength = 2)]
        public string? Title { get; set; }
        [Required]
        public string? Description { get; set; }
        
        [DataType(DataType.Date)]
        [Display(Name = "Date Created")]
        public DateTimeOffset Created 
        { get => _created;
          set 
            {
                _created = value.ToUniversalTime();
            } 
        }
        [DataType(DataType.Date)]
        [Display(Name = "Date Updated")]
        public DateTimeOffset? Updated 
        { get => _updated;
            set 
            {
                _updated = value.HasValue ? value.Value.ToUniversalTime() : null;
            } 
        }
        public bool Archived { get; set; }
        public bool ArchivedByProject { get; set; }

        public BTTicketTypes TicketType { get; set; }
        public BTTicketStatuses TicketStatus { get; set; }
        public BTTicketPriorities TicketPriority { get; set; }

        // Foreign Key Props
        public int ProjectId { get; set; }
        //public int TicketTypeId { get; set; }
        //public int TicketStatusId { get; set; }
        //public int TicketPriorityId { get; set; }
        public string? DeveloperUserId { get; set; }
        [Required]
        public string? SubmitterUserId { get; set; }

        // Navigation Props
        public virtual Project? Project { get; set; }
        //public virtual TicketPriority? TicketPriority { get; set; }
        /// <summary>
        //public virtual TicketType? TicketType { get; set; } 
        /// </summary>
        //public virtual TicketStatus? TicketStatus { get; set; }
        public virtual BTUser? DeveloperUser { get; set; }
        public virtual BTUser? SubmitterUser { get; set; }


        public virtual ICollection<TicketComment> Comments { get; set; } = new HashSet<TicketComment>();
        public virtual ICollection<TicketAttachment> Attachments { get; set; } = new HashSet<TicketAttachment>();
        public virtual ICollection<TicketHistory> History { get; set; } = new HashSet<TicketHistory>();



    }
}
