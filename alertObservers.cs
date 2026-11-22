/****************/
// MSSV: 202418996
// Họ tên: Nguyễn Khánh Toàn
/****************/

namespace CampusSafetyHub;

public sealed class DashboardObserver : IAlertObserver
{
    public void Update(AlertEvent alertEvent)
    {
        Console.WriteLine(
            $"[DASHBOARD] {alertEvent.SensorName}: {alertEvent.Value:F1}{alertEvent.Unit} " +
            $"[{alertEvent.Severity}]");
    }
}

public sealed class EmailAlertObserver : IAlertObserver
{
    public void Update(AlertEvent alertEvent)
    {
        Console.WriteLine(
            $"[EMAIL] Giả lập gửi cảnh báo {alertEvent.Severity} cho {alertEvent.SensorName}.");
    }
}

public sealed class AuditLogObserver : IAlertObserver
{
    private readonly List<AlertEvent> _entries = new();

    public IReadOnlyList<AlertEvent> Entries => _entries.AsReadOnly();

    public void Update(AlertEvent alertEvent)
    {
        _entries.Add(alertEvent);

        Console.WriteLine(
            $"[AUDIT] {alertEvent.OccurredAt:HH:mm:ss} - {alertEvent.SensorName} " +
            $"[{alertEvent.Severity}]");
    }
}

public sealed class EmergencyBridgeObserver : IAlertObserver
{
    private readonly IEmergencyMediator _mediator;

    public EmergencyBridgeObserver(IEmergencyMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    public void Update(AlertEvent alertEvent)
    {
        if (alertEvent.Severity == "CRITICAL")
            _mediator.Notify(EmergencyEvent.FireDetected);
    }
}
