// OOP: Interface — defines the contract that both StaffMember and StaffDecorator must satisfy.
//
// Design Pattern: Decorator
//   This interface is the "Component" role in the Decorator pattern.
//   Both the real objects (StaffMember subclasses) and the wrappers (StaffDecorator subclasses)
//   implement this interface, so callers can treat decorated and undecorated staff identically.
//
// Why a separate interface rather than just the abstract class?
//   StaffDecorator cannot extend StaffMember (it is not a staff member — it wraps one).
//   The interface gives both branches a shared type without forcing an incorrect inheritance.
//
// Relationship summary (from class diagram):
//   IStaffComponent  <|..  StaffMember       (realisation)
//   IStaffComponent  <|..  StaffDecorator    (realisation)
//   StaffDecorator   o-->  IStaffComponent   (aggregation — the wrapped field)

namespace EIRS
{
    /// <summary>
    /// Common contract for all staff participants in the system, whether raw or decorated.
    /// Any class that needs to hold "some staff component" should type its variable as
    /// IStaffComponent rather than StaffMember, so decorators are transparently accepted.
    /// </summary>
    public interface IStaffComponent
    {
        /// <summary>Returns the display name of this staff component.</summary>
        string GetName();

        /// <summary>Returns the unique staff identifier of this staff component.</summary>
        string GetStaffId();

        /// <summary>
        /// Displays the role label for this staff component.
        /// Each concrete class overrides this to describe itself appropriately.
        /// </summary>
        void DisplayRole();
    }
}