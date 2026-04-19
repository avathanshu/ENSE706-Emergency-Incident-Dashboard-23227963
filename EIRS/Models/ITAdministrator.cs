// OOP Concept: Inheritance (extends StaffMember)

using System;

namespace EIRS
{
    public class ITAdministrator : StaffMember
    {
        public ITAdministrator(string staffId, string name) : base(staffId, name) { }

        /// <summary>Manages system access controls for the given staff member.</summary>
        public void ManageAccessControls(StaffMember target, string action)
        {
            Console.WriteLine($"[IT ADMIN] {Name} {action} access for {target.Name}.");
        }

        public override void DisplayRole()
        {
            Console.WriteLine($"{Name} | Role: IT Administrator");
        }
    }
}