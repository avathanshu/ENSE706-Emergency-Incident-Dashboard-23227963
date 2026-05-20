// OOP: Encapsulation -- one entry in the audit trail
// Kept as a simple data class so AuditLogService can store and query entries
// without any logic leaking into the entry itself.

using System;

namespace EIRS
{
    public class AuditLogEntry
    {
        public string   EntryId     { get; private set; }
        public DateTime Timestamp   { get; private set; }
        public string   ActorId     { get; private set; } // Who performed the action
        public string   Action      { get; private set; } // What was done
        public string   TargetId    { get; private set; } // What it was done to (incident, shift, etc.)

        public AuditLogEntry(string entryId, string actorId, string action, string targetId)
        {
            EntryId   = entryId;
            Timestamp = DateTime.Now;
            ActorId   = actorId;
            Action    = action;
            TargetId  = targetId;
        }

        public void Print()
        {
            Console.WriteLine($"[AUDIT {EntryId}] {Timestamp:HH:mm:ss} | Actor: {ActorId} | Action: {Action} | Target: {TargetId}");
        }
    }
}
