using System.ComponentModel.DataAnnotations;

namespace Guardian_BugTracker_23.Models
{
    public class ProjectPriority
    {   
        public int Id { get; set; }
        [Display(Name = "Project Priority")]
        public string? Name { get; set; }
    }
}
