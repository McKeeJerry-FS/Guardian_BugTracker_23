using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Guardian_BugTracker_23.Data;
using Guardian_BugTracker_23.Models;
using Guardian_BugTracker_23.Extensions;
using Guardian_BugTracker_23.Models.ViewModels;
using Guardian_BugTracker_23.Services;
using Microsoft.AspNetCore.Identity;
using Guardian_BugTracker_23.Services.Interfaces;
using Guardian_BugTracker_23.Data.Enums;

namespace Guardian_BugTracker_23.Controllers
{
    public class TicketsController : BTBaseController
    {
        private readonly ApplicationDbContext _context;
        private readonly IBTFileService _bTFileService;
        private readonly IBTTicketService _btTicketService;
        private readonly IBTRolesService _btrolesService;
        private readonly UserManager<BTUser> _userManager;

        public TicketsController(ApplicationDbContext context,
                                 IBTFileService bTFileService,
                                 IBTTicketService bTTicketService,
                                 UserManager<BTUser> userManager,
                                 IBTRolesService bTRolesService)
        {
            _context = context;
            _bTFileService = bTFileService;
            _btTicketService = bTTicketService;
            _btrolesService = bTRolesService;
            _userManager = userManager;
        }

        // GET: Tickets
        public async Task<IActionResult> Index()
        {
            List<Ticket> tickets = await _btTicketService.GetAllTicketsByCompanyIdAsync(_companyId);
            return View(tickets);
        }

        // Get Method completed and working: Dec. 7, 2023
        [HttpGet]
        public async Task<IActionResult> AssignTicketDeveloper(int? id)
        {
			List<BTUser> members = new();
            if(id != null)
            {
                Ticket ticket = await _btTicketService.GetTicketByIdAsync(id, _companyId);
			    //Get the list of developers for the company
			    IEnumerable<BTUser> projectDevelopers = await _btrolesService.GetUsersInRoleAsync(nameof(BTRoles.Developer), _companyId);
			    foreach (var projectDeveloper in projectDevelopers)
			    {
				    members.Add(projectDeveloper);
			    }
                BTUser? currentDev = await _btTicketService.GetTicketDeveloperAsync(id, _companyId);
                AssignTicketVM vm = new()
                {
                    Ticket = ticket,
                    Developers = new SelectList(members, "Id", "FullName"),
                    DeveloperId = currentDev?.Id
                };
                ViewData["Developers"] = new SelectList(members, "Id", "Name");
                return View(vm);
            }
			ViewData["Developers"] = new SelectList(members, "Id", "Name");
			return View();

		}


        [HttpPost]
        public async Task<IActionResult> AssignTicketDeveloper(int? id, string? developerId)
        {
            // Get Ticket info
            Ticket? ticket = await _btTicketService.GetTicketByIdAsync(id, _companyId);
            // Check if the developer exists
            if(developerId != null)
            {
                await _btTicketService.AssignTicketAsync(ticket.Id, developerId);
                return RedirectToAction(nameof(Details), "Tickets", new { id = ticket.Id });
            }

            

            // Add the developer
            
            // Return to the Details View

            return View();                                                                                                                                        
        }

        // GET: Tickets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            // New Identity Extention in USE
            int companyId = User.Identity!.GetCompanyId();

            var ticket = await _btTicketService.GetTicketByIdAsync(id, _companyId);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // GET: Tickets/Create
        public IActionResult Create()
        {
            ViewData["TicketPriority"] = new SelectList(_context.TicketPriorities, "Id", "Name");
            ViewData["TicketStatus"] = new SelectList(_context.TicketStatuses, "Id", "Name");
            ViewData["TicketType"] = new SelectList(_context.TicketTypes, "Id", "Name");
            ViewData["DeveloperUserId"] = new SelectList(_context.BTUsers, "Id", "Id");
            ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Name");
            ViewData["SubmitterUserId"] = new SelectList(_context.BTUsers, "Id", "Id");
            return View();
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,Created,Updated,Archived,ArchivedByProject,TicketType,TicketStatus,TicketPriority,ProjectId,DeveloperUserId,SubmitterUserId")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                await _btTicketService.AddTicketAsync(ticket);
                return RedirectToAction(nameof(Index));
            }
            
            ViewData["TicketPriority"] = new SelectList(_context.TicketPriorities, "Id", "Name", ticket.TicketPriority);
            ViewData["TicketStatus"] = new SelectList(_context.TicketStatuses, "Id", "Name", ticket.TicketStatus);
            ViewData["TicketType"] = new SelectList(_context.TicketTypes, "Id", "Name", ticket.TicketType);
            ViewData["DeveloperUserId"] = new SelectList(_context.BTUsers, "Id", "Id", ticket.DeveloperUserId);
            ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Name", ticket.ProjectId);
            ViewData["SubmitterUserId"] = new SelectList(_context.BTUsers, "Id", "Id", ticket.SubmitterUserId);
            return View(ticket);
        }

