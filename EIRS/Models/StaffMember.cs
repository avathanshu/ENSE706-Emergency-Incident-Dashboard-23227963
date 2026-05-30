// OOP: Abstract Class, Encapsulation, Polymorphism
//
// StaffMember is the abstract base for every person who works in the system.
// It now implements IStaffComponent, which is required by the Decorator pattern:
//   both StaffMember and StaffDecorator satisfy that interface, allowing decorated and
//   undecorated staff to be used interchangeably wherever IStaffComponent is expected.
//
// Changes from original:
//   - Implements IStaffComponent (adds GetName() and GetStaffId() methods).
//   - GetName() / GetStaffId() satisfy the interface contract without removing the
//     existing Name / StaffId properties, so no call sites break.
//
// Relationship summary (from class diagram):
//   IStaffComponent  <|..  StaffMember                (realisation)
//   StaffMember      <|--  SupportWorker              (inheritance)
//   StaffMember      <|--  IncidentManager            (inheritance)
//   StaffMember      <|--  HRManager                  (inheritance)
//   StaffMember      <|--  ITAdministrator            (inheritance)

using System;

namespace EIRS
{
    /// <summary>
    /// Abstract base class representing any staff member in the EIRS system.
    /// Concrete subclasses must implement <see cref="DisplayRole"/> to describe their role.
    /// </summary>
    public abstract class StaffMember : IStaffComponent
    {
        // Private backing fields — Encapsulation: external code cannot set these directly.
        private string _staffId;
        private string _name;
        private bool   _isAvailable;
        private float  _hoursWorked;

        // Public properties expose the fields with controlled access.
        // StaffId and Name have private setters — they are identity fields and must not change.
        // IsAvailable and HoursWorked are mutable so allocation and timesheet logic can update them.
        public string StaffId     { get => _staffId;     private set => _staffId     = value; }
        public string Name        { get => _name;        private set => _name        = value; }
        public bool   IsAvailable { get => _isAvailable; set       => _isAvailable   = value; }
        public float  HoursWorked { get => _hoursWorked; set       => _hoursWorked   = value; }

        /// <summary>
        /// Initialises a staff member with identity and default availability.
        /// Protected so only subclass constructors can invoke it.
        /// </summary>
        /// <param name="staffId">Unique staff identifier (e.g. "SW-001").</param>
        /// <param name="name">Display name of the staff member.</param>
        protected StaffMember(string staffId, string name)
        {
            _staffId     = staffId;
            _name        = name;
            _isAvailable = true;   // All staff start available until assigned or constrained
            _hoursWorked = 0f;
        }

        // ── IStaffComponent implementation ────────────────────────────────────────────────
        // These two methods satisfy the interface contract so StaffMember and StaffDecorator
        // share a common type without forcing the decorator into the inheritance hierarchy.

        /// <summary>Returns the display name of this staff member. Satisfies IStaffComponent.</summary>
        public string GetName() => _name;

        /// <summary>Returns the unique staff ID of this staff member. Satisfies IStaffComponent.</summary>
        public string GetStaffId() => _staffId;

        // ── Shared behaviour ──────────────────────────────────────────────────────────────

        /// <summary>
        /// Delivers an in-app notification message to this staff member.
        /// For multi-channel dispatch, use <see cref="NotificationStaffDecorator"/> instead.
        /// </summary>
        /// <param name="message">The message text to display.</param>
        public void ReceiveNotification(string message)
        {
            Console.WriteLine($"[NOTIFICATION -> {Name}]: {message}");
        }

        /// <summary>
        /// Records that this staff member has accepted a shift, marking them unavailable.
        /// Called by <see cref="Shift.AssignStaff"/> as part of the shift assignment flow.
        /// </summary>
        /// <param name="shift">The shift being accepted.</param>
        public void AcceptShift(Shift shift)
        {
            IsAvailable = false;
            Console.WriteLine($"{Name} accepted Shift {shift.ShiftId}.");
        }

        // ── Polymorphism ──────────────────────────────────────────────────────────────────

        /// <summary>
        /// Abstract — each subclass prints its own role label.
        /// Polymorphism: callers iterate a List&lt;StaffMember&gt; and call DisplayRole()
        /// without knowing the concrete type.
        /// </summary>
        public abstract void DisplayRole();
    }
}