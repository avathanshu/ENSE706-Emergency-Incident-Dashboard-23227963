// OOP Concept: Encapsulation, Association (linked to StaffMember and EmergencyIncident)

using System;

namespace EIRS
{
    public class TimesheetRecord
    {
        private float  _hoursLogged;
        private bool   _discrepancyFlag;

        // Associations — each record belongs to one staff member and one incident
        public StaffMember       Staff    { get; private set; }
        public EmergencyIncident Incident { get; private set; }

        public float HoursLogged     { get => _hoursLogged;     private set => _hoursLogged     = value; }
        public bool  DiscrepancyFlag { get => _discrepancyFlag; private set => _discrepancyFlag = value; }

        public TimesheetRecord(StaffMember staff, EmergencyIncident incident, float hoursLogged)
        {
            Staff    = staff;
            Incident = incident;
            _hoursLogged     = hoursLogged;
            _discrepancyFlag = false;
            FlagDiscrepancy();
        }

        /// <summary>Automatically flags a discrepancy if logged hours exceed the 12-hour threshold.</summary>
        public void FlagDiscrepancy()
        {
            if (_hoursLogged > 12)
            {
                _discrepancyFlag = true;
                Console.WriteLine($"[DISCREPANCY] {Staff.Name} logged {HoursLogged}h for Incident {Incident.IncidentId} — exceeds 12h limit.");
            }
        }

        /// <summary>Prints a summary of this timesheet record.</summary>
        public void PrintSummary()
        {
            string flag = DiscrepancyFlag ? "⚠ FLAGGED" : "OK";
            Console.WriteLine($"[TIMESHEET] {Staff.Name} | Incident: {Incident.IncidentId} | Hours: {HoursLogged} | Status: {flag}");
        }
    }
}