/****************/
// MSSV: 202418996
// Họ tên: Nguyễn Khánh Toàn
/****************/

namespace CampusSafetyHub;

public sealed class EmergencyMediator : IEmergencyMediator
{
    private readonly Alarm _alarm;
    private readonly DoorController _door;
    private readonly VentilationController _ventilation;

    public EmergencyMediator(Alarm alarm, DoorController door, VentilationController ventilation)
    {
        _alarm = alarm ?? throw new ArgumentNullException(nameof(alarm));
        _door = door ?? throw new ArgumentNullException(nameof(door));
        _ventilation = ventilation ?? throw new ArgumentNullException(nameof(ventilation));
    }

    public void Notify(EmergencyEvent emergencyEvent)
    {
        if (emergencyEvent != EmergencyEvent.FireDetected)
            return;

        Console.WriteLine("[MEDIATOR] FIRE_DETECTED");
        _alarm.Activate();
        _door.Unlock();
        _ventilation.Stop();
    }
}
