using Guardian_BugTracker_23.Data.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Guardian_BugTracker_23.Models
{
    public class Project
    {
        private DateTimeOffset _created;
        private DateTimeOffset _startDate;
        private DateTimeOffset _endDate;
        public int Id { get; set; }
        [Required]
        [Display(Name = "Project Name")]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} and max {1} characters long.", MinimumLength = 2)]
        public string? Name { get; set; }
        public int CompanyId { get; set; }
        [Required]
        public string? Description { get; set; }
        
        [DataType(DataType.Date)]
        [Display(Name = "Date Created")]
        public DateTimeOffset Created {
            get => _created; 
            set
            {
                _created = value.ToUniversalTime();
            }
        }
        [DataType(DataType.Date)]
        [Display(Name = "Date Started")]
        public DateTimeOffset? StartDate { 
            get => _startDate;
            set 
            {
                _startDate = value.Value.ToUniversalTime();
            } 
        }
        [DataType(DataType.Date)]
        [Display(Name = "Date Ended")]
        public DateTimeOffset? EndDate
        {
            get => _endDate;
            set 
            {
                _endDate = value.Value.ToUniversalTime();            
            }
        
        }
        public BTProjectPriorities ProjectPriority { get; set; }

        //public int ProjectPriorityId { get; set; }

        // Image Properties
        [NotMapped]
        public IFormFile? ImageFormFile { get; set; }
        [Display(Name = "Project Image")]
        public byte[]? ImageFileData { get; set; }
        [Display(Name = "File Extension")]
        public string? ImageFileType { get; set; }

        public bool Archived { get; set; }

        // Navigation
        public virtual Company? Company { get; set; }
        //public virtual ProjectPriority? ProjectPriority { get; set; }
        public virtual ICollection<Ticket> Tickets { get; set; } = new HashSet<Ticket>();
        public virtual ICollection<BTUser> Members { get; set; } = new HashSet<BTUser>();
    }
}
