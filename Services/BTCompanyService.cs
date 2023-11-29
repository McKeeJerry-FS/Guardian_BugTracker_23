using Guardian_BugTracker_23.Models;
using Guardian_BugTracker_23.Services.Interfaces;

namespace Guardian_BugTracker_23.Services
{
    public class BTCompanyService : IBTCompanyService
    {
        public Task<Company> GetCompanyInfoAsync(int? companyId)
        {
            throw new NotImplementedException();
        }

        public Task<List<Invite>> GetInvitesAsync(int? companyId)
        {
            throw new NotImplementedException();
        }

        public Task<List<BTUser>> GetMembersAsync(int? companyId)
        {
            throw new NotImplementedException();
        }

        public Task<List<Project>> GetProjectsAsync(int? companyId)
        {
            throw new NotImplementedException();
        }
    }
}
