using HWManager.Core.Services;

namespace HWManager.WebMonitor;

public sealed class HardwareSnapshotCollector(
    HardwareMonitorService hardwareMonitor,
    HardwareSnapshotStore snapshotStore,
    ILogger<HardwareSnapshotCollector> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var timer = new PeriodicTimer(TimeSpan.FromSeconds(1));

        while (!stoppingToken.IsCancellationRequested)
        {
            CollectSnapshot();

            try
            {
                await timer.WaitForNextTickAsync(stoppingToken);
            }
            catch (OperationCanceledException)
            {
                break;
            }
        }
    }

    private void CollectSnapshot()
    {
        try
        {
            snapshotStore.Add(hardwareMonitor.GetCurrentStatus());
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "하드웨어 정보를 수집하지 못했습니다.");
        }
    }
}
