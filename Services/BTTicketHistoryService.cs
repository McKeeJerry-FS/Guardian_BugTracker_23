using Guardian_BugTracker_23.Data;
using Guardian_BugTracker_23.Data.Enums;
using Guardian_BugTracker_23.Models;
using Guardian_BugTracker_23.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Guardian_BugTracker_23.Services
{
    public class BTTicketHistoryService : IBTTicketHistoryService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<BTTicketHistoryService> _logger;

        public BTTicketHistoryService(ApplicationDbContext context, 
                                      ILogger<BTTicketHistoryService> logger)
        {
            _context = context;
            _logger = logger;

        }


        public async Task AddHistoryAsync(Ticket? oldTicket, Ticket? newTicket, string? userId)
        {
            // NEW Ticket has been added
            if (oldTicket == null && newTicket != null)
            {
                TicketHistory ticketHistory = new()
                {
                    TicketId = newTicket.Id,
                    PropertyName = "",
                    OldValue = "",
                    NewValue = "",
                    Created = DateTimeOffset.Now,
                    UserId = userId,
                    Description = "New Ticket Created"
                };
                try
                {
                    _logger.LogInformation($"New ticket created {DateTimeOffset.Now}");
                    await _context.History.AddAsync(ticketHistory);
                    await _context.SaveChangesAsync();
                }
                catch (Exception)
                {
                    _logger.LogError("Error in creating new ticket");
                    throw;
                }
            }
            else
            {
                // Check Ticket Title
                if (oldTicket?.Title != newTicket?.Title)
                {
                    TicketHistory? history = new()
                    {
                        TicketId = newTicket!.Id,
                        PropertyName = "Title",
                        OldValue = oldTicket?.Title,
                        NewValue = newTicket?.Title,
                        Created = DateTime.UtcNow,
                        UserId = userId,
                        Description = $"New ticket title: {newTicket?.Title}"
                    };
                    await _context.History.AddAsync(history);
                }

                //Check Ticket Description
                if (oldTicket?.Description != newTicket?.Description)
                {
                    TicketHistory? history = new()
                    {
                        TicketId = newTicket!.Id,
                        PropertyName = "Description",
                        OldValue = oldTicket?.Description,
                        NewValue = newTicket?.Description,
                        Created = DateTime.UtcNow,
                        UserId = userId,
                        Description = $"New ticket description: {newTicket?.Description}"
                    };
                    await _context.History.AddAsync(history);
                }

                //Check Ticket Priority
                if (oldTicket?.TicketPriority != newTicket?.TicketPriority)
                {
                    TicketHistory? history = new()
                    {
                        TicketId = newTicket!.Id,
                        PropertyName = "TicketPriority",
                        OldValue = Enum.GetName(oldTicket!.TicketPriority),
                        NewValue = Enum.GetName(newTicket!.TicketPriority),
                        Created = DateTime.UtcNow,
                        UserId = userId,
                        Description = $"New ticket priority: {Enum.GetName(newTicket.TicketPriority)}"
                    };
                    await _context.History.AddAsync(history);
                }

                //Check Ticket Status
                if (oldTicket?.TicketStatus != newTicket?.TicketStatus)
                {
                    TicketHistory? history = new()
                    {
                        TicketId = newTicket!.Id,
                        PropertyName = "TicketStatus",
                        OldValue = Enum.GetName(oldTicket!.TicketStatus),
                        NewValue = Enum.GetName(newTicket!.TicketStatus),
                        Created = DateTime.UtcNow,
                        UserId = userId,
                        Description = $"New ticket Status: {Enum.GetName(oldTicket!.TicketStatus)}"
                    };
                    await _context.History.AddAsync(history);
                }

                //Check Ticket Type
                if (oldTicket?.TicketType != newTicket?.TicketType)
                {
                    TicketHistory? history = new()
                    {
                        TicketId = newTicket!.Id,
                        PropertyName = "TicketTypeId",
                        OldValue = Enum.GetName(oldTicket!.TicketType),
                        NewValue = Enum.GetName(newTicket!.TicketType),
                        Created = DateTime.UtcNow,
                        UserId = userId,
                        Description = $"New ticket Type: {Enum.GetName(newTicket!.TicketType)}"
                    };
                    await _context.History.AddAsync(history);
                }

                //Check Ticket Developer
                if (oldTicket?.DeveloperUserId != newTicket?.DeveloperUserId)
                {
                    TicketHistory? history = new()
                    {
                        TicketId = newTicket!.Id,
                        PropertyName = "Developer",
                        OldValue = oldTicket?.DeveloperUser?.FullName ?? "Not Assigned",
                        NewValue = newTicket?.DeveloperUser?.FullName,
                        Created = DateTime.UtcNow,
                        UserId = userId,
                        Description = $"New ticket developer: {newTicket?.DeveloperUser?.FullName}"

                    };
                    await _context.History.AddAsync(history);
                }
                
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (Exception)
                {

                    throw;
                }
            }
            
        }

        public async Task AddHistoryAsync(int? ticketId, string? model, string? userId)
        {
            try
            {
                if(ticketId != null && model != null && userId != null)
                {

                    Ticket? ticket = await _context.Tickets.FindAsync(ticketId);
                    string description = model.ToLower().Replace("ticket", "");
                    description = $"New {description} added to ticket: {ticket!.Title}";


                    TicketHistory? history = new()
                    {
                        TicketId = ticket.Id,
                        PropertyName = model,
                        OldValue = "",
                        NewValue = "",
                        Created = DateTime.UtcNow,
                        UserId = userId,
                        Description = description
                    };

                    await _context.History.AddAsync(history);
                    await _context.SaveChangesAsync();
                }

            }
            catch (Exception)
            {

                throw;
            }
        }


        public async Task<List<TicketHistory>> GetCompanyTicketsHistoriesAsync(int? companyId)
        {
            try
            {
                List<Project> projects = (await _context.Companies
                                                        .Include(c => c.Projects)
                                                            .ThenInclude(p => p.Tickets)
                                                                .ThenInclude(t => t.History)
                                                                    .ThenInclude(h => h.User)
                                                        .FirstOrDefaultAsync(c => c.Id == companyId))!.Projects.ToList();

                List<Ticket> tickets = projects.SelectMany(p => p.Tickets).ToList();

                List<TicketHistory> ticketHistories = tickets.SelectMany(t => t.History).ToList();

                return ticketHistories;
            }
            catch (Exception)
            {

                throw;
            }

        }
    
        public async Task<List<TicketHistory>> GetProjectTicketsHistoriesAsync(int? projectId, int? companyId)
        {
            try
            {
                Project? project = await _context.Projects.Where(p => p.CompanyId == companyId)
                                                         .Include(p => p.Tickets)
                                                            .ThenInclude(t => t.History)
                                                                .ThenInclude(h => h.User)
                                                         .FirstOrDefaultAsync(p => p.Id == projectId);

                List<TicketHistory> ticketHistory = project!.Tickets.SelectMany(t => t.History).ToList();

                return ticketHistory;
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
    
}
