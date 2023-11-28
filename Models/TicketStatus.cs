using System.ComponentModel.DataAnnotations;

namespace Guardian_BugTracker_23.Models
{
    public class TicketStatus
    {   
        public int Id { get; set; }
        [Display(Name = "Status Name")]
        public string? Name { get; set; }
    }
}
