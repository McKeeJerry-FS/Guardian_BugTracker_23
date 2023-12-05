using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Guardian_BugTracker_23.Data;
using Guardian_BugTracker_23.Models;
using Microsoft.AspNetCore.Identity;
using Guardian_BugTracker_23.Services.Interfaces;

namespace Guardian_BugTracker_23.Controllers
{
    public class TicketCommentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<BTUser> _userManager;
        private readonly ILogger<TicketCommentsController> _logger;
        private readonly IBTTicketService _ticketService;

        public TicketCommentsController(ApplicationDbContext context,
                                        UserManager<BTUser> userManager,
                                        ILogger<TicketCommentsController> logger,
                                        IBTTicketService bTTicketService)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
            _ticketService = bTTicketService;
        }

        // GET: TicketComments
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Comments.Include(t => t.Ticket).Include(t => t.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: TicketComments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Comments == null)
            {
                return NotFound();
            }

            var ticketComment = await _context.Comments
                .Include(t => t.Ticket)
                .Include(t => t.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ticketComment == null)
            {
                return NotFound();
            }

            return View(ticketComment);
        }

        // GET: TicketComments/Create
        public IActionResult Create()
        {

            ViewData["TicketId"] = new SelectList(_context.Tickets, "Id", "Description");
            ViewData["UserId"] = new SelectList(_context.BTUsers, "Id", "Id");
            return View();
        }

        // POST: TicketComments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Comment,TicketId")] TicketComment ticketComment)
        {
            

            ModelState.Remove("UserId");
            
            if (ModelState.IsValid)
            {
                ticketComment.UserId = _userManager.GetUserId(User);
                ticketComment.Created = DateTimeOffset.Now;
                
                _context.Add(ticketComment);
                await _context.SaveChangesAsync();
                
            }
            ViewData["TicketId"] = new SelectList(_context.Tickets, "Id", "Description", ticketComment.TicketId);
            ViewData["UserId"] = new SelectList(_context.BTUsers, "Id", "Id", ticketComment.UserId);
            return RedirectToAction("Details", "Tickets", new { Id = ticketComment.TicketId});
        }

        // GET: TicketComments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Comments == null)
            {
                return NotFound();
            }

            var ticketComment = await _context.Comments.FindAsync(id);
            if (ticketComment == null)
            {
                return NotFound();
            }
            ViewData["TicketId"] = new SelectList(_context.Tickets, "Id", "Description", ticketComment.TicketId);
            ViewData["UserId"] = new SelectList(_context.BTUsers, "Id", "Id", ticketComment.UserId);
            return View(ticketComment);
        }

        // POST: TicketComments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Comment,Created,TicketId,UserId")] TicketComment ticketComment)
        {
            if (id != ticketComment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ticketComment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TicketCommentExists(ticketComment.Id))
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
            ViewData["TicketId"] = new SelectList(_context.Tickets, "Id", "Description", ticketComment.TicketId);
            ViewData["UserId"] = new SelectList(_context.BTUsers, "Id", "Id", ticketComment.UserId);
            return View(ticketComment);
        }

        // GET: TicketComments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Comments == null)
            {
                return NotFound();
            }

            var ticketComment = await _context.Comments
                .Include(t => t.Ticket)
                .Include(t => t.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ticketComment == null)
            {
                return NotFound();
            }

            return View(ticketComment);
        }

        // POST: TicketComments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Comments == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Comments'  is null.");
            }
            var ticketComment = await _context.Comments.FindAsync(id);
            if (ticketComment != null)
            {
                _context.Comments.Remove(ticketComment);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TicketCommentExists(int id)
        {
          return (_context.Comments?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
