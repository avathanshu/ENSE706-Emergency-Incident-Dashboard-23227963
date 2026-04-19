// OOP Concept: Inheritance (extends StaffMember)

using System;
using System.Collections.Generic;

namespace EIRS
{
    public class SupportWorker : StaffMember
    {
        // List of residential homes this worker is assigned to
        private List<string> _assignedHomes;
        public List<string> AssignedHomes { get => _assignedHomes; private set => _assignedHomes = value; }

        public SupportWorker(string staffId, string name, List<string> assignedHomes)
            : base(staffId, name)
        {
            _assignedHomes = assignedHomes;
        }

        /// <summary>Displays the shift details relevant to this support worker.</summary>
        public void ViewShiftDetails(Shift shift)
        {
            Console.WriteLine($"[SHIFT DETAILS for {Name}] Shift {shift.ShiftId}: {shift.StartTime:HH:mm} - {shift.EndTime:HH:mm}");
        }

        /// <summary>Polymorphic override — prints this staff member's role.</summary>
        public override void DisplayRole()
        {
            Console.WriteLine($"{Name} | Role: Support Worker | Homes: {string.Join(", ", AssignedHomes)}");
        }
    }
}