/****************/
// MSSV: 202418996
// Họ tên: Nguyễn Khánh Toàn
/****************/

namespace CampusSafetyHub;

public sealed class SmokeSensorAdapter : ISensor
{
    private readonly LegacySmokeDetector _legacy;

    public SmokeSensorAdapter(LegacySmokeDetector legacy)
    {
        _legacy = legacy ?? throw new ArgumentNullException(nameof(legacy));
    }

    // Cảm biến cũ dùng thang 0..1000, còn SensorMonitor dùng phần trăm.
    public double ReadValue() => _legacy.GetSmokeLevel() / 10.0;

    public string GetUnit() => "% smoke";
}
