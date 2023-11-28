using Guardian_BugTracker_23.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Guardian_BugTracker_23.Data
{
    public class ApplicationDbContext : IdentityDbContext<BTUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<BTUser> BTUsers { get; set; }
        public DbSet<BTUser> Members { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Invite> Invites { get; set; }
        public DbSet<TicketComment> Comments { get; set; }
        public DbSet<TicketType> TicketTypes { get; set; }
        public DbSet<TicketAttachment> Attachments { get; set; } 
        public DbSet<TicketStatus> TicketStatuses { get; set; }
        public DbSet<TicketHistory> History { get; set; }
        public DbSet<TicketPriority> TicketPriorities { get; set; }
        public DbSet<ProjectPriority> ProjectPriorities { get; set; }
        public DbSet<NotificationType> NotificationTypes { get; set; }
        public DbSet<Notification> Notifications { get; set; }

 
    }
}
