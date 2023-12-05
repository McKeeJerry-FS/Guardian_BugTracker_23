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

namespace Guardian_BugTracker_23.Controllers
{
    public class TicketsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IBTFileService _bTFileService;
        private readonly IBTTicketService _btTicketService;
        private readonly UserManager<BTUser> _userManager;

        public TicketsController(ApplicationDbContext context,
                                 IBTFileService bTFileService,
                                 IBTTicketService bTTicketService,
                                 UserManager<BTUser> userManager)
        {
            _context = context;
            _bTFileService = bTFileService;
            _btTicketService = bTTicketService;
            _userManager = userManager;
        }

        // GET: Tickets
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Tickets.Include(t => t.DeveloperUser).Include(t => t.Project).Include(t => t.SubmitterUser);
            return View(await applicationDbContext.ToListAsync());
        }

        public async Task<IActionResult> AssignTicket(int? id)
        {
            AssignTicketVM vm = new()
            {
                
            };

            return View(vm);                                                                                                                                        
        }

        // GET: Tickets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Tickets == null)
            {
                return NotFound();
            }

            // New Identity Extention in USE
            int companyId = User.Identity!.GetCompanyId();

            var ticket = await _context.Tickets
                .Include(t => t.DeveloperUser)
                .Include(t => t.Project)
                .Include(t => t.SubmitterUser)
                .Include(t => t.Attachments)
                .Include(t => t.Comments)
                .FirstOrDefaultAsync(m => m.Id == id);
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
                _context.Add(ticket);
                await _context.SaveChangesAsync();
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
            if (id == null || _context.Tickets == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets.FindAsync(id);
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
                    _context.Update(ticket);
                    await _context.SaveChangesAsync();
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
            if (id == null || _context.Tickets == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets
                .Include(t => t.DeveloperUser)
                .Include(t => t.Project)
                .Include(t => t.SubmitterUser)
                .FirstOrDefaultAsync(m => m.Id == id);
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
            if (_context.Tickets == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Tickets'  is null.");
            }
            var ticket = await _context.Tickets.FindAsync(id);
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
			if (id == null || _context.Tickets == null)
			{
				return NotFound();
			}

			var ticket = await _context.Tickets
                .Where(t => t.Archived == true)
				.Include(t => t.DeveloperUser)
				.Include(t => t.Project)
				.Include(t => t.SubmitterUser)
				.FirstOrDefaultAsync(m => m.Id == id);
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
			if (_context.Tickets == null)
			{
				return Problem("Entity set 'ApplicationDbContext.Tickets'  is null.");
			}
			var ticket = await _context.Tickets.FindAsync(id);
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
