using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using EIRS; // Keep your namespace imports

// 1. Initialize the Web Application Builder
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// 2. Enable serving index.html as the default file from wwwroot
app.UseDefaultFiles();
app.UseStaticFiles();

// 3. Keep your existing backend console logic running at startup
Console.WriteLine("==============================================");
Console.WriteLine("    EIRS BACKEND CORE INITIALIZATION          ");
Console.WriteLine("==============================================");

var audit  = AuditLogService.Instance;
var notify = NotificationService.Instance;

var medIncident = IncidentFactory.CreateMedical("High");
medIncident.LogIncident();

var resident = new Resident("R001", "John Smith",
    new List<string> { "Metformin", "Lisinopril" },
    new List<string> { "Penicillin" },
    "Do not resuscitate",
    new List<string> { "Seizure risk", "Fall risk" });
resident.PrintClinicalSummary();

var constraint = new AllocationConstraint("SW001", 40f, new List<string> { "First Aid", "Manual Handling" });
Console.WriteLine($"SW001 eligible: {constraint.IsEligible()}");

var worker = new SupportWorker("SW001", "Alice Brown", new List<string> { "Home A", "Home B" });
var strategy = new BasicAvailabilityStrategy();
var allocated = strategy.Allocate(medIncident, new List<SupportWorker> { worker }, new List<AllocationConstraint> { constraint });

var shift = new Shift("S001", DateTime.Now, DateTime.Now.AddHours(8));
if (allocated != null)
{
    var assignment = new ShiftAssignment("SA001", shift, allocated, "SupportWorker");
    assignment.Accept();
    assignment.LogHours(8f);
}

notify.Send("SW001", "You have been allocated to incident " + medIncident.IncidentId);
notify.Send("SW002", "Incident update: High severity at Home A", "Email");

var timeline = new IncidentTimeline(medIncident.IncidentId);
timeline.AddEntry("IM001", "Incident logged, staff allocation initiated");
timeline.AddEntry("SW001", "Arrived at scene, resident stable");
timeline.PrintTimeline();

audit.PrintAll();
Console.WriteLine("==============================================");

// 4. Start the Web Server
Console.WriteLine("\n🚀 EIRS Server Web Dashboard running!");
Console.WriteLine("👉 Open: http://localhost:5000 or https://localhost:5001");
app.Run();