// OOP: Concrete Strategy -- checks availability and HR constraints before allocating
// This replaces the simple foreach loop in IncidentManager.AutoAllocate().
// A second strategy (e.g. QualificationStrategy, SeniorityStrategy) can be added
// without changing IncidentManager or this class at all.

using System.Collections.Generic;

namespace EIRS
{
    public class BasicAvailabilityStrategy : IAllocationStrategy
    {
        // Finds the first worker who is available AND passes all HR constraints
        public SupportWorker? Allocate(EmergencyIncident incident,
                                       List<SupportWorker> pool,
                                       List<AllocationConstraint> constraints)
        {
            System.Console.WriteLine($"[STRATEGY: BasicAvailability] Searching for staff for incident {incident.IncidentId}");

            foreach (var worker in pool)
            {
                if (!worker.IsAvailable) continue;

                // Find this worker's constraint object
                var constraint = constraints.Find(c => c.StaffId == worker.StaffId);

                // If no constraint record exists, treat as ineligible for safety
                if (constraint == null)
                {
                    System.Console.WriteLine($"[STRATEGY] No constraint record for {worker.Name}, skipping");
                    continue;
                }

                if (constraint.IsEligible())
                {
                    System.Console.WriteLine($"[STRATEGY] Selected: {worker.Name}");
                    return worker;
                }
            }

            System.Console.WriteLine("[STRATEGY] No eligible staff found");
            return null;
        }
    }
}
