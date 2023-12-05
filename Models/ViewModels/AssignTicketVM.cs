using Microsoft.AspNetCore.Mvc.Rendering;

namespace Guardian_BugTracker_23.Models.ViewModels
{
    public class AssignTicketVM
    {
        public Ticket? Ticket { get; set; }
        public SelectList? Developers { get; set; }
        public string? DeveloperId { get; set; }
    }
}
