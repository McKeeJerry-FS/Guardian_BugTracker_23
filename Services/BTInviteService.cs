using Guardian_BugTracker_23.Models;
using Guardian_BugTracker_23.Services.Interfaces;

namespace Guardian_BugTracker_23.Services
{
    public class BTInviteService : IBTInviteService
    {
        public Task<bool> AcceptInviteAsync(Guid? token, string? userId)
        {
            throw new NotImplementedException();
        }

        public Task AddNewInviteAsync(Invite? invite)
        {
            throw new NotImplementedException();
        }

        public Task<bool> AnyInviteAsync(Guid? token, string? email, int? companyId)
        {
            throw new NotImplementedException();
        }

        public Task<Invite?> GetInviteAsync(int? inviteId, int? companyId)
        {
            throw new NotImplementedException();
        }

        public Task<Invite?> GetInviteAsync(Guid? token, string? email, int? companyId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ValidateInviteCodeAsync(Guid? token)
        {
            throw new NotImplementedException();
        }
    }
}
