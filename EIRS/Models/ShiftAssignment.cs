// OOP: Association class -- sits between Shift and StaffMember
// Previously Shift held a List<StaffMember> directly, losing confirmation status
// and role context. This class owns that relationship and carries the extra data.
// Adding new fields (e.g. transport required, overtime flag) goes here, not in Shift.

using System;

namespace EIRS
{
    public enum AssignmentStatus { Pending, Accepted, Declined }

    public class ShiftAssignment
    {
        public string           AssignmentId { get; private set; }
        public Shift            Shift        { get; private set; }
        public StaffMember      Staff        { get; private set; }
        public AssignmentStatus Status       { get; private set; }
        public float            HoursWorked  { get; private set; }
        public string           RoleOnShift  { get; private set; }

        public ShiftAssignment(string assignmentId, Shift shift, StaffMember staff, string roleOnShift)
        {
            AssignmentId = assignmentId;
            Shift        = shift;
            Staff        = staff;
            RoleOnShift  = roleOnShift;
            Status       = AssignmentStatus.Pending;
            HoursWorked  = 0f;
        }

        // Staff member confirms they are taking this shift
        public void Accept()
        {
            Status = AssignmentStatus.Accepted;
            Console.WriteLine($"[ASSIGNMENT] {Staff.Name} ACCEPTED shift {Shift.ShiftId} as {RoleOnShift}");
            AuditLogService.Instance.Log(Staff.StaffId, "AcceptedShift", Shift.ShiftId);
        }

        // Staff member declines -- triggers escalation logic upstream
        public void Decline()
        {
            Status = AssignmentStatus.Declined;
            Console.WriteLine($"[ASSIGNMENT] {Staff.Name} DECLINED shift {Shift.ShiftId}");
            AuditLogService.Instance.Log(Staff.StaffId, "DeclinedShift", Shift.ShiftId);
        }

        // Called at close-out to record actual hours against this assignment
        public void LogHours(float hours)
        {
            HoursWorked = hours;
            Console.WriteLine($"[HOURS] {Staff.Name} logged {hours}h for shift {Shift.ShiftId}");
        }
    }
}
