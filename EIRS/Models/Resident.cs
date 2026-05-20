// OOP: Encapsulation -- clinical data is private and accessed only through properties
// This class exists separately from StaffMember to keep resident data isolated from workforce data.
// A Resident is not a system user, so it does not inherit from StaffMember.

using System.Collections.Generic;

namespace EIRS
{
    public class Resident
    {
        public string ResidentId   { get; private set; }
        public string Name         { get; private set; }

        // Clinical fields required by FR8 -- accessed during emergency incidents
        private List<string> _medications;
        private List<string> _allergies;
        private string       _advanceCareDirective;
        private List<string> _clinicalFlags;

        public Resident(string residentId, string name, List<string> medications,
                        List<string> allergies, string advanceCareDirective, List<string> clinicalFlags)
        {
            ResidentId            = residentId;
            Name                  = name;
            _medications          = medications;
            _allergies            = allergies;
            _advanceCareDirective = advanceCareDirective;
            _clinicalFlags        = clinicalFlags;
        }

        // Returns a formatted clinical summary for use during an incident
        public string GetClinicalSummary()
        {
            return $"[RESIDENT: {Name}] " +
                   $"Medications: {string.Join(", ", _medications)} | " +
                   $"Allergies: {string.Join(", ", _allergies)} | " +
                   $"ACD: {_advanceCareDirective} | " +
                   $"Flags: {string.Join(", ", _clinicalFlags)}";
        }

        public void PrintClinicalSummary()
        {
            System.Console.WriteLine(GetClinicalSummary());
        }
    }
}
