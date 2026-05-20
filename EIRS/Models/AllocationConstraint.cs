// OOP: Encapsulation -- groups all HR employment constraints for one staff member
// Previously these constraints lived nowhere in code and were only described in the report.
// Separating them here means allocation strategies check one object, not scattered fields.
// FR9: rest periods, max hours, leave status, qualifications, resident familiarity.

using System;
using System.Collections.Generic;

namespace EIRS
{
    public class AllocationConstraint
    {
        public string       StaffId              { get; private set; }
        public bool         OnApprovedLeave      { get; set; }
        public bool         InMandatoryRest      { get; set; }
        public float        MaxHoursPerWeek      { get; set; }
        public float        CurrentWeeklyHours   { get; set; }
        public List<string> Qualifications       { get; private set; }
        public List<string> FamiliarResidentIds  { get; private set; }
        public DateTime     RestPeriodEnd        { get; set; }

        public AllocationConstraint(string staffId, float maxHoursPerWeek, List<string> qualifications)
        {
            StaffId             = staffId;
            MaxHoursPerWeek     = maxHoursPerWeek;
            Qualifications      = qualifications;
            FamiliarResidentIds = new List<string>();
            OnApprovedLeave     = false;
            InMandatoryRest     = false;
            CurrentWeeklyHours  = 0f;
            RestPeriodEnd       = DateTime.MinValue;
        }

        // Returns true if this staff member is eligible to be allocated right now
        public bool IsEligible()
        {
            if (OnApprovedLeave)   { Console.WriteLine($"[CONSTRAINT] {StaffId}: on approved leave");     return false; }
            if (InMandatoryRest)   { Console.WriteLine($"[CONSTRAINT] {StaffId}: in mandatory rest");     return false; }
            if (DateTime.Now < RestPeriodEnd)
                                   { Console.WriteLine($"[CONSTRAINT] {StaffId}: rest period not ended"); return false; }
            if (CurrentWeeklyHours >= MaxHoursPerWeek)
                                   { Console.WriteLine($"[CONSTRAINT] {StaffId}: at max weekly hours");   return false; }
            return true;
        }

        // Checks whether the staff member has a required qualification
        public bool HasQualification(string required)
        {
            return Qualifications.Contains(required);
        }
    }
}
