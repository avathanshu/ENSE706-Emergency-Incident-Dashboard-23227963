// OOP Concepts: Abstraction (abstract class), Encapsulation (private fields), Polymorphism (abstract DisplayRole)

using System;

namespace EIRS
{
    /// <summary>Abstract base class representing any staff member in the system.</summary>
    public abstract class StaffMember
    {
        private string _staffId;
        private string _name;
        private bool   _isAvailable;
        private float  _hoursWorked;

        public string StaffId      { get => _staffId;      private set => _staffId      = value; }
        public string Name         { get => _name;         private set => _name         = value; }
        public bool   IsAvailable  { get => _isAvailable;  set => _isAvailable  = value; }
        public float  HoursWorked  { get => _hoursWorked;  set => _hoursWorked  = value; }

        protected StaffMember(string staffId, string name)
        {
            _staffId     = staffId;
            _name        = name;
            _isAvailable = true;
            _hoursWorked = 0f;
        }

        /// <summary>Sends a notification message to this staff member.</summary>
        public void ReceiveNotification(string message)
        {
            Console.WriteLine($"[NOTIFICATION -> {Name}]: {message}");
        }

        /// <summary>Staff member accepts an offered shift.</summary>
        public void AcceptShift(Shift shift)
        {
            IsAvailable = false;
            Console.WriteLine($"{Name} accepted Shift {shift.ShiftId}.");
        }

        /// <summary>Polymorphic method — each subclass displays its own role label.</summary>
        public abstract void DisplayRole();
    }
}