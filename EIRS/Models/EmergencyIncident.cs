// OOP: Encapsulation, Association
//
// EmergencyIncident represents a single emergency event raised within the system.
// It is the central domain object that incidents, timelines, residents, and
// timesheet records all associate with.
//
// Change from original:
//   Added _incidentType field (present in the class diagram but absent from the original file).
//   IncidentFactory sets this field when constructing typed incidents ("Medical", "Behavioural",
//   "StaffingShortage"), so consumers can branch on type without string-matching the ID prefix.
//
// Relationships (from class diagram):
//   EmergencyIncident  1  *--  1    IncidentTimeline   (composition — owns its timeline)
//   EmergencyIncident  1   -->  1..*  Shift            (association — incident spans shifts)
//   EmergencyIncident  *   -->  1     Resident         (association — incident involves a resident)

using System;

namespace EIRS
{
    /// <summary>
    /// Represents an emergency event requiring staff allocation and documentation.
    /// Created via <see cref="IncidentFactory"/> to ensure type-specific setup is applied.
    /// </summary>
    public class EmergencyIncident
    {
        // ── Private backing fields (Encapsulation) ────────────────────────────────────────
        // All identity and state fields are private; public properties control access.
        private string   _incidentId;
        private string   _severity;
        private string   _status;
        private string   _incidentType;   // Added per class diagram: "Medical" | "Behavioural" | "StaffingShortage"
        private DateTime _timestamp;

        // ── Public properties ─────────────────────────────────────────────────────────────
        // IncidentId, Status, Timestamp, and IncidentType have private setters — they are
        // set at creation and must not be changed by external code.
        // Severity has a public setter because it may be escalated after initial triage.

        /// <summary>Unique identifier for this incident (e.g. "INC-MED-001").</summary>
        public string   IncidentId    { get => _incidentId;    private set => _incidentId    = value; }

        /// <summary>
        /// Triage severity level (e.g. "High", "Medium", "Low").
        /// Public setter allows escalation after the incident is first logged.
        /// </summary>
        public string   Severity      { get => _severity;      set         => _severity      = value; }

        /// <summary>Lifecycle status: "Open" on creation, "Resolved" after MarkAsResolved().</summary>
        public string   Status        { get => _status;        private set => _status        = value; }

        /// <summary>Categorises the incident: "Medical", "Behavioural", or "StaffingShortage".</summary>
        public string   IncidentType  { get => _incidentType;  private set => _incidentType  = value; }

        /// <summary>UTC timestamp recorded when the incident was created.</summary>
        public DateTime Timestamp     { get => _timestamp;     private set => _timestamp     = value; }

        /// <summary>
        /// Constructs a new incident. Prefer using <see cref="IncidentFactory"/> rather than
        /// calling this constructor directly — the factory sets incidentType and triggers
        /// type-specific escalation logic (FR7).
        /// </summary>
        /// <param name="incidentId">Unique identifier assigned by the factory.</param>
        /// <param name="severity">Initial triage severity level.</param>
        /// <param name="incidentType">
        ///   Category of incident. Defaults to "Unknown" when constructed directly (not via factory).
        /// </param>
        public EmergencyIncident(string incidentId, string severity, string incidentType = "Unknown")
        {
            _incidentId   = incidentId;
            _severity     = severity;
            _incidentType = incidentType;
            _status       = "Open";
            _timestamp    = DateTime.Now;
        }

        // ── Behaviour ─────────────────────────────────────────────────────────────────────

        /// <summary>
        /// Writes the incident's current state to the console.
        /// Called immediately after creation so dispatchers can see the event at a glance.
        /// </summary>
        public void LogIncident()
        {
            Console.WriteLine(
                $"[INCIDENT LOGGED] ID: {IncidentId} | Type: {IncidentType} | " +
                $"Severity: {Severity} | Status: {Status} | Time: {Timestamp:HH:mm:ss}"
            );
        }

        /// <summary>
        /// Transitions the incident status to "Resolved" and records the resolution time.
        /// After this call the incident should not receive further allocations or timeline entries.
        /// </summary>
        public void MarkAsResolved()
        {
            _status = "Resolved";
            Console.WriteLine($"[RESOLVED] Incident {IncidentId} marked as resolved at {DateTime.Now:HH:mm:ss}");
        }
    }
}