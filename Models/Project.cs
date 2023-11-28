using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Guardian_BugTracker_23.Models
{
    public class Project
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Project Name")]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} and max {1} characters long.", MinimumLength = 2)]
        public string? Name { get; set; }
        public int CompanyId { get; set; }
        [Required]
        public string? Description { get; set; }
        // Format for PostgreSQL
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset StartDate { get; set;}
        public DateTimeOffset EndDate { get; set;}
        
        public int ProjectPriorityId { get; set; }

        // Image Properties
        [NotMapped]
        public IFormFile? ImageFormFile { get; set; }
        public byte[]? ImageFileData { get; set; }
        public string? ImageFileType { get; set; }

        public bool Archived { get; set; }

        // Navigation
        public virtual Company? Company { get; set; }
        public virtual ProjectPriority? ProjectPriority { get; set; }
        public virtual ICollection<Ticket> Tickets { get; set; } = new HashSet<Ticket>();
        public virtual ICollection<BTUser> Members { get; set; } = new HashSet<BTUser>();
    }
}
