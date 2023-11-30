﻿using Guardian_BugTracker_23.Models;
using Microsoft.AspNetCore.Identity;

namespace Guardian_BugTracker_23.Services.Interfaces
{
    public interface IBTRolesService
    {
        public Task<bool> AddUserToRoleAsync(BTUser? user, string? roleName);

        public Task<IEnumerable<IdentityRole>> GetRolesAsync();

        public Task<IEnumerable<string>?> GetUserRolesAsync(BTUser? user);

        public Task<List<BTUser>> GetUsersInRoleAsync(string? roleName, int? companyId);

        public Task<bool> IsUserInRoleAsync(BTUser? member, string? roleName);

        public Task<bool> RemoveUserFromRoleAsync(BTUser? user, string? roleName);

        public Task<bool> RemoveUserFromRolesAsync(BTUser? user, IEnumerable<string>? roleNames);
    }
}
