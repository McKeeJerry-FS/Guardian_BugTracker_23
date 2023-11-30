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
using Microsoft.AspNetCore.Authorization;
using Guardian_BugTracker_23.Models.ViewModels;
using Guardian_BugTracker_23.Extensions;

namespace Guardian_BugTracker_23.Controllers
{
    public class CompaniesController : BTBaseController
    {
        private readonly ApplicationDbContext _context;
        private readonly IBTCompanyService _companyService;
        private readonly IBTRolesService _rolesService;
        private readonly ILogger<CompaniesController> _logger;

        public CompaniesController(ApplicationDbContext context,
                                   ILoggerFactory logger,
                                   IBTCompanyService bTCompanyService,
                                   IBTRolesService bTRolesService)
        {
            _context = context;
            _logger = logger.CreateLogger<CompaniesController>();
            _companyService = bTCompanyService;
            _rolesService = bTRolesService;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> ManageUserRoles()
        {
            // Add an instance of the ViewModel as a list
            List<ManageUserRolesVM> model = new List<ManageUserRolesVM>();
            // Get CompanyId => because of the base controller you don't need the intermediate variable
            int companyId = _companyId;
            // Get all company users
            
            IEnumerable<BTUser> members = await _companyService.GetMembersAsync(companyId);
            // Loop over the users to populate the model
            // - instantiate the ManageUserRolesVM
            // - use _roleService to get user's roles
            // - Assign a multiselect of roles a user
            // - Add user's viewmodel to model (List<ManageUserRolesVM>)
            
            foreach(BTUser user in members)
            {
                if(string.Compare(user.Id, _userId) != 0)
                {
                    ManageUserRolesVM vm = new();
                    IEnumerable<string>? currentRoles = await _rolesService.GetUserRolesAsync(user);
                    vm.BTUser = user;
                    vm.Roles = new MultiSelectList(await _rolesService.GetRolesAsync(), "Name", "Name", currentRoles);

                    model.Add(vm);
                }
            }
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> ManageUserRoles(ManageUserRolesVM vm)
        {
            // Get the company id
            // Instantiate the BTUser
            BTUser? btUser = (await _companyService.GetMembersAsync(_companyId)).FirstOrDefault(b => b.Id == vm.BTUser?.Id);

            // Get Roles for the User
            IEnumerable<string>? currentRoles = await _rolesService.GetUserRolesAsync(btUser);
            // Get selected roles for the BTUser
            string? selectedRole = vm.SelectedRoles?.FirstOrDefault();
            // Remove currentRoles; Add new roles
            if (!string.IsNullOrEmpty(selectedRole))
            {
                if (await _rolesService.RemoveUserFromRolesAsync(btUser, currentRoles))
                {
                    await _rolesService.AddUserToRoleAsync(btUser, selectedRole);
                    _logger.LogInformation(message: $"User successfully added to role: {DateTimeOffset.Now:MM dd, yyyy - HH:mm}") ;
                }
            }
            // Navigate
            return RedirectToAction(nameof(ManageUserRoles));
        }

        // GET: Companies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Companies == null)
            {
                return NotFound();
            }

            var company = await _context.Companies
                .FirstOrDefaultAsync(m => m.Id == id);
            if (company == null)
            {
                return NotFound();
            }

            return View(company);
        }

        // GET: Companies/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Companies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,ImageFileData,ImageFileType")] Company company)
        {
            if (ModelState.IsValid)
            {
                _context.Add(company);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(company);
        }

        // GET: Companies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Companies == null)
            {
                return NotFound();
            }

            var company = await _context.Companies.FindAsync(id);
            if (company == null)
            {
                return NotFound();
            }
            return View(company);
        }

        // POST: Companies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,ImageFileData,ImageFileType")] Company company)
        {
            if (id != company.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(company);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CompanyExists(company.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(company);
        }

        // GET: Companies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Companies == null)
            {
                return NotFound();
            }

            var company = await _context.Companies
                .FirstOrDefaultAsync(m => m.Id == id);
            if (company == null)
            {
                return NotFound();
            }

            return View(company);
        }

        // POST: Companies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Companies == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Companies'  is null.");
            }
            var company = await _context.Companies.FindAsync(id);
            if (company != null)
            {
                _context.Companies.Remove(company);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CompanyExists(int id)
        {
          return (_context.Companies?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
