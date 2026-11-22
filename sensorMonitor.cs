/****************/
// MSSV: 202418996
// Họ tên: Nguyễn Khánh Toàn
/****************/

namespace CampusSafetyHub;

public sealed class SensorMonitor
{
    private readonly List<IAlertObserver> _observers = new();

    public void Attach(IAlertObserver observer)
    {
        ArgumentNullException.ThrowIfNull(observer);

        if (!_observers.Contains(observer))
            _observers.Add(observer);
    }

    public void Detach(IAlertObserver observer)
    {
        _observers.Remove(observer);
    }

    public void CheckSensor(string sensorName, ISensor sensor)
    {
        double value = sensor.ReadValue();
        string? severity = value switch
        {
            >= 70 => "CRITICAL",
            >= 50 => "WARNING",
            _ => null
        };

        if (severity == null)
            return;

        var alertEvent = new AlertEvent(
            sensorName, value, sensor.GetUnit(), severity, DateTime.Now);

        Notify(alertEvent);
    }

    private void Notify(AlertEvent alertEvent)
    {
        // Duyệt bản sao để observer có thể hủy đăng ký ngay trong Update().
        foreach (IAlertObserver observer in _observers.ToArray())
        {
            try
            {
                observer.Update(alertEvent);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[MONITOR] Lỗi ở {observer.GetType().Name}: {ex.Message}");
            }
        }
    }
}
