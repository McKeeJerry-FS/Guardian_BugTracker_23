using Guardian_BugTracker_23.Models;

namespace Guardian_BugTracker_23.Services.Interfaces
{
    public interface IBTCompanyService
    {
        public Task<Company> GetCompanyInfoAsync(int? companyId);

        public Task<IEnumerable<BTUser>> GetMembersAsync(int? companyId);

        public Task<List<Project>> GetProjectsAsync(int? companyId);

        public Task<List<Invite>> GetInvitesAsync(int? companyId);
    }
}
