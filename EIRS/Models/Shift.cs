// OOP Concept: Encapsulation, Association (linked to StaffMember — many-to-many)

using System;
using System.Collections.Generic;

namespace EIRS
{
    public class Shift
    {
        private string   _shiftId;
        private DateTime _startTime;
        private DateTime _endTime;

        // A shift can be assigned to multiple staff members
        private List<StaffMember> _assignedStaff = new();

        public string   ShiftId    { get => _shiftId;    private set => _shiftId    = value; }
        public DateTime StartTime  { get => _startTime;  private set => _startTime  = value; }
        public DateTime EndTime    { get => _endTime;    private set => _endTime    = value; }

        public Shift(string shiftId, DateTime startTime, DateTime endTime)
        {
            _shiftId   = shiftId;
            _startTime = startTime;
            _endTime   = endTime;
        }

        /// <summary>Assigns a staff member to this shift and records their acceptance.</summary>
        public void AssignStaff(StaffMember member)
        {
            _assignedStaff.Add(member);
            member.AcceptShift(this);
        }

        /// <summary>Prints all staff currently assigned to this shift.</summary>
        public void PrintAssignedStaff()
        {
            Console.WriteLine($"\n[SHIFT {ShiftId}] Assigned Staff:");
            if (_assignedStaff.Count == 0) { Console.WriteLine("  (none)"); return; }
            foreach (var s in _assignedStaff)
                Console.WriteLine($"  - {s.Name}");
        }
    }
}