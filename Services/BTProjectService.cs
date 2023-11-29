using Guardian_BugTracker_23.Controllers;
using Guardian_BugTracker_23.Data;
using Guardian_BugTracker_23.Models;
using Guardian_BugTracker_23.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Guardian_BugTracker_23.Services
{

    public class BTProjectService : IBTProjectService
    {

        private readonly ApplicationDbContext _context;
        private readonly ILogger<ProjectsController> _logger;

        public BTProjectService(ApplicationDbContext context,
                                ILogger<ProjectsController> logger)
        {
            _context = context;
            _logger = logger;
        }


        public Task AddMembersToProjectAsync(IEnumerable<string>? userIds, int? projectId, int? companyId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> AddMemberToProjectAsync(BTUser? member, int? projectId)
        {
            throw new NotImplementedException();
        }

        public Task AddProjectAsync(Project project)
        {
            throw new NotImplementedException();
        }

        public Task<bool> AddProjectManagerAsync(string? userId, int? projectId)
        {
            throw new NotImplementedException();
        }

        public Task ArchiveProjectAsync(Project? project, int? companyId)
        {
            throw new NotImplementedException();
        }

        // Testing
        public async Task<List<Project>> GetAllProjectsASync()
        {
            IEnumerable<Project> results = await _context.Projects.Include(p => p.Company).ToListAsync();
            return results.ToList();
        }

        public async Task<List<Project>> GetAllProjectsByCompanyIdAsync(int? companyId)
        {
            try
            {
                IEnumerable<Project> results = new List<Project>();
                results = await _context.Projects.Where(p => p.CompanyId == companyId).ToListAsync();
                _logger.LogInformation("Connection Successful: Projects loaded");
                return results.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError("Projects not found.");
                throw;
            }
            
           
        }

        public Task<List<Project>> GetArchivedProjectsByCompanyIdAsync(int? companyId)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        public Task<Project> GetProjectByIdAsync(int? projectId, int? companyId)
        {
            throw new NotImplementedException();
        }

        public Task<BTUser> GetProjectManagerAsync(int? projectId)
        {
            throw new NotImplementedException();
        }

        public Task<List<BTUser>> GetProjectMembersByRoleAsync(int? projectId, string? roleName, int? companyId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ProjectPriority>> GetProjectPrioritiesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<Project>?> GetUserProjectsAsync(string? userId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveMemberFromProjectAsync(BTUser? member, int? projectId)
        {
            throw new NotImplementedException();
        }

        public Task RemoveMembersFromProjectAsync(int? projectId, int? companyId)
        {
            throw new NotImplementedException();
        }

        public Task RemoveProjectManagerAsync(int? projectId)
        {
            throw new NotImplementedException();
        }

        public Task RestoreProjectAsync(Project? project, int? companyId)
        {
            throw new NotImplementedException();
        }

        public Task UpdateProjectAsync(Project? project)
        {
            throw new NotImplementedException();
        }
    }
}
