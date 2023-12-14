using Guardian_BugTracker_23.Data;
using Guardian_BugTracker_23.Data.Enums;
using Guardian_BugTracker_23.Models;
using Guardian_BugTracker_23.Services.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NuGet.ProjectModel;
using System.Linq;

namespace Guardian_BugTracker_23.Services
{
    public class BTTicketService : IBTTicketService
    {

        private readonly ILogger<BTTicketService> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<BTUser> _userManager;
        private readonly IBTRolesService _rolesService;
        private readonly IBTProjectService _projectService;

        public BTTicketService(ApplicationDbContext context,
                                ILogger<BTTicketService> logger,
                                UserManager<BTUser> userManager,
                                IBTRolesService bTRolesService,
                                IBTProjectService bTProjectService)
        {
            _context = context;
            _logger = logger;
            _userManager = userManager;
            _rolesService = bTRolesService;
            _projectService = bTProjectService;
        }

        public async Task AddTicketAsync(Ticket? ticket)
        {
            try
            {
                if(ticket != null)
                {
                    _context.Add(ticket);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task AddTicketAttachmentAsync(TicketAttachment? ticketAttachment)
        {
			try
			{
                if(ticketAttachment != null)
                {
				    await _context.AddAsync(ticketAttachment);
				    await _context.SaveChangesAsync();
                }
			}
			catch (Exception)
			{

				throw;
			}
		}

        public Task AddTicketCommentAsync(TicketComment? ticketComment)
        {
            throw new NotImplementedException();
        }

        public async Task ArchiveTicketAsync(Ticket? ticket)
        {
            try
            {
                if (ticket != null)
                {
					ticket.Archived = true;
                    _logger.LogInformation("Ticket successfully archived");
                    await _context.SaveChangesAsync();
				}
			}
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task AssignTicketAsync(int? ticketId, string? userId)
        {
            try
            {
                if(ticketId != null && userId != null)
                {
                    // Get Ticket
                    Ticket? ticket = await _context.Tickets.FirstOrDefaultAsync(t => t.Id == ticketId);
                    // Check for Nulls
                    if (ticket != null)
                    {
                        // Find the user based off the id
                        // BTUser? user = await _context.Users.FindAsync(userId);
                        if(await _context.Users.AnyAsync(u => u.Id == userId)) 
                        { 
                            // assign the user
                            ticket.DeveloperUserId = userId;
                            // save the changes
                            await UpdateTicketAsync(ticket);
                        }
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<Ticket>> GetAllTicketsByCompanyIdAsync(int? companyId)
        {
            try
            {
                IEnumerable<Ticket> result = await _context.Tickets.Where(t => t.Project!.CompanyId == companyId)
                                                                   .Include(t => t.DeveloperUser)
                                                                   .Include(t => t.Project)
                                                                   .Include(t => t.SubmitterUser)
                                                                   .Include(t => t.History)
                                                                   .AsNoTracking()
                                                                   .ToListAsync();
                return result.ToList();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<Ticket>> GetAllTicketsByProjectIdAsync(int? projectId)
        {
            try
            {
                IEnumerable<Ticket> result = await _context.Tickets.Where(t => t.Project!.Id == projectId)
                                                                   .Include(t => t.DeveloperUser)
                                                                   .Include(t => t.Project)
                                                                   .Include(t => t.SubmitterUser)
																   .Include(t => t.History)
																   .AsNoTracking()
                                                                   .ToListAsync();
                return result.ToList();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<Ticket> GetTicketAsNoTrackingAsync(int? ticketId, int? companyId)
        {
            try
            {
                if(ticketId != null && companyId != null)
                {
                    Ticket? ticket =  await _context.Tickets.Include(t => t.DeveloperUser)
                                                    .Include(t => t.Project)
                                                    .Include(t => t.SubmitterUser)
                                                    .Include(t => t.Attachments)
                                                    .Include(t => t.Comments)
												    .Include(t => t.History)
												    .AsNoTracking()
                                                    .FirstOrDefaultAsync(m => m.Id == ticketId && m.Project!.CompanyId == companyId);
                    return ticket!;
                }
                return null!;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<TicketAttachment?> GetTicketAttachmentByIdAsync(int? ticketAttachmentId)
        {
			try
			{
                if(ticketAttachmentId != null)
                {
				    TicketAttachment? ticketAttachment = await _context.Attachments
																      .Include(t => t.BTUser)
																      .FirstOrDefaultAsync(t => t.Id == ticketAttachmentId);
				    return ticketAttachment;
                }
                return null;
			}
			catch (Exception)
			{

				throw;
			}
		}

        public async Task<Ticket> GetTicketByIdAsync(int? ticketId, int? companyId)
        {
            try
            {
                if(ticketId != null && companyId != null)
                {
                    Ticket? ticket =  await _context.Tickets.Include(t => t.DeveloperUser)
                                                    .Include(t => t.Project)
                                                    .Include(t => t.SubmitterUser)
                                                    .Include(t => t.Attachments)
                                                    .Include(t => t.Comments)
												    .Include(t => t.History)
                                                    .ThenInclude(h => h.User)
												    .FirstOrDefaultAsync(m => m.Id == ticketId && m.Project!.CompanyId == companyId);
                    return ticket!;
                }
                return null!;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<BTUser?> GetTicketDeveloperAsync(int? ticketId, int? companyId)
        {
            if(ticketId != null && companyId != null)
            {
                Ticket? ticket = await _context.Tickets.FirstOrDefaultAsync(t => t.Id == ticketId);
                if(ticket!.DeveloperUser != null)
                {
                    return ticket.DeveloperUser;
                }
            }
            return null;
        }

        public Task<IEnumerable<TicketPriority>> GetTicketPrioritiesAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<List<Ticket>> GetTicketsByUserIdAsync(string? userId, int? companyId)
        {
            
            try
            {
                List<Ticket> tickets = new();
                if (!string.IsNullOrEmpty(userId) && companyId != null)
                {
                    BTUser? btUser = await _context.Users.Include(u => u.Projects).ThenInclude(p => p.Tickets).FirstOrDefaultAsync(u => u.Id == userId);
                    tickets = (await GetAllTicketsByCompanyIdAsync(companyId))
                                    .Where(t => !(t.Archived |t.ArchivedByProject ))  // <-- Binary "OR" LINQ Statement
                                    .ToList();
                    try
                    {
                        if (await _rolesService.IsUserInRoleAsync(btUser, nameof(BTRoles.Admin)))
                        {
                            return tickets;
                        }
                        else if(await _rolesService.IsUserInRoleAsync(btUser, nameof(BTRoles.Developer)))
                        {
                            return tickets.Where(t => t.DeveloperUserId == userId || t.SubmitterUserId == userId).ToList();
                        }
                        else if (await _rolesService.IsUserInRoleAsync(btUser, nameof(BTRoles.Submitter)))
                        {
                            return tickets.Where(t => t.SubmitterUserId == userId).ToList();
                        }
                        else if (await _rolesService.IsUserInRoleAsync(btUser, nameof(BTRoles.ProjectManager)))
                        {
                            List<Ticket> projectTickets = (btUser!.Projects.SelectMany(p => p.Tickets).ToList()!)
                                                                           .Where(t => !(t.Archived | t.ArchivedByProject))
                                                                           .ToList();
                            return projectTickets;
                        }
                        
                    }
                    catch (Exception)
                    {

                        throw;
                    }

                                    
                    return tickets;
                }
                return tickets;
            }
            catch (Exception)
            {

                throw;
            }
		}

        public Task<IEnumerable<TicketStatus>> GetTicketStatusesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TicketType>> GetTicketTypesAsync()
        {
            throw new NotImplementedException();
        }

        public async Task RestoreTicketAsync(Ticket? ticket)
        {
			try
			{
				if (ticket != null)
				{
					ticket.Archived = false;
					_logger.LogInformation("Ticket successfully restored");
					await _context.SaveChangesAsync();
				}
			}
			catch (Exception ex)
			{
                _logger.LogError($"Error: {ex} : {DateTimeOffset.Now: MMM dd, yyyy - HH:mm}");
				throw;
			}
		}

        public async Task UpdateTicketAsync(Ticket? ticket)
        {
            try
            {
                if(ticket != null)
                {
                    ticket.Updated = DateTimeOffset.Now;
                    _context.Update(ticket);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
