using Guardian_BugTracker_23.Data;
using Guardian_BugTracker_23.Models;
using Guardian_BugTracker_23.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Guardian_BugTracker_23.Services
{
    public class BTCompanyService : IBTCompanyService
    {
        private readonly ILogger<BTCompanyService> _logger;
        private readonly ApplicationDbContext _context;

        public BTCompanyService(ILogger<BTCompanyService> logger,
                                ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        


        public async Task<Company> GetCompanyInfoAsync(int? companyId)
        {
            try
            {
                if(companyId != null)
                {
                    Company? company =  await _context.Companies.FirstOrDefaultAsync(c => c.Id == companyId);
                    return company!;
                }
                return null!;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public Task<List<Invite>> GetInvitesAsync(int? companyId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<BTUser>> GetMembersAsync(int? companyId)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();

            try
            {
                IEnumerable<BTUser> members = Enumerable.Empty<BTUser>();
                
                if(companyId != null)
                {
                    members = await _context.Users
                                                  .Where(u => u.CompanyId == companyId)
                                                  .ToListAsync();
                }
                Log.Information("Users were acquired successfully", DateTimeOffset.Now.ToString("MM dd, yyyy - HH:mm"));
                return members;
            }
            catch (Exception)
            {
                Log.Error("Something went wrong: Members were not found", DateTimeOffset.Now.ToString("MM dd, yyyy - HH:mm"));
                throw;
            }
        }

        public Task<List<Project>> GetProjectsAsync(int? companyId)
        {
            throw new NotImplementedException();
        }
    }
}
