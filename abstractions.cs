/****************/
// MSSV: 202418996
// Họ tên: Nguyễn Khánh Toàn
/****************/

namespace CampusSafetyHub;

public interface ISensor
{
    double ReadValue();
    string GetUnit();
}

public sealed record AlertEvent(
    string SensorName,
    double Value,
    string Unit,
    string Severity,
    DateTime OccurredAt);

public interface IAlertObserver
{
    void Update(AlertEvent alertEvent);
}

public enum EmergencyEvent
{
    FireDetected
}

public interface IEmergencyMediator
{
    void Notify(EmergencyEvent emergencyEvent);
}
