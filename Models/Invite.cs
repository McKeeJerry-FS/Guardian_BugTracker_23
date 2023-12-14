using Guardian_BugTracker_23.Models;
using System.ComponentModel.DataAnnotations;

namespace Guardian_BugTracker_23.Models
{
    public class Invite
    {

        private DateTimeOffset _inviteDate;
        private DateTimeOffset _joinDate;

        public int Id { get; set; }
        // Format dates for PostgreSQL
        [DataType(DataType.Date)]
        [Display(Name = "Date Sent")]
        public DateTimeOffset InviteDate 
        { get => _inviteDate; 
          set 
            {
                _inviteDate = value.ToUniversalTime();
            } 
        }
        [DataType(DataType.Date)]
        [Display(Name = "Date Joined")]
        public DateTimeOffset? JoinDate 
        { get => _joinDate;
          set 
            {
                _joinDate = value.Value.ToUniversalTime();
            }
        }
        public Guid CompanyToken { get; set; }
        public int CompanyId { get; set; }
        public int ProjectId { get; set; }
        [Required]
        public string? InvitorId { get; set; }
        public string? InviteeId { get; set; }
        [Required]
        public string? InviteeEmail { get; set; }
        [Required]
        public string? InviteeFirstName { get; set; }
        [Required]
        public string? InviteeLastName { get; set; }
        // Add attributes for StringLength
        public string? Message { get; set; }
        public bool IsValid { get; set; }

        // Navigation
        public virtual Company? Company { get; set; }
        public virtual Project? Project { get; set; }
        public virtual BTUser? Invitor { get; set; }
        public virtual BTUser? Invitee { get; set; }
    }
}
