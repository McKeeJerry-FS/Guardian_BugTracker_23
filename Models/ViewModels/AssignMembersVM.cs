using Microsoft.AspNetCore.Mvc.Rendering;

namespace Guardian_BugTracker_23.Models.ViewModels
{
    public class AssignMembersVM
    {
        public int ProjectId { get; set; }
        public string? ProjectName { get; set; }
        public MultiSelectList? MembersList { get; set; }
        public string? PMId { get; set; }
        public string? UserId { get; set; }
    }
}
