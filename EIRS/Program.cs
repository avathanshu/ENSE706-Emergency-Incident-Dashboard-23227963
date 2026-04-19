/*
 * Emergency Incident Roster System (EIRS)
 * ENSE706 — Phase 1 | Spectrum Care NZ
 *
 * OOP concepts used throughout:
 *   Abstraction    - StaffMember is abstract, can't be instantiated on its own
 *   Encapsulation  - fields are private, accessed through properties
 *   Inheritance    - SupportWorker, IncidentManager, ITAdministrator, HRManager all extend StaffMember
 *   Polymorphism   - DisplayRole() is overridden in each subclass
 */

using EIRS;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors();
var app = builder.Build();
app.UseCors(p => p.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
app.UseDefaultFiles();
app.UseStaticFiles();

// -- In-memory store (no database for this prototype) -------------------------

var incidents  = new List<EmergencyIncident>();
var shifts     = new List<Shift>();
var timesheets = new List<TimesheetRecord>();

// Keeps track of which worker was assigned to which incident.
// This way we only free that specific worker when the incident is resolved,
// rather than resetting everyone at once.
var allocationMap = new Dictionary<string, SupportWorker>();

// Staff objects - using first names only for readability
var manager = new IncidentManager("IM-01", "Leia");
var hr      = new HRManager("HR-01", "Obi-Wan");
var itAdmin = new ITAdministrator("IT-01", "Padme");

var workers = new List<SupportWorker>
{
    new SupportWorker("SW-01", "Luke",   new List<string> { "Home A", "Home B" }),
    new SupportWorker("SW-02", "Han",    new List<string> { "Home C" }),
    new SupportWorker("SW-03", "Ahsoka", new List<string> { "Home A" }),
};

// -- Endpoints ----------------------------------------------------------------

// Returns the current state of all support workers
app.MapGet("/staff", () => workers.Select(w => new {
    w.StaffId, w.Name, w.IsAvailable,
    w.HoursWorked, Homes = w.AssignedHomes
}));

// Returns all incidents, including who was allocated to each one
app.MapGet("/incidents", () => incidents.Select(i => new {
    i.IncidentId, i.Severity, i.Status,
    Timestamp       = i.Timestamp.ToString("HH:mm:ss"),
    AllocatedWorker = allocationMap.TryGetValue(i.IncidentId, out var w) ? w.Name : "None"
}));

// Logs a new incident and auto-allocates the next available worker
app.MapPost("/incidents", (IncidentRequest req) => {
    var incident = new EmergencyIncident(
        $"INC-{incidents.Count + 1:D3}",
        req.Severity
    );
    incident.LogIncident();
    incidents.Add(incident);

    var allocated = manager.AutoAllocate(incident, workers);
    if (allocated != null)
        allocationMap[incident.IncidentId] = allocated;

    return Results.Ok(new {
        incident.IncidentId,
        incident.Severity,
        incident.Status,
        AllocatedTo = allocated?.Name ?? "None available"
    });
});

// Resolves an incident and only frees the worker that was assigned to it
app.MapPost("/incidents/{id}/resolve", (string id) => {
    var incident = incidents.FirstOrDefault(i => i.IncidentId == id);
    if (incident == null) return Results.NotFound("Incident not found.");

    incident.MarkAsResolved();

    // Only restore the specific worker tied to this incident
    if (allocationMap.TryGetValue(id, out var assignedWorker))
    {
        assignedWorker.IsAvailable = true;
        Console.WriteLine($"[ROSTER] {assignedWorker.Name} freed from Incident {id}.");
        allocationMap.Remove(id);
    }

    return Results.Ok(new { incident.IncidentId, incident.Status });
});

// Returns all timesheet records
app.MapGet("/timesheets", () => timesheets.Select(t => new {
    Staff     = t.Staff.Name,
    Incident  = t.Incident.IncidentId,
    t.HoursLogged,
    t.DiscrepancyFlag
}));

// Saves a new timesheet entry - flags automatically if hours exceed 12
app.MapPost("/timesheets", (TimesheetRequest req) => {
    var worker   = workers.FirstOrDefault(w => w.StaffId == req.StaffId);
    var incident = incidents.FirstOrDefault(i => i.IncidentId == req.IncidentId);
    if (worker == null || incident == null)
        return Results.BadRequest("Invalid StaffId or IncidentId.");

    var record = new TimesheetRecord(worker, incident, req.HoursLogged);
    timesheets.Add(record);
    return Results.Ok(new { record.HoursLogged, record.DiscrepancyFlag });
});

app.Run();

// -- DTOs ---------------------------------------------------------------------
record IncidentRequest(string Severity);
record TimesheetRequest(string StaffId, string IncidentId, float HoursLogged);