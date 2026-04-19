// OOP Concept: Inheritance (extends StaffMember)

using System;

namespace EIRS
{
    public class HRManager : StaffMember
    {
        public HRManager(string staffId, string name) : base(staffId, name) { }

        /// <summary>Reviews and logs an allocation decision made by an IncidentManager.</summary>
        public void ReviewAllocationDecisions(IncidentManager manager, EmergencyIncident incident)
        {
            Console.WriteLine($"[HR REVIEW] {Name} reviewed allocation by {manager.Name} for Incident {incident.IncidentId}.");
        }

        public override void DisplayRole()
        {
            Console.WriteLine($"{Name} | Role: HR Manager");
        }
    }
}