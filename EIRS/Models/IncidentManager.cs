// OOP: Inheritance, Polymorphism, Strategy Pattern dependency
//
// IncidentManager extends StaffMember and is responsible for allocating support workers
// to emergency incidents and managing the on-call roster.
//
// Key design change from original:
//   The original AutoAllocate() contained its own foreach loop that checked IsAvailable
//   directly. This mixed allocation policy into the manager class, making it hard to swap
//   rules (e.g. adding seniority weighting or qualification filtering).
//
//   The class now holds a private IAllocationStrategy field injected at construction.
//   AutoAllocate() simply delegates to that strategy. To change allocation rules, pass
//   a different strategy — IncidentManager never needs to change.
//
//   The overloaded AutoAllocate(incident, staff, constraints) method supports the full
//   strategy signature (needed when HR constraints must be evaluated).
//
// NotificationService dependency:
//   Notifications are now dispatched through NotificationService.Instance rather than
//   calling worker.ReceiveNotification() directly. This routes the message through the
//   singleton so it appears in notification history and can be audited.
//
// Relationship summary (from class diagram):
//   StaffMember      <|--  IncidentManager             (inheritance)
//   IncidentManager  ..>   IAllocationStrategy         (dependency — injected)
//   IncidentManager  ..>   NotificationService         (dependency — singleton call)
//   IncidentManager  ..>   BasicAvailabilityStrategy   (dependency — default strategy)

using System;
using System.Collections.Generic;

namespace EIRS
{
    /// <summary>
    /// A staff member who manages the allocation of support workers to emergency incidents.
    /// Allocation logic is fully delegated to an injected <see cref="IAllocationStrategy"/>,
    /// so the manager class never needs to change when allocation rules change.
    /// </summary>
    public class IncidentManager : StaffMember
    {
        // The strategy used to select a worker for each incident.
        // Typed as the interface so any IAllocationStrategy implementation can be swapped in.
        private readonly IAllocationStrategy _strategy;

        /// <summary>
        /// Constructs an IncidentManager with a specific allocation strategy.
        /// </summary>
        /// <param name="staffId">Unique staff identifier.</param>
        /// <param name="name">Display name.</param>
        /// <param name="strategy">
        ///   The allocation strategy to use. If null, defaults to
        ///   <see cref="BasicAvailabilityStrategy"/> so the system is always functional.
        /// </param>
        public IncidentManager(string staffId, string name, IAllocationStrategy? strategy = null)
            : base(staffId, name)
        {
            // Default to BasicAvailabilityStrategy if none provided.
            // This keeps existing call sites (that pass no strategy) working unchanged.
            _strategy = strategy ?? new BasicAvailabilityStrategy();
        }

        /// <summary>
        /// Auto-allocates the best available support worker to the given incident using the
        /// injected <see cref="IAllocationStrategy"/>. This overload operates without HR
        /// constraints — use the three-argument overload when constraints must be evaluated.
        /// </summary>
        /// <param name="incident">The incident requiring a worker.</param>
        /// <param name="staff">Pool of support workers to select from.</param>
        /// <returns>The allocated worker, or null if none are eligible.</returns>
        public SupportWorker? AutoAllocate(EmergencyIncident incident, List<SupportWorker> staff)
        {
            Console.WriteLine($"\n[AUTO-ALLOCATE] {Name} allocating staff for Incident {incident.IncidentId}...");

            // Build empty constraints list so the strategy can run without null checks.
            // Workers will pass constraint eligibility as long as no constraints are registered.
            var emptyConstraints = new List<AllocationConstraint>();

            var worker = _strategy.Allocate(incident, staff, emptyConstraints);

            if (worker != null)
            {
                // Mark the worker unavailable and notify them via the singleton service.
                // Using NotificationService (rather than worker.ReceiveNotification) ensures
                // the message is stored in history for compliance queries.
                worker.IsAvailable = false;
                NotificationService.Instance.Send(
                    worker.StaffId,
                    $"You have been allocated to Incident {incident.IncidentId} (Severity: {incident.Severity}).",
                    "InApp"
                );
                Console.WriteLine($"[ALLOCATED] {worker.Name} assigned to Incident {incident.IncidentId}.");

                // Audit the allocation decision for FR14 compliance.
                AuditLogService.Instance.Log(StaffId, "AutoAllocatedWorker", incident.IncidentId);
            }
            else
            {
                Console.WriteLine("[WARNING] No available staff found for allocation.");
            }

            return worker;
        }

        /// <summary>
        /// Auto-allocates using the full strategy signature, including HR constraint evaluation.
        /// Use this overload when AllocationConstraint records are available (standard path).
        /// </summary>
        /// <param name="incident">The incident requiring a worker.</param>
        /// <param name="staff">Pool of support workers to select from.</param>
        /// <param name="constraints">HR constraints for each worker in the pool.</param>
        /// <returns>The allocated worker, or null if none are eligible.</returns>
        public SupportWorker? AutoAllocate(EmergencyIncident incident,
                                           List<SupportWorker> staff,
                                           List<AllocationConstraint> constraints)
        {
            Console.WriteLine($"\n[AUTO-ALLOCATE] {Name} allocating staff (with constraints) for Incident {incident.IncidentId}...");

            var worker = _strategy.Allocate(incident, staff, constraints);

            if (worker != null)
            {
                worker.IsAvailable = false;
                NotificationService.Instance.Send(
                    worker.StaffId,
                    $"You have been allocated to Incident {incident.IncidentId} (Severity: {incident.Severity}).",
                    "InApp"
                );
                Console.WriteLine($"[ALLOCATED] {worker.Name} assigned to Incident {incident.IncidentId}.");
                AuditLogService.Instance.Log(StaffId, "AutoAllocatedWorker", incident.IncidentId);
            }
            else
            {
                Console.WriteLine("[WARNING] No available staff found for allocation.");
            }

            return worker;
        }

        /// <summary>
        /// Manually assigns a specific worker to an incident, bypassing strategy selection.
        /// Used when an IncidentManager overrides the automatic allocation.
        /// </summary>
        /// <param name="worker">The worker to assign.</param>
        /// <param name="incident">The incident they are being assigned to.</param>
        public void AllocateStaff(SupportWorker worker, EmergencyIncident incident)
        {
            worker.IsAvailable = false;
            Console.WriteLine($"[MANUAL ALLOCATE] {worker.Name} manually assigned to Incident {incident.IncidentId}.");
            AuditLogService.Instance.Log(StaffId, "ManualAllocatedWorker", incident.IncidentId);
        }

        /// <summary>
        /// Restores the roster by marking all workers in the pool as available again.
        /// Called after an incident is resolved or for end-of-shift cleanup.
        /// </summary>
        /// <param name="staff">The pool of workers to restore.</param>
        public void RestoreRoster(List<SupportWorker> staff)
        {
            foreach (var worker in staff)
                worker.IsAvailable = true;

            Console.WriteLine("[ROSTER RESTORED] All staff marked as available.");
            AuditLogService.Instance.Log(StaffId, "RosterRestored", "AllStaff");
        }

        /// <summary>Polymorphic role display — prints this staff member's role label.</summary>
        public override void DisplayRole()
        {
            Console.WriteLine($"{Name} | Role: Incident Manager");
        }
    }
}