using Guardian_BugTracker_23.Data;
using Guardian_BugTracker_23.Models;
using Guardian_BugTracker_23.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Guardian_BugTracker_23.Data.Enums;
using Serilog;

namespace Guardian_BugTracker_23.Services
{
   
    public class BTRolesService : IBTRolesService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<BTUser> _userManager;
        private readonly ILogger<BTRolesService> _logger;

        public BTRolesService(ApplicationDbContext context,
                              UserManager<BTUser> userManager,
                              ILogger<BTRolesService> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }


        public async Task<bool> AddUserToRoleAsync(BTUser? user, string? roleName)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();

            try
            {
                if (user != null && !string.IsNullOrEmpty(roleName))
                {
                    bool result = (await _userManager.AddToRoleAsync(user, roleName)).Succeeded;
                    Log.Information($"This function is {result}", DateTimeOffset.Now.ToString("MM dd, yyyy - HH:mm"));
                    return result;
                }
                
                Log.Error("Something has gone wrong!", DateTimeOffset.Now.ToString("MM dd, yyyy - HH:mm"));
                return false;
            }
            catch (Exception ex)
            {
                Log.Error($"Something has gone wrong: {ex}", DateTimeOffset.Now.ToString("MM dd, yyyy - HH:mm"));
                throw;
            }
        }

        public async Task<IEnumerable<IdentityRole>> GetRolesAsync()
        {
            try
            {
                Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();

                IEnumerable<IdentityRole> result = Enumerable.Empty<IdentityRole>();
                result = await _context.Roles.AsNoTracking()
                                             .ToListAsync();
                
                Log.Information("Roles have been acquired successfully",  DateTimeOffset.Now.ToString("MM dd, yyyy - HH:mm"));
                return result;
            }
            catch (Exception ex)
            {
                Log.Error($"Something has gone wrong: {ex}", DateTimeOffset.Now.ToString("MM dd, yyyy - HH:mm"));
                throw;
            }
        }

        public async Task<IEnumerable<string>?> GetUserRolesAsync(BTUser? user)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();

            try
            {
                if (user != null)
                {
                    IEnumerable<string> userRoles = await _userManager.GetRolesAsync(user);
                    return userRoles;
                }
                return null;
            }
            catch (Exception ex)
            {
                Log.Warning($"Something went wrong: {ex}");
                throw;
            }
        }

        public async Task<List<BTUser>> GetUsersInRoleAsync(string? roleName, int? companyId)
        {
            try
            {
                IEnumerable<BTUser>? usersInRoles = await _userManager.GetUsersInRoleAsync(roleName!); 
                return usersInRoles.ToList();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> IsUserInRoleAsync(BTUser? member, string? roleName)
        {
            try
            {
                bool result = false;
                if (member != null && roleName != null)
                {
                    await _userManager.IsInRoleAsync(member, roleName);
                    result = true;
                }
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> RemoveUserFromRoleAsync(BTUser? user, string? roleName)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();

            try
            {
                if (user != null && !string.IsNullOrEmpty(roleName))
                {
                    bool result = (await _userManager.RemoveFromRoleAsync(user, roleName)).Succeeded;
                    Log.Information($"This function is {result}", DateTimeOffset.Now.ToString("MM dd, yyyy - HH:mm"));
                    return result;
                }

                Log.Error("Something has gone wrong!", DateTimeOffset.Now.ToString("MM dd, yyyy - HH:mm"));
                return false;
            }
            catch (Exception ex)
            {
                Log.Error($"Something has gone wrong: {ex}", DateTimeOffset.Now.ToString("MM dd, yyyy - HH:mm"));
                throw;
            }
        }

        public async Task<bool> RemoveUserFromRolesAsync(BTUser? user, IEnumerable<string>? roleNames)
        {
            try
            {
                Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();

                if (user != null && roleNames != null)
                {
                    bool result = (await _userManager.RemoveFromRolesAsync(user, roleNames)).Succeeded;
                    Log.Information("Users removed from roles successfully");
                    return result;
                }

                return false;

            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
