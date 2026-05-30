// OOP: Abstract Class, Decorator Pattern
//
// Design Pattern: Decorator
//   StaffDecorator is the "Decorator" role in the Decorator pattern.
//   It wraps any IStaffComponent and forwards GetName / GetStaffId to the wrapped object,
//   so subclasses only need to override DisplayRole() and add their extra behaviour.
//
//   Concrete decorators (NotificationStaffDecorator, LoggingStaffDecorator) extend this class.
//   Callers never construct a StaffDecorator directly — they wrap an existing IStaffComponent.
//
// Why aggregation, not inheritance?
//   The diagram shows StaffDecorator aggregates IStaffComponent (hollow diamond).
//   A decorator IS-NOT a StaffMember; it HAS-A IStaffComponent it delegates to.
//   This lets decorators be stacked: e.g. new LoggingStaffDecorator(new NotificationStaffDecorator(worker))
//
// Relationship summary (from class diagram):
//   IStaffComponent  <|..  StaffDecorator                    (realisation)
//   StaffDecorator   o-->  IStaffComponent   (wrapped field) (aggregation)
//   StaffDecorator   <|--  NotificationStaffDecorator        (inheritance)
//   StaffDecorator   <|--  LoggingStaffDecorator             (inheritance)

namespace EIRS
{
    /// <summary>
    /// Abstract base for all staff decorators.
    /// Holds a reference to a wrapped IStaffComponent and forwards identity queries to it.
    /// Subclasses add cross-cutting behaviour (notifications, logging) without modifying
    /// any existing StaffMember class.
    /// </summary>
    public abstract class StaffDecorator : IStaffComponent
    {
        // Protected so subclasses can reach through to the wrapped component if needed,
        // e.g. to pass it to a service call. The diagram marks this field as protected (#).
        protected readonly IStaffComponent _wrapped;

        /// <summary>
        /// Constructs a decorator around an existing staff component.
        /// The component may itself be another decorator (stacking is supported).
        /// </summary>
        /// <param name="component">The IStaffComponent to wrap. Must not be null.</param>
        protected StaffDecorator(IStaffComponent component)
        {
            _wrapped = component;
        }

        /// <summary>
        /// Forwards the name query to the wrapped component.
        /// Decorators do not have their own identity — they augment the wrapped object's.
        /// </summary>
        public string GetName() => _wrapped.GetName();

        /// <summary>
        /// Forwards the staff ID query to the wrapped component.
        /// </summary>
        public string GetStaffId() => _wrapped.GetStaffId();

        /// <summary>
        /// Abstract — each concrete decorator must describe itself (and may delegate to wrapped).
        /// </summary>
        public abstract void DisplayRole();
    }
}