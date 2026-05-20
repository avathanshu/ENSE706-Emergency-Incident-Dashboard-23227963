// OOP: Encapsulation -- a Notification is a plain data object
// Keeping this separate from StaffMember means notification data can be logged,
// queued, or replayed without touching the staff class at all.

using System;

namespace EIRS
{
    public class Notification
    {
        public string   NotificationId { get; private set; }
        public string   RecipientId    { get; private set; }
        public string   Message        { get; private set; }
        public string   Channel        { get; private set; } // "InApp", "Email", "SMS"
        public DateTime SentAt         { get; private set; }
        public bool     Acknowledged   { get; private set; }

        public Notification(string notificationId, string recipientId, string message, string channel)
        {
            NotificationId = notificationId;
            RecipientId    = recipientId;
            Message        = message;
            Channel        = channel;
            SentAt         = DateTime.Now;
            Acknowledged   = false;
        }

        // Called when a staff member accepts or declines -- records the response
        public void Acknowledge()
        {
            Acknowledged = true;
            Console.WriteLine($"[ACK] Notification {NotificationId} acknowledged by {RecipientId} at {DateTime.Now:HH:mm:ss}");
        }

        public void Print()
        {
            Console.WriteLine($"[NOTIFICATION {NotificationId}] To: {RecipientId} | Channel: {Channel} | Msg: {Message}");
        }
    }
}
