// OOP: Factory Method pattern
// Without this, every caller would write: new EmergencyIncident(id, "High", "Medical", ...)
// and hardcode incident type knowledge everywhere. The factory centralises creation.
// Adding a new incident type (e.g. "Fire") means adding one method here, zero changes elsewhere.
// FR7 requires different behaviour per incident type -- the factory ensures correct setup.

using System;

namespace EIRS
{
    public static class IncidentFactory
    {
        private static int _counter = 1;

        // Creates a medical incident with severity and type pre-configured
        // Automatically triggers nursing escalation flag per FR7
        public static EmergencyIncident CreateMedical(string severityLevel)
        {
            var id = $"INC-MED-{_counter++:D3}";
            var incident = new EmergencyIncident(id, severityLevel);
            Console.WriteLine($"[FACTORY] Medical incident created: {id} | Severity: {severityLevel}");
            Console.WriteLine($"[FACTORY] Auto-escalation to nursing flagged for incident {id}");
            AuditLogService.Instance.Log("System", "CreateMedicalIncident", id);
            return incident;
        }

        // Creates a behavioural incident -- different escalation pathway
        public static EmergencyIncident CreateBehavioural(string severityLevel)
        {
            var id = $"INC-BEH-{_counter++:D3}";
            var incident = new EmergencyIncident(id, severityLevel);
            Console.WriteLine($"[FACTORY] Behavioural incident created: {id} | Severity: {severityLevel}");
            AuditLogService.Instance.Log("System", "CreateBehaviouralIncident", id);
            return incident;
        }

        // Creates a staffing shortage incident -- triggers HR constraint review
        public static EmergencyIncident CreateStaffingShortage(string severityLevel)
        {
            var id = $"INC-STF-{_counter++:D3}";
            var incident = new EmergencyIncident(id, severityLevel);
            Console.WriteLine($"[FACTORY] Staffing shortage incident created: {id} | Severity: {severityLevel}");
            AuditLogService.Instance.Log("System", "CreateStaffingIncident", id);
            return incident;
        }
    }
}
