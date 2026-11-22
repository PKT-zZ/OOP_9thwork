/****************/
// MSSV: 202418996
// Họ tên: Nguyễn Khánh Toàn
/****************/

namespace CampusSafetyHub;

public sealed class Alarm
{
    public bool IsActive { get; private set; }

    public void Activate()
    {
        IsActive = true;
        Console.WriteLine("[ALARM] Bật còi báo động.");
    }
}

public sealed class DoorController
{
    public bool IsUnlocked { get; private set; }

    public void Unlock()
    {
        IsUnlocked = true;
        Console.WriteLine("[DOOR] Mở khóa cửa thoát hiểm.");
    }
}

public sealed class VentilationController
{
    public bool IsRunning { get; private set; } = true;

    public void Stop()
    {
        IsRunning = false;
        Console.WriteLine("[VENTILATION] Tắt thông gió.");
    }
}
