// OOP: Encapsulation -- bundles a timestamped event log for one incident
// FR6 requires a running timeline that multiple staff can contribute to.
// Keeping this separate from EmergencyIncident means the incident class
// stays focused on state, not on logging history.

using System;
using System.Collections.Generic;

namespace EIRS
{
    public class TimelineEntry
    {
        public DateTime Timestamp  { get; private set; }
        public string   AuthorId   { get; private set; }
        public string   EventNote  { get; private set; }

        public TimelineEntry(string authorId, string eventNote)
        {
            Timestamp  = DateTime.Now;
            AuthorId   = authorId;
            EventNote  = eventNote;
        }

        public void Print()
        {
            Console.WriteLine($"  [{Timestamp:HH:mm:ss}] {AuthorId}: {EventNote}");
        }
    }

    public class IncidentTimeline
    {
        public string              IncidentId { get; private set; }
        private List<TimelineEntry> _entries  = new();

        public IncidentTimeline(string incidentId)
        {
            IncidentId = incidentId;
        }

        // Any authorised staff member can add an entry (FR6)
        public void AddEntry(string authorId, string eventNote)
        {
            var entry = new TimelineEntry(authorId, eventNote);
            _entries.Add(entry);
            entry.Print();
            AuditLogService.Instance.Log(authorId, "AddedTimelineEntry", IncidentId);
        }

        // Prints the full event history for this incident
        public void PrintTimeline()
        {
            Console.WriteLine($"\n--- TIMELINE FOR INCIDENT {IncidentId} ---");
            if (_entries.Count == 0) { Console.WriteLine("  (no entries)"); return; }
            foreach (var e in _entries)
                e.Print();
        }
    }
}
