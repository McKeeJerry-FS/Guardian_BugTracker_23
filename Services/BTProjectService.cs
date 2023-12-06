using Guardian_BugTracker_23.Controllers;
using Guardian_BugTracker_23.Data;
using Guardian_BugTracker_23.Data.Enums;
using Guardian_BugTracker_23.Models;
using Guardian_BugTracker_23.Services.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Guardian_BugTracker_23.Services
{

    public class BTProjectService : IBTProjectService
    {

        private readonly ApplicationDbContext _context;
        private readonly ILogger<ProjectsController> _logger;
        private readonly IBTRolesService _bTRolesService;

        public BTProjectService(ApplicationDbContext context,
                                ILogger<ProjectsController> logger,
                                IBTRolesService bTRolesService)
        {
            _context = context;
            _logger = logger;
            _bTRolesService = bTRolesService;
        }


        public async Task AddMembersToProjectAsync(IEnumerable<string>? userIds, int? projectId, int? companyId)
        {
            try
            {
                Project? project = await _context.Projects.Where(p => p.CompanyId == companyId)
                                                          .FirstOrDefaultAsync(p => p.Id == projectId);

                foreach (string userId in userIds!)
                {
                    BTUser? user = await _context.Users.FindAsync(userId);
                    if (project != null && user != null) 
                    {
                        _logger.LogInformation($"{user.FullName} was found, {DateTimeOffset.Now}");
                        project.Members!.Add(user);
                        _logger.LogInformation($"{userId} was added to the project successfully, {DateTimeOffset.Now}");
                    }
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex} - Error coming from the database, {DateTimeOffset.Now}");
                throw;
            }
        }

        public async Task<bool> AddMemberToProjectAsync(BTUser? member, int? projectId)
        {
            try
            {
                if (member != null && projectId != null)
                {
                    Project? project = await GetProjectByIdAsync(projectId, member.CompanyId);

                    if (project != null)
                    {
                        // Project instance must "Include" Members to do the following:
                        bool IsOnProject = project.Members.Any(m => m.Id == member.Id);
                        if (!IsOnProject)
                        {
                            project.Members.Add(member);
                            await _context.SaveChangesAsync();
                            return true;
                        }
                    }
                }
                return false;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public Task AddProjectAsync(Project project)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> AddProjectManagerAsync(string? userId, int? projectId)
        {
            try
            {
                if (userId != null && projectId != null)
                {
                    BTUser? currentPM = await GetProjectManagerAsync(projectId);
                    BTUser? selectedPM = await _context.Users.FindAsync(userId);

                    //Remove current PM
                    if (currentPM != null)
                    {
                        await RemoveProjectManagerAsync(projectId);
                    }

                    // Add new PM
                    try
                    {
                        await AddMemberToProjectAsync(selectedPM!, projectId);
                        return true;
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                }
                return false;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public Task ArchiveProjectAsync(Project? project, int? companyId)
        {
            try
            {
                throw new NotImplementedException();
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        // Testing
        public async Task<List<Project>> GetAllProjectsASync()
        {
            IEnumerable<Project> results = await _context.Projects.Include(p => p.Company).ToListAsync();
            return results.ToList();
        }

        public async Task<List<Project>> GetAllProjectsByCompanyIdAsync(int? companyId)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();

            try
            {
                IEnumerable<Project> results = new List<Project>();
                results = await _context.Projects.Where(p => p.CompanyId == companyId).ToListAsync();
                Log.Information("Projects were acquired successfully", DateTimeOffset.Now.ToString("MM dd, yyyy - HH:mm"));
                _logger.LogInformation("Connection Successful: Projects loaded");
                return results.ToList();
            }
            catch (Exception ex)
            {
                Log.Warning($"Something went wrong: {ex}", DateTimeOffset.Now.ToString("MM dd, yyyy - HH:mm"));
                _logger.LogError("Projects not found.");
                throw;
            }
            
           
        }

        public Task<List<Project>> GetArchivedProjectsByCompanyIdAsync(int? companyId)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        public async Task<Project> GetProjectByIdAsync(int? projectId, int? companyId)
        {
            try
            {
                var project = await _context.Projects.Where(p => p.Id == projectId)
                                               .Include(p => p.Company)
                                               .FirstOrDefaultAsync();
                
                return project;
                
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, DateTimeOffset.Now.ToString("MM dd, yyyy - HH:mm:ss"));
                throw;
            }
        }

        public async Task<BTUser> GetProjectManagerAsync(int? projectId)
        {
            try
            {
                Project? project = await _context.Projects.Include(p => p.Members).FirstOrDefaultAsync(p => p.Id == projectId);
                if (project != null)
                {
                    foreach (BTUser member in project.Members)
                    {
                        if (await _bTRolesService.IsUserInRoleAsync(member, nameof(BTRoles.ProjectManager)))
                        {
                            return member;

                        }
                    }
                }
                return null;
                 
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<BTUser> GetProjectMemberAsync(int? projectId)
        {
            try
            {
                Project? project = await _context.Projects.Include(p => p.Members).FirstOrDefaultAsync(p => p.Id == projectId);
                if (project != null)
                {
                    foreach (BTUser member in project.Members)
                    {
                        if (await _bTRolesService.IsUserInRoleAsync(member, nameof(BTRoles.Developer)) || 
                            await _bTRolesService.IsUserInRoleAsync(member, nameof(BTRoles.Submitter)) || 
                            await _bTRolesService.IsUserInRoleAsync(member, nameof(BTRoles.DemoUser)) || 
                            await _bTRolesService.IsUserInRoleAsync(member, nameof(BTRoles.Admin)))
                        {
                            return member;

                        }
                    }
                }
                return null;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public Task<List<BTUser>> GetProjectMembersByRoleAsync(int? projectId, string? roleName, int? companyId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<ProjectPriority>> GetProjectPrioritiesAsync()
        {
            try
            {
                List<ProjectPriority> results = await _context.ProjectPriorities.Include(pp => pp.Id).ToListAsync();
                _logger.LogInformation("Database successfully reached, ProjectPriorities list created.");
                return results;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString(), DateTimeOffset.Now.ToString("MMM dd, yyyy - HH:mm"));
                throw;
            }
        }

        public Task<List<Project>?> GetUserProjectsAsync(string? userId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> RemoveMemberFromProjectAsync(BTUser? member, int? projectId)
        {
            try
            {
                if (member != null && projectId != null)
                {
                    Project? project = await GetProjectByIdAsync(projectId, member.CompanyId);

                    if (project != null)
                    {
                        // Project instance must "Include" Members to do the following:
                        bool IsOnProject = project.Members.Any(m => m.Id == member.Id);
                        if (!IsOnProject)
                        {
                            project.Members.Remove(member);
                            await _context.SaveChangesAsync();
                            return true;
                        }
                    }
                }
                return false;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task RemoveMembersFromProjectAsync(int? projectId, int? companyId)
        {
            try
            {
                Project? project = await _context.Projects.Where(p => p.CompanyId == companyId).FirstOrDefaultAsync(p => p.Id == projectId);
                if (project != null)
                {
                    project.Members!.Clear();
                    _context.Update(project);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task RemoveProjectManagerAsync(int? projectId)
        {
            try
            {
                if (projectId != null)
                {
                    Project? project = await _context.Projects.Include(p => p.Members).FirstOrDefaultAsync(p => p.Id == projectId);
                    if (project != null)
                    {
                        foreach(BTUser member in project!.Members)
                        {
                            if (await _bTRolesService.IsUserInRoleAsync(member, nameof(BTRoles.ProjectManager)))
                            {
                                // Remove BTUser from project
                                await RemoveMemberFromProjectAsync(member, projectId);
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
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
