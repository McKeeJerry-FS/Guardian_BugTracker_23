﻿using Guardian_BugTracker_23.Data.Enums;
using Guardian_BugTracker_23.Models;
using Guardian_BugTracker_23.Services.Interfaces;

namespace Guardian_BugTracker_23.Services
{
    public class BTNotificationService : IBTNotificationService
    {
        public Task AddNotificationAsync(Notification? notification)
        {
            throw new NotImplementedException();
        }

        public Task<List<Notification>> GetNotificationsByUserIdAsync(string? userId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> NewDeveloperNotificationAsync(int? ticketId, string? developerId, string? senderId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> NewTicketNotificationAsync(int? ticketId, string? senderId)
        {
            throw new NotImplementedException();
        }

        public Task NotificationsByRoleAsync(int? companyId, Notification? notification, BTRoles role)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SendEmailNotificationAsync(Notification? notification, string? emailSubject)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SendEmailNotificationByRoleAsync(int? companyId, Notification? notification, BTRoles role)
        {
            throw new NotImplementedException();
        }

        public Task<bool> TicketUpdateNotificationAsync(int? ticketId, string? developerId, string? senderId = null)
        {
            throw new NotImplementedException();
        }
    }
}