        // GET: Tickets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {


            var ticket = await _btTicketService.GetTicketByIdAsync(id, _companyId);
            if (ticket == null)
            {
                return NotFound();
            }
            ViewData["TicketPriority"] = new SelectList(_context.TicketPriorities, "Id", "Name", ticket.TicketPriority);
            ViewData["TicketStatus"] = new SelectList(_context.TicketStatuses, "Id", "Name", ticket.TicketStatus);
            ViewData["TicketType"] = new SelectList(_context.TicketTypes, "Id", "Name", ticket.TicketType);
            ViewData["DeveloperUserId"] = new SelectList(_context.BTUsers, "Id", "Id", ticket.DeveloperUserId);
            ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Name", ticket.ProjectId);
            ViewData["SubmitterUserId"] = new SelectList(_context.BTUsers, "Id", "Id", ticket.SubmitterUserId);
            return View(ticket);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,Created,Updated,Archived,ArchivedByProject,TicketType,TicketStatus,TicketPriority,ProjectId,DeveloperUserId,SubmitterUserId")] Ticket ticket)
        {
            if (id != ticket.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _btTicketService.UpdateTicketAsync(ticket);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TicketExists(ticket.Id))
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
            ViewData["TicketPriority"] = new SelectList(_context.TicketPriorities, "Id", "Name", ticket.TicketPriority);
            ViewData["TicketStatus"] = new SelectList(_context.TicketStatuses, "Id", "Name", ticket.TicketStatus);
            ViewData["TicketType"] = new SelectList(_context.TicketTypes, "Id", "Name", ticket.TicketType);
            ViewData["DeveloperUserId"] = new SelectList(_context.BTUsers, "Id", "Id", ticket.DeveloperUserId);
            ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Name", ticket.ProjectId);
            ViewData["SubmitterUserId"] = new SelectList(_context.BTUsers, "Id", "Id", ticket.SubmitterUserId);
            return View(ticket);
        }

        // GET: Tickets/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {

            var ticket = await _btTicketService.GetTicketByIdAsync(id, _companyId);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            
            var ticket = await _btTicketService.GetTicketByIdAsync(id, _companyId);
            if (ticket != null)
            {
                await _btTicketService.ArchiveTicketAsync(ticket);
            }
           
            return RedirectToAction(nameof(Index));
        }

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> AddTicketAttachment([Bind("Id,FormFile,Description,TicketId")] TicketAttachment ticketAttachment)
		{
			string statusMessage;
            ModelState.Remove(nameof(TicketAttachment.BTUserId));
            ModelState.Remove(nameof(TicketAttachment.FileName));

			if (ModelState.IsValid && ticketAttachment.FormFile != null)
			{
				ticketAttachment.FileData = await _bTFileService.ConvertFileToByteArrayAsync(ticketAttachment.FormFile);
				ticketAttachment.FileName = ticketAttachment.FormFile.FileName;
				ticketAttachment.FileType = ticketAttachment.FormFile.ContentType;

				ticketAttachment.Created = DateTimeOffset.Now;
				ticketAttachment.BTUserId = _userManager.GetUserId(User);

				await _btTicketService.AddTicketAttachmentAsync(ticketAttachment);
				statusMessage = "Success: New attachment added to Ticket.";
			}
			else
			{
				statusMessage = "Error: Invalid data.";

			}

			return RedirectToAction("Details", new { id = ticketAttachment.TicketId, message = statusMessage });
		}

		public async Task<IActionResult> ShowFile(int id)
		{
			TicketAttachment ticketAttachment = await _btTicketService.GetTicketAttachmentByIdAsync(id);
			string fileName = ticketAttachment.FileName!;
			byte[] fileData = ticketAttachment.FileData!;
			string ext = Path.GetExtension(fileName)!.Replace(".", "");

			Response.Headers.Add("Content-Disposition", $"inline; filename={fileName}");
			return File(fileData!, $"application/{ext}");
		}

        [HttpGet]
        public async Task<IActionResult> Restore(int? id)
        {
			
            var ticket = await _btTicketService.GetTicketByIdAsync(id, _companyId);
			if (ticket == null)
			{
				return NotFound();
			}

			return View(ticket);
		}

        [HttpPost, ActionName("Restore")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RestoreConfirmed(int? id)
        {
			
			var ticket = await _btTicketService.GetTicketByIdAsync(id, _companyId);
			if (ticket != null)
			{
				await _btTicketService.RestoreTicketAsync(ticket);
			}

			return RedirectToAction(nameof(Index));
		}

		private bool TicketExists(int id)
        {
          return (_context.Tickets?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
