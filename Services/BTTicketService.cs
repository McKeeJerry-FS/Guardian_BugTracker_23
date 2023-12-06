﻿using Guardian_BugTracker_23.Data;
using Guardian_BugTracker_23.Models;
using Guardian_BugTracker_23.Services.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using NuGet.ProjectModel;

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
				_context.Add(ticketAttachment);
				await _context.SaveChangesAsync();
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
                Ticket? ticket = await _context.Tickets.FirstOrDefaultAsync(t => t.Id == ticketId);
                if (userId != null)
                {
                    
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
                                                                   .AsNoTracking()
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

        public async Task<TicketAttachment?> GetTicketAttachmentByIdAsync(int? ticketAttachmentId)
        {
			try
			{
				TicketAttachment ticketAttachment = await _context.Attachments
																  .Include(t => t.BTUser)
																  .FirstOrDefaultAsync(t => t.Id == ticketAttachmentId);
				return ticketAttachment;
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
                return await _context.Tickets.Include(t => t.DeveloperUser)
                                                .Include(t => t.Project)
                                                .Include(t => t.SubmitterUser)
                                                .Include(t => t.Attachments)
                                                .Include(t => t.Comments)
                                                .FirstOrDefaultAsync(m => m.Id == ticketId);
            }
            catch (Exception ex)
            {

                throw;
            }
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
