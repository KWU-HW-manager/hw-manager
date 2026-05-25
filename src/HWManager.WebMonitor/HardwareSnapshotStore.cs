using HWManager.Core.Models;

namespace HWManager.WebMonitor;

public sealed class HardwareSnapshotStore
{
    private const int Capacity = 180;
    private readonly Queue<SystemSnapshot> _items = new();
    private readonly object _syncRoot = new();

    public SystemSnapshot? Latest
    {
        get
        {
            lock (_syncRoot)
            {
                return _items.Count == 0 ? null : _items.Last();
            }
        }
    }

    public void Add(SystemSnapshot snapshot)
    {
        lock (_syncRoot)
        {
            _items.Enqueue(snapshot);
            while (_items.Count > Capacity)
            {
                _items.Dequeue();
            }
        }
    }

    public IReadOnlyList<SystemSnapshot> GetAll()
    {
        lock (_syncRoot)
        {
            return _items.ToArray();
        }
    }
}
