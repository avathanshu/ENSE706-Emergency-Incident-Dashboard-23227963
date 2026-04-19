// OOP Concept: Inheritance (extends StaffMember), Polymorphism (DisplayRole override)

using System;
using System.Collections.Generic;

namespace EIRS
{
    public class IncidentManager : StaffMember
    {
        public IncidentManager(string staffId, string name) : base(staffId, name) { }

        /// <summary>
        /// Auto-allocates the first available SupportWorker to the given incident.
        /// Returns the allocated worker, or null if none are available.
        /// </summary>
        public SupportWorker? AutoAllocate(EmergencyIncident incident, List<SupportWorker> staff)
        {
            Console.WriteLine($"\n[AUTO-ALLOCATE] Searching for available staff for Incident {incident.IncidentId}...");

            foreach (var worker in staff)
            {
                if (worker.IsAvailable)
                {
                    worker.IsAvailable = false;
                    worker.ReceiveNotification($"You have been allocated to Incident {incident.IncidentId} (Severity: {incident.Severity}).");
                    Console.WriteLine($"[ALLOCATED] {worker.Name} assigned to Incident {incident.IncidentId}.");
                    return worker;
                }
            }

            Console.WriteLine("[WARNING] No available staff found for allocation.");
            return null;
        }

        /// <summary>Restores the roster by marking all workers as available again.</summary>
        public void RestoreRoster(List<SupportWorker> staff)
        {
            foreach (var worker in staff)
                worker.IsAvailable = true;

            Console.WriteLine("[ROSTER RESTORED] All staff marked as available.");
        }

        /// <summary>Allocates staff manually (wrapper for demo flexibility).</summary>
        public void AllocateStaff(SupportWorker worker, EmergencyIncident incident)
        {
            worker.IsAvailable = false;
            Console.WriteLine($"[MANUAL ALLOCATE] {worker.Name} manually assigned to Incident {incident.IncidentId}.");
        }

        public override void DisplayRole()
        {
            Console.WriteLine($"{Name} | Role: Incident Manager");
        }
    }
}