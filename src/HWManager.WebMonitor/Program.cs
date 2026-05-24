using System.Diagnostics;
using HWManager.Core.Models;
using HWManager.Core.Services;
using HWManager.WebMonitor;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});
builder.Services.AddSingleton<HardwareMonitorService>();
builder.Services.AddSingleton<ProcessService>();
builder.Services.AddSingleton<HardwareSnapshotStore>();
builder.Services.AddHostedService<HardwareSnapshotCollector>();

var app = builder.Build();

app.UseCors();
app.UseDefaultFiles();
app.UseStaticFiles();

app.MapGet("/api/metrics", (HardwareSnapshotStore snapshots, HardwareMonitorService hardwareMonitor, ProcessService processService) =>
{
    SystemSnapshot latest = snapshots.Latest ?? hardwareMonitor.GetCurrentStatus();

    return Results.Ok(new
    {
        status = "ok",
        machineName = Environment.MachineName,
        os = Environment.OSVersion.VersionString,
        uptimeSeconds = Environment.TickCount64 / 1000,
        processCount = SafeProcessCount(),
        latest = ToDto(latest),
        topProcesses = processService.GetProcesses()
            .Take(10)
            .Select(p => new
            {
                id = p.Id,
                name = p.Name,
                memoryUsageMB = Math.Round(p.MemoryUsageMB, 1)
            })
    });
});

app.MapGet("/api/history", (HardwareSnapshotStore snapshots) => Results.Ok(
    snapshots.GetAll().Select(ToDto)
));

app.MapGet("/api/processes", (ProcessService processService, int? top) =>
{
    int take = Math.Clamp(top ?? 30, 1, 200);

    return Results.Ok(processService.GetProcesses()
        .Take(take)
        .Select(p => new
        {
            id = p.Id,
            name = p.Name,
            memoryUsageMB = Math.Round(p.MemoryUsageMB, 1)
        }));
});

app.MapGet("/health", () => Results.Ok(new { status = "healthy", measuredAt = DateTime.Now }));

app.Run();

static object ToDto(SystemSnapshot snapshot) => new
{
    measuredAt = snapshot.MeasuredAt,
    cpuUsage = Math.Round(snapshot.CpuUsage, 1),
    ramUsage = Math.Round(snapshot.RamUsage, 1),
    gpuUsage = Math.Round(snapshot.GpuUsage, 1)
};

static int SafeProcessCount()
{
    try
    {
        return Process.GetProcesses().Length;
    }
    catch
    {
        return 0;
    }
}
