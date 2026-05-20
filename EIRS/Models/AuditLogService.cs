// OOP: Singleton pattern -- one audit log for the entire system
// FR14 requires a complete audit trail. Having a second instance would mean
// split logs, which breaks compliance. Singleton prevents this.
// Any class that needs to log an action calls AuditLogService.Instance.Log(...)

using System.Collections.Generic;

namespace EIRS
{
    public class AuditLogService
    {
        private static AuditLogService? _instance;
        private static readonly object _lock = new object();

        private List<AuditLogEntry> _entries = new();
        private int _counter = 1;

        private AuditLogService() { }

        public static AuditLogService Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance == null)
                        _instance = new AuditLogService();
                    return _instance;
                }
            }
        }

        // Records any system action -- called by IncidentManager, HRManager, etc.
        public void Log(string actorId, string action, string targetId)
        {
            var entry = new AuditLogEntry($"A{_counter++}", actorId, action, targetId);
            _entries.Add(entry);
            entry.Print();
        }

        // Returns all entries relating to a specific incident or shift
        public List<AuditLogEntry> GetEntriesFor(string targetId)
        {
            return _entries.FindAll(e => e.TargetId == targetId);
        }

        // Prints all stored audit entries -- used for compliance reporting
        public void PrintAll()
        {
            System.Console.WriteLine("\n--- FULL AUDIT LOG ---");
            foreach (var entry in _entries)
                entry.Print();
        }
    }
}
