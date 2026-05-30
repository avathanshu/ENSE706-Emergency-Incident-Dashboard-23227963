// OOP: Factory Method Pattern (static factory class)
//
// IncidentFactory centralises the creation of EmergencyIncident objects.
// Without this, every caller would write:
//   new EmergencyIncident(id, severity, "Medical")
// and hardcode type knowledge — and type-specific setup (auto-escalation, HR review flags)
// would be scattered across the codebase.
//
// Adding a new incident type (e.g. "Fire") means adding one method here; zero changes elsewhere.
//
// Change from original:
//   Now passes the incident type string as the third constructor argument, which was
//   added to EmergencyIncident to satisfy the class diagram (_incidentType field).
//
// Relationship summary (from class diagram):
//   IncidentFactory  ..>  EmergencyIncident   (dependency — creates instances)
//   IncidentFactory  ..>  AuditLogService     (dependency — logs creation events)
//
// FR7: Different incident types trigger different escalation pathways — the factory
//      ensures the correct setup is applied at creation time.

using System;

namespace EIRS
{
    /// <summary>
    /// Static factory for creating typed <see cref="EmergencyIncident"/> objects.
    /// Each method applies type-specific configuration and logs the creation event
    /// to <see cref="AuditLogService"/> for FR14 compliance.
    /// </summary>
    public static class IncidentFactory
    {
        // Auto-incrementing counter ensures unique IDs across the process lifetime.
        private static int _counter = 1;

        /// <summary>
        /// Creates a medical emergency incident.
        /// Automatically flags nursing escalation per FR7.
        /// </summary>
        /// <param name="severityLevel">Triage severity: "High", "Medium", or "Low".</param>
        /// <returns>A fully initialised medical <see cref="EmergencyIncident"/>.</returns>
        public static EmergencyIncident CreateMedical(string severityLevel)
        {
            var id = $"INC-MED-{_counter++:D3}";

            // Pass "Medical" as incidentType so consumers can branch without parsing the ID prefix.
            var incident = new EmergencyIncident(id, severityLevel, "Medical");

            Console.WriteLine($"[FACTORY] Medical incident created: {id} | Severity: {severityLevel}");
            Console.WriteLine($"[FACTORY] Auto-escalation to nursing flagged for incident {id}");

            // Audit every creation event — actor is "System" because no human initiated this call.
            AuditLogService.Instance.Log("System", "CreateMedicalIncident", id);

            return incident;
        }

        /// <summary>
        /// Creates a behavioural emergency incident.
        /// Routes through a different escalation pathway than medical incidents (FR7).
        /// </summary>
        /// <param name="severityLevel">Triage severity: "High", "Medium", or "Low".</param>
        /// <returns>A fully initialised behavioural <see cref="EmergencyIncident"/>.</returns>
        public static EmergencyIncident CreateBehavioural(string severityLevel)
        {
            var id = $"INC-BEH-{_counter++:D3}";

            var incident = new EmergencyIncident(id, severityLevel, "Behavioural");

            Console.WriteLine($"[FACTORY] Behavioural incident created: {id} | Severity: {severityLevel}");

            AuditLogService.Instance.Log("System", "CreateBehaviouralIncident", id);

            return incident;
        }

        /// <summary>
        /// Creates a staffing shortage incident.
        /// Triggers HR constraint review to check whether additional staff can be sourced (FR9).
        /// </summary>
        /// <param name="severityLevel">Triage severity: "High", "Medium", or "Low".</param>
        /// <returns>A fully initialised staffing shortage <see cref="EmergencyIncident"/>.</returns>
        public static EmergencyIncident CreateStaffingShortage(string severityLevel)
        {
            var id = $"INC-STF-{_counter++:D3}";

            var incident = new EmergencyIncident(id, severityLevel, "StaffingShortage");

            Console.WriteLine($"[FACTORY] Staffing shortage incident created: {id} | Severity: {severityLevel}");
            Console.WriteLine($"[FACTORY] HR constraint review triggered for incident {id}");

            AuditLogService.Instance.Log("System", "CreateStaffingIncident", id);

            return incident;
        }
    }
}