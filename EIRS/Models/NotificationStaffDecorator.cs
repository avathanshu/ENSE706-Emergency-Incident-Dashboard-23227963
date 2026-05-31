// OOP: Concrete Decorator — adds notification-dispatch behaviour to any IStaffComponent.
//
// Design Pattern: Decorator
//   NotificationStaffDecorator is a "ConcreteDecorator" in the Decorator pattern.
//   It extends StaffDecorator and adds Notify(), which routes a message through
//   NotificationService on the chosen channel without touching the wrapped staff object at all.
//
// Why use a decorator instead of adding Notify() to StaffMember?
//   Not every staff component needs every channel — some may only need in-app alerts,
//   others may require SMS escalation. Wrapping at the call site keeps StaffMember focused
//   on workforce state and lets notification logic evolve independently.
// Relationship summary (from class diagram):
//   StaffDecorator  <|--  NotificationStaffDecorator   (inheritance)
//   (uses NotificationService via dependency)

namespace EIRS
{
    /// <summary>
    /// Decorator that adds a <see cref="Notify"/> method to any <see cref="IStaffComponent"/>.
    /// Notification dispatch is routed through <see cref="NotificationService"/> so channel
    /// configuration and history logging remain centralised in that singleton.
    /// </summary>
    public class NotificationStaffDecorator : StaffDecorator
    {
        // The delivery channel for this decorator: "InApp", "Email", or "SMS".
        // Set at construction so the same worker can be wrapped differently per incident type.
        private readonly string _channel;

        /// <summary>
        /// Wraps a staff component with notification capability on the given channel.
        /// </summary>
        /// <param name="component">The staff component to decorate.</param>
        /// <param name="channel">Delivery channel: "InApp" | "Email" | "SMS". Defaults to "InApp".</param>
        public NotificationStaffDecorator(IStaffComponent component, string channel = "InApp")
            : base(component)
        {
            _channel = channel;
        }

        /// <summary>
        /// Delegates to the wrapped component's DisplayRole, then appends the channel info.
        /// This ensures the full decoration stack is visible when roles are printed.
        /// </summary>
        public override void DisplayRole()
        {
            _wrapped.DisplayRole();
            System.Console.WriteLine($"  [+NotificationDecorator] Channel: {_channel}");
        }

        /// <summary>
        /// Sends a notification to the wrapped staff member via <see cref="NotificationService"/>.
        /// Using the service (rather than Console.WriteLine directly) means the message is
        /// stored in history and can be retrieved for compliance audits.
        /// </summary>
        /// <param name="message">The notification message to dispatch.</param>
        public void Notify(string message)
        {
            System.Console.WriteLine($"[NOTIFY-DECORATOR] Dispatching '{_channel}' notification to {GetName()}");
            NotificationService.Instance.Send(GetStaffId(), message, _channel);
        }
    }
}