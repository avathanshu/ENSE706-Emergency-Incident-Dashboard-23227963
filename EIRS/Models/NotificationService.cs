// OOP: Singleton pattern -- only one NotificationService should exist in the system
// Previously, notification logic lived inside StaffMember.ReceiveNotification().
// Pulling it here means all notification dispatch is in one place.
// New channels (SMS, email) can be added here without touching any other class.

using System;
using System.Collections.Generic;

namespace EIRS
{
    public class NotificationService
    {
        // Singleton instance -- private so nothing outside can create a second one
        private static NotificationService? _instance;
        private static readonly object _lock = new object();

        private List<Notification> _sentNotifications = new();
        private int _counter = 1;

        // Private constructor prevents direct instantiation
        private NotificationService() { }

        // Thread-safe access to the single instance
        public static NotificationService Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance == null)
                        _instance = new NotificationService();
                    return _instance;
                }
            }
        }

        // Sends a notification to a staff member and logs it internally
        public Notification Send(string recipientId, string message, string channel = "InApp")
        {
            var notification = new Notification($"N{_counter++}", recipientId, message, channel);
            _sentNotifications.Add(notification);
            notification.Print();
            return notification;
        }

        // Sends the same notification to multiple recipients at once
        public void Broadcast(List<string> recipientIds, string message, string channel = "InApp")
        {
            foreach (var id in recipientIds)
                Send(id, message, channel);
        }

        // Returns all notifications sent to a specific recipient
        public List<Notification> GetHistoryFor(string recipientId)
        {
            return _sentNotifications.FindAll(n => n.RecipientId == recipientId);
        }
    }
}
