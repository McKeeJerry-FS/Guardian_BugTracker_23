using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Guardian_BugTracker_23.Data;
using Guardian_BugTracker_23.Models;
using Guardian_BugTracker_23.Services.Interfaces;
using Guardian_BugTracker_23.Data.Enums;
using X.PagedList;
using Microsoft.AspNetCore.Authorization;
using Guardian_BugTracker_23.Models.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace Guardian_BugTracker_23.Controllers
{
    [Authorize]
    public class ProjectsController : BTBaseController
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ProjectsController> _logger;
        private readonly IBTProjectService _btProjectService;
        private readonly IBTRolesService _btRolesService;
        private readonly UserManager<BTUser> _userManager;

        public ProjectsController(ApplicationDbContext context,
                                  ILogger<ProjectsController> logger,
                                  IBTProjectService bTProjectService,
                                  IBTRolesService bTRolesService,
                                  UserManager<BTUser> userManager
                                  )
        {
            _context = context;
            _logger = logger;
            _btProjectService = bTProjectService;
            _btRolesService = bTRolesService;
            _userManager = userManager;
        }

        // GET: Projects
        public async Task<IActionResult> Index(int? pageNum)
        {
            //IPagedList<Project> projects;
            //// new service included
            //int pageSize = 4;
            //int page = pageNum ?? 1;

            //projects = await (await _btProjectService.GetAllProjectsASync()).ToPagedListAsync(page, pageSize);
            //return View(projects);
            List<Project> results;
            results = await _btProjectService.GetAllProjectsByCompanyIdAsync(_companyId);
            return View(results.ToList());

        }

        // GET: Projects/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            // modify the code for the Service layer once it is working as it should here

            if (id == null)
            {
                return NotFound();
            }


            var project = await _btProjectService.GetProjectByIdAsync(id, _companyId);



            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        // GET: Projects/Create
        public IActionResult Create()
        {

            ViewData["ProjectPriority"] = new SelectList(_context.ProjectPriorities, "Id", "Name");
            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Name");
            return View();
        }

        // POST: Projects/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,CompanyId,Description,Created,StartDate,EndDate,ProjectPriority,ImageFileData,ImageFileType,Archived")] Project project)
        {
            if (ModelState.IsValid)
            {
                _context.Add(project);
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Project was successfully created: {DateTimeOffset.Now: MM dd, yyyy - HH:mm}");
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProjectPriority"] = new SelectList(_context.ProjectPriorities, "Id", "Name", project.ProjectPriority);
            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Name", _companyId);
            return View(project);
        }

        // GET: Projects/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Projects == null)
            {
                return NotFound();
            }

            var project = await _context.Projects.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }

            ViewData["ProjectPriority"] = new SelectList(_context.ProjectPriorities, "Id", "Name", project.ProjectPriority);
            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Name", _companyId);
            return View(project);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,CompanyId,Description,Created,StartDate,EndDate,ProjectPriority,ImageFileData,ImageFileType,Archived")] Project project)
        {
            if (id != project.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(project);
                    _logger.LogInformation($"Project was successfully updated: {DateTimeOffset.Now.ToUniversalTime()}");
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectExists(project.Id))
                    {
                        _logger.LogError($"Project was not found: {DateTimeOffset.Now.ToUniversalTime()}");
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProjectPriority"] = new SelectList(_context.ProjectPriorities, "Id", "Name", project.ProjectPriority);

            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Name", _companyId);
            return View(project);
        }

        // GET: Projects/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Projects == null)
            {
                return NotFound();
            }

            var project = await _context.Projects
                .Include(p => p.Company)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Projects == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Projects'  is null.");
            }
            var project = await _context.Projects.FindAsync(id);
            if (project != null)
            {
                _context.Projects.Remove(project);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        // AssignPM Methods
        [HttpGet]
        public async Task<IActionResult> AssignPM(int? id)
        {
            Project? project = await _btProjectService.GetProjectByIdAsync(id, _companyId);

            if (id == null) { return NotFound(); }

            //Get the list of project managers for the company
            IEnumerable<BTUser> projectManagers = await _btRolesService.GetUsersInRoleAsync(nameof(BTRoles.ProjectManager), _companyId);

            // Get the current PM if on is assigned
            BTUser? currentPM = await _btProjectService.GetProjectManagerAsync(id);

            // Instantiate and Initialize the ViewModel
            AssignPMViewModel vm = new()
            {
                ProjectId = project.Id,
                ProjectName = project.Name,
                PMList = new SelectList(projectManagers, "Id", "FullName", currentPM?.Id),
                PMId = currentPM?.Id,
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignPM(AssignPMViewModel vm)
        {
            if (!string.IsNullOrEmpty(vm.PMId))
            {
                if (await _btProjectService.AddProjectManagerAsync(vm.PMId, vm.ProjectId))
                {
                    return RedirectToAction(nameof(Details), new { id = vm.ProjectId });
                }
                else
                {
                    ModelState.AddModelError("PMId", "Error assigning PM");
                    return View(vm);
                }
            }

            ModelState.AddModelError("PMId", "No Project Manager chosen. Please select a PM");
            IEnumerable<BTUser> projectManagers = await _btRolesService.GetUsersInRoleAsync(nameof(BTRoles.ProjectManager), _companyId);
            BTUser? currentPM = await _btProjectService.GetProjectManagerAsync(vm.ProjectId);
            vm.PMList = new SelectList(projectManagers, "Id", "Name", currentPM?.Id);
            
            return View(vm);


        }

        //Assign project member Methods
        [HttpGet]
        public async Task<IActionResult> AssignProjectMembers(int? id)
        {
            Project? project = await _btProjectService.GetProjectByIdAsync(id, _companyId);

            if (id == null) { return NotFound(); }

            List<BTUser> members = new();
            //Get the list of project managers for the company
            IEnumerable<BTUser> projectDevelopers = await _btRolesService.GetUsersInRoleAsync(nameof(BTRoles.Developer), _companyId);
            IEnumerable<BTUser> projectSubmitters = await _btRolesService.GetUsersInRoleAsync(nameof(BTRoles.Submitter), _companyId);
            foreach (var projectDeveloper in projectDevelopers)
            {
                members.Add(projectDeveloper);
            }
            foreach (var projectSubmitter in projectSubmitters)
            {
                members.Add(projectSubmitter);
            }
            // Get the current PM if on is assigned
            BTUser? currentPM = await _btProjectService.GetProjectManagerAsync(id);

            // Instantiate and Initialize the ViewModel
            AssignMembersVM vm = new()
            {
                ProjectId = project.Id,
                ProjectName = project.Name,
                MembersList = new MultiSelectList(members, "Id", "FullName", currentPM?.Id),
                PMId = currentPM?.Id,
            };
            ViewData["Members"] = new MultiSelectList(_context.Members, "Id", "Name", vm.PMId);
            return View(vm);
        }

        //AssignPM Methods
        [HttpPost]
        public async Task<IActionResult> AssignProjectMembers(AssignMembersVM vm, IEnumerable<string> selected)
        {
            Project? project = await _btProjectService.GetProjectByIdAsync(vm.ProjectId, _companyId);

            if(selected != null)
            {
                // Remove the current selected members first
                await _btProjectService.RemoveMembersFromProjectAsync(vm.ProjectId, _companyId);
                // Add newly selected members
                await _btProjectService.AddMembersToProjectAsync(selected, vm.ProjectId, _companyId);

                return RedirectToAction(nameof(Details), new { id = vm.ProjectId });
            }
            

            ModelState.AddModelError("PMId", "No Project Manager chosen. Please select a PM");
            List<BTUser> members = new();
            IEnumerable<BTUser> projectDevelopers = await _btRolesService.GetUsersInRoleAsync(nameof(BTRoles.Developer), _companyId);
            IEnumerable<BTUser> projectSubmitters = await _btRolesService.GetUsersInRoleAsync(nameof(BTRoles.Submitter), _companyId);
            foreach (var projectDeveloper in projectDevelopers)
            {
                members.Add(projectDeveloper);
            }
            foreach (var projectSubmitter in projectSubmitters)
            {
                members.Add(projectSubmitter);
            }
            BTUser? currentPM = await _btProjectService.GetProjectManagerAsync(vm.ProjectId);
            vm.MembersList = new MultiSelectList(members, "Id", "Name", currentPM?.Id);
            ViewData["Members"] = new MultiSelectList(_context.Members, "Id", "Name", vm.PMId);

            return View();

        }

        [HttpGet]
        public async Task<IActionResult> RemoveAllTeamMembers(int projectId)
        {
            Project? project = await _btProjectService.GetProjectByIdAsync(projectId, _companyId);
            if (project == null)
            {
                return NotFound();
            }

            List<BTUser> members = new();
            IEnumerable<BTUser> projectDevelopers = await _btRolesService.GetUsersInRoleAsync(nameof(BTRoles.Developer), _companyId);
            IEnumerable<BTUser> projectSubmitters = await _btRolesService.GetUsersInRoleAsync(nameof(BTRoles.Submitter), _companyId);
            foreach (var projectDeveloper in projectDevelopers)
            {
                members.Add(projectDeveloper);
            }
            foreach (var projectSubmitter in projectSubmitters)
            {
                members.Add(projectSubmitter);
            }
            BTUser? currentPM = await _btProjectService.GetProjectManagerAsync(projectId);
            
            ViewData["Members"] = new MultiSelectList(_context.Members, "Id", "Name", currentPM?.Id);

            return View();
            
            
        }

        [HttpPost]
        public async Task<IActionResult>RemoveAllTeamMembers(AssignMembersVM vm, IEnumerable<string> selected)
        {
            Project? project = await _btProjectService.GetProjectByIdAsync(vm.ProjectId, _companyId);

            if (selected != null)
            {
                // Remove the current selected members first
                await _btProjectService.RemoveMembersFromProjectAsync(vm.ProjectId, _companyId);
                

                return RedirectToAction(nameof(Details), new { id = vm.ProjectId });
            }
            ViewData["Members"] = new MultiSelectList(_context.Members, "Id", "Name", vm.PMId);
            return RedirectToAction(nameof(Details), new { id = vm.ProjectId });
        }

        private bool ProjectExists(int id)
        {
            return (_context.Projects?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}

