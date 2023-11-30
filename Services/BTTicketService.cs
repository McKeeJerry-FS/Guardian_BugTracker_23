using Guardian_BugTracker_23.Data;
using Guardian_BugTracker_23.Models;
using Guardian_BugTracker_23.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Guardian_BugTracker_23.Services
{
    public class BTTicketService : IBTTicketService
    {

        private readonly ILogger<BTTicketService> _logger;
        private readonly ApplicationDbContext _context;

        public BTTicketService(ApplicationDbContext context,
                                ILogger<BTTicketService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public Task AddTicketAsync(Ticket? ticket)
        {
            throw new NotImplementedException();
        }

        public Task AddTicketAttachmentAsync(TicketAttachment? ticketAttachment)
        {
            throw new NotImplementedException();
        }

        public Task AddTicketCommentAsync(TicketComment? ticketComment)
        {
            throw new NotImplementedException();
        }

        public Task ArchiveTicketAsync(Ticket? ticket)
        {
            throw new NotImplementedException();
        }

        public Task AssignTicketAsync(int? ticketId, string? userId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Ticket>> GetAllTicketsByCompanyIdAsync(int? companyId)
        {
            try
            {
                IEnumerable<Ticket> result = await _context.Tickets.Where(t => t.Project!.CompanyId == companyId)
                                                                   .Include(t => t.DeveloperUser)
                                                                   .Include(t => t.Project)
                                                                   .Include(t => t.SubmitterUser)
                                                                   .ToListAsync();
                return result.ToList();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public Task<Ticket> GetTicketAsNoTrackingAsync(int? ticketId, int? companyId)
        {
            throw new NotImplementedException();
        }

        public Task<TicketAttachment?> GetTicketAttachmentByIdAsync(int? ticketAttachmentId)
        {
            throw new NotImplementedException();
        }

        public Task<Ticket> GetTicketByIdAsync(int? ticketId, int? companyId)
        {
            throw new NotImplementedException();
        }

        public Task<BTUser?> GetTicketDeveloperAsync(int? ticketId, int? companyId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TicketPriority>> GetTicketPrioritiesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<Ticket>> GetTicketsByUserIdAsync(string? userId, int? companyId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TicketStatus>> GetTicketStatusesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TicketType>> GetTicketTypesAsync()
        {
            throw new NotImplementedException();
        }

        public Task RestoreTicketAsync(Ticket? ticket)
        {
            throw new NotImplementedException();
        }

        public Task UpdateTicketAsync(Ticket? ticket)
        {
            throw new NotImplementedException();
        }
    }
}
