/****************/
// MSSV: 202418996
// Họ tên: Nguyễn Khánh Toàn
/****************/

using System.Text;

namespace CampusSafetyHub;

public static class Program
{
    public static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;

        PrintHeader("1. ADAPTER");
        var legacy = new LegacySmokeDetector(735);
        ISensor sensor = new SmokeSensorAdapter(legacy);
        Console.WriteLine(FormattableString.Invariant($"  Đọc từ 735: {sensor.ReadValue():F1}{sensor.GetUnit()}"));
        Check("Adapter chuyển đúng giá trị và đơn vị",
            sensor.ReadValue() == 73.5 && sensor.GetUnit() == "% smoke");

        var alarm = new Alarm();
        var door = new DoorController();
        var ventilation = new VentilationController();
        IEmergencyMediator mediator = new EmergencyMediator(alarm, door, ventilation);

        var monitor = new SensorMonitor();
        var audit = new AuditLogObserver();
        monitor.Attach(new DashboardObserver());
        monitor.Attach(new EmailAlertObserver());
        monitor.Attach(audit);
        monitor.Attach(audit);
        monitor.Attach(new EmergencyBridgeObserver(mediator));

        const string sensorName = "Cảm biến phòng máy";

        PrintHeader("2. DƯỚI 50% - BÌNH THƯỜNG");
        legacy.SetSmokeLevel(499);
        monitor.CheckSensor(sensorName, sensor);
        Check("49.9% không phát cảnh báo", audit.Entries.Count == 0);

        PrintHeader("3. TỪ 50% - WARNING");
        legacy.SetSmokeLevel(500);
        monitor.CheckSensor(sensorName, sensor);
        Check("Observer đăng ký hai lần chỉ nhận một WARNING",
            audit.Entries.Count == 1 && audit.Entries[0].Severity == "WARNING");
        Check("WARNING không kích hoạt ứng phó",
            !alarm.IsActive && !door.IsUnlocked && ventilation.IsRunning);

        PrintHeader("4. TỪ 70% - CRITICAL");
        legacy.SetSmokeLevel(700);
        monitor.CheckSensor(sensorName, sensor);
        Check("Nhật ký ghi CRITICAL",
            audit.Entries.Count == 2 && audit.Entries[1].Severity == "CRITICAL");
        Check("Mediator bật còi, mở khóa cửa và tắt thông gió",
            alarm.IsActive && door.IsUnlocked && !ventilation.IsRunning);

        PrintHeader("5. HỦY ĐĂNG KÝ VÀ XỬ LÝ LỖI OBSERVER");
        legacy.SetSmokeLevel(500);
        CheckObserverChanges(sensorName, sensor);
    }

    private static void CheckObserverChanges(string sensorName, ISensor sensor)
    {
        var monitor = new SensorMonitor();
        var audit = new AuditLogObserver();
        int received = 0;

        var selfDetaching = new DemoObserver(observer =>
        {
            received++;
            monitor.Detach(observer);
        });

        monitor.Attach(selfDetaching);
        monitor.Attach(new DemoObserver(_ => throw new InvalidOperationException("Lỗi mô phỏng.")));
        monitor.Attach(audit);

        monitor.CheckSensor(sensorName, sensor);
        monitor.CheckSensor(sensorName, sensor);

        Check("Observer tự hủy chỉ nhận thông báo đầu tiên", received == 1);
        Check("Observer đứng sau vẫn nhận đủ hai cảnh báo", audit.Entries.Count == 2);
    }

    private static void Check(string description, bool condition)
    {
        if (!condition)
            throw new InvalidOperationException($"Không đạt: {description}");

        Console.WriteLine($"  PASS - {description}");
    }

    private static void PrintHeader(string title)
    {
        Console.WriteLine();
        Console.WriteLine($"===== {title} =====");
    }

    private sealed class DemoObserver : IAlertObserver
    {
        private readonly Action<IAlertObserver> _onUpdate;

        public DemoObserver(Action<IAlertObserver> onUpdate)
        {
            _onUpdate = onUpdate;
        }

        public void Update(AlertEvent alertEvent) => _onUpdate(this);
    }
}
