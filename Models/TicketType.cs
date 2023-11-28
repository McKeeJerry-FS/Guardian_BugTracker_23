using System.ComponentModel.DataAnnotations;

namespace Guardian_BugTracker_23.Models
{
    public class TicketType
    {
        public int Id { get; set; }
        
        [Display(Name = "Ticket Type")]
        public string? Name { get; set; }
    }
}
