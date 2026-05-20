// OOP: Strategy pattern interface
// IncidentManager previously contained allocation logic directly in AutoAllocate().
// By depending on this interface instead, IncidentManager does not need to change
// when a new allocation rule is introduced. You just write a new class that implements this.
// FR3 and FR9 both change independently -- Strategy pattern keeps them separate.

using System.Collections.Generic;

namespace EIRS
{
    public interface IAllocationStrategy
    {
        // Returns the best available SupportWorker from the pool, or null if none qualify
        SupportWorker? Allocate(EmergencyIncident incident,
                                List<SupportWorker> pool,
                                List<AllocationConstraint> constraints);
    }
}
