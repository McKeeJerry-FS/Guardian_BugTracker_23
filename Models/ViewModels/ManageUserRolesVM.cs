using Microsoft.AspNetCore.Mvc.Rendering;

namespace Guardian_BugTracker_23.Models.ViewModels
{
    public class ManageUserRolesVM
    {
        public BTUser? BTUser { get; set; }
        public MultiSelectList? Roles { get; set; }
        public IEnumerable<string>? SelectedRoles { get; set; }
    }
}
