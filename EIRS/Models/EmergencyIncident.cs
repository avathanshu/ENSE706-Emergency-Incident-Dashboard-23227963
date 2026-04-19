// OOP Concepts: Encapsulation (private fields, public properties), Association (linked to Shift and TimesheetRecord)

using System;

namespace EIRS
{
    public class EmergencyIncident
    {
        // Private backing fields — encapsulation
        private string _incidentId;
        private string _severity;
        private string _status;
        private DateTime _timestamp;

        public string IncidentId   { get => _incidentId;  private set => _incidentId  = value; }
        public string Severity     { get => _severity;    set => _severity    = value; }
        public string Status       { get => _status;      private set => _status      = value; }
        public DateTime Timestamp  { get => _timestamp;   private set => _timestamp   = value; }

        public EmergencyIncident(string incidentId, string severity)
        {
            _incidentId = incidentId;
            _severity   = severity;
            _status     = "Open";
            _timestamp  = DateTime.Now;
        }

        /// <summary>Logs the incident details to the console.</summary>
        public void LogIncident()
        {
            Console.WriteLine($"[INCIDENT LOGGED] ID: {IncidentId} | Severity: {Severity} | Status: {Status} | Time: {Timestamp:HH:mm:ss}");
        }

        /// <summary>Marks the incident as resolved and records the resolution time.</summary>
        public void MarkAsResolved()
        {
            _status = "Resolved";
            Console.WriteLine($"[RESOLVED] Incident {IncidentId} marked as resolved at {DateTime.Now:HH:mm:ss}");
        }
    }
}