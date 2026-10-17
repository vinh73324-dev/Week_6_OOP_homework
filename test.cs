/************************
Họ và tên: Phạm Văn Vinh
MSSV: 202419018
*************************/
using System;

class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        // I. Luồng thành công
        SeperateLine();
        Console.WriteLine("=== I. Luồng thành công ===");
        SeperateLine();
        // 1. Tạo 1 giảng viên
        Console.WriteLine(" 1. Tạo 1 giảng viên");
        var lec1 = new Lecturer("LEC001", "Vũ Thành Nam");

        // 2. Tạo thiết bị
        Console.WriteLine(" 2. Tạo thiết bị");
        var pjr1 = new ProjectorRemote("PJR001","Điều khiển máy chiếu Sony", 2020, DeviceStatus.Available, 2);
        var acr1 = new AirConditionerRemote("ACR001", "Điều khiển điều hòa Daikin", 2023, DeviceStatus.Available, 3);
        var mic1 = new Microphone("MIC001", "Micro không dây AKG", 2024, DeviceStatus.Available, 1);

        // 3. Tạo phiếu mượn
        Console.WriteLine(" 3. Tạo phiếu mượn");
        var slip1 = new BorrowingSlip("SLIP001", lec1, "D3-101", new DateTime(2026, 10, 10));

        // 4. Thêm thêm thiết bị vào phiếu
        Console.WriteLine(" 4. Thêm thêm thiết bị vào phiếu");
        slip1.AddDevice(pjr1, 2);
        slip1.AddDevice(mic1, 1);
        slip1.AddDevice(acr1, 3);

        // 5. Trả điều khiển máy chiếu đúng hạn
        Console.WriteLine(" 5. Trả điều khiển máy chiếu đúng hạn");
        decimal lateFeePJR = slip1.ReturnDevice("PJR001", new DateTime(2026, 10, 12));
        Console.WriteLine($"Tiền phạt trả điều khiển máy chiếu: {lateFeePJR} VND");

        // 6. Trả micro muộn 2 ngày
        Console.WriteLine(" 6. Trả micro muộn 2 ngày");
        decimal lateFeeMic = slip1.ReturnDevice("MIC001", new DateTime(2026, 10, 13));
        Console.WriteLine($"Tiền phạt trả microphone: {lateFeeMic} VND");

        // 7. Hiển thị các phiếu trả muộn, chưa trả
        Console.WriteLine(" 7. Hiển thị các phiếu trả muộn, chưa trả");
        Console.WriteLine("     -- Các thiết bị trả muộn --");
        var lateItems = slip1.Items.Where(i => i.IsReturned && (i.ReturnedDate > i.DueDate));
        foreach (var item in lateItems)
        {
            int lateDays = (item.ReturnedDate.Value.Date - item.DueDate.Date).Days;
            Console.WriteLine($"- Phiếu: {slip1.SlipId} | Thiết bị: {item.Device.DeviceId} ({item.Device.GetType().Name}) | Hạn trả: {item.DueDate:dd/MM/yyyy} | Ngày trả: {item.ReturnedDate:dd/MM/yyyy} | Trễ: {lateDays} ngày");
        }
        Console.WriteLine("     -- Các thiết bị chưa trả --");
        var unreturnedItems = slip1.Items.Where(i => !i.IsReturned);
        foreach (var item in unreturnedItems)
        {
            Console.WriteLine($"- Phiếu: {slip1.SlipId} | Thiết bị: {item.Device.DeviceId} ({item.Device.GetType().Name}) | Hạn trả: {item.DueDate:dd/MM/yyyy}");
        }

        // II. Luồng phát sinh ngoại lệ
        SeperateLine();
        Console.WriteLine("=== II. Luồng phát sinh ngoại lệ ===");
        SeperateLine();

        // 1. Mượn điều khiển điều hòa đang bảo trì
        Console.WriteLine(" 1. Mượn điều khiển điều hòa đang bảo trì");
        var acr2 = new AirConditionerRemote("ACR002", "Điều khiển Panasonic", 2022, DeviceStatus.UnderMaintenance, 2);
        var lec2 = new Lecturer("LEC002", "Nguyễn Văn A");
        var slip2 = new BorrowingSlip("SLIP002", lec2, "D5-401", DateTime.Today);

        try
        {
            slip2.AddDevice(acr2, 1);
        }
        catch (DeviceUnavailableException ex)
        {
            Console.WriteLine($"{ex.Message} (Mã TB: {ex.DeviceId}, Trạng thái: {ex.CurrentStatus})");
        }

        // 2. Trợ giảng mượn thiết bị thứ ba
        Console.WriteLine(" 2. Trợ giảng mượn thiết bị thứ ba");
        var ta1 = new TeachingAssistant("TA001", "Trần Văn B");
        var slip3 = new BorrowingSlip("SLIP102", ta1, "B202", DateTime.Today);

        var d1 = new Microphone("MIC101", "Micro 1", 2024, DeviceStatus.Available, 1);
        var d2 = new ProjectorRemote("PJR101", "Máy chiếu 1", 2023, DeviceStatus.Available, 2);
        var d3 = new AirConditionerRemote("ACR101", "Điều hòa 1", 2023, DeviceStatus.Available, 2);

        try
        {
            slip3.AddDevice(d1, 1);
            slip3.AddDevice(d2, 1);
            slip3.AddDevice(d3, 1);
        }
        catch (BorrowLimitExceededException ex)
        {
            Console.WriteLine($"{ex.Message} (Người mượn: {ex.BorrowerId}, Số lượng mượn tối đa: {ex.MaximumDevices})");
            Console.WriteLine($"Thiết bị được thêm gần nhất trong phiếu mượn sẽ bị xóa.");
        }

        // 3. Thêm cùng một micro hai lần
        Console.WriteLine(" 3. Thêm cùng một micro hai lần");
        var lec3 = new Lecturer("LEC111", "Nguyễn Văn B");
        var slip4 = new BorrowingSlip("SLIP103", lec3, "C7-101", DateTime.Today);
        var mic2 = new Microphone("MIC201", "Micro AKG", 2024, DeviceStatus.Available, 1);

        try
        {
            slip4.AddDevice(mic2, 1);
            slip4.AddDevice(mic2, 1);
        }
        catch (DuplicateDeviceException ex)
        {
            Console.WriteLine($"{ex.Message} (Mã thiết bị: {ex.DeviceId}, Mã phiếu: {ex.SlipId})");
        }
        
        // 4. Mượn micro trong 3 ngày dù giới hạn là 1 ngày.
        Console.WriteLine(" 4. Mượn micro trong 3 ngày dù giới hạn là 1 ngày");
        var lec4 = new Lecturer("LEC401", "Nguyễn Văn A");
        var slip5 = new BorrowingSlip("SLIP104", lec4, "C9-101", DateTime.Today);
        var mic3 = new Microphone("MIC301", "Micro AKG", 2024, DeviceStatus.Available, 1);

        try
        {
            slip5.AddDevice(mic3, 3);
        }
        catch (InvalidBorrowPeriodException ex)
        {
            Console.WriteLine($"{ex.Message} (Mã TB: {ex.DeviceId}, Ngày yêu cầu: {ex.BorrowDays}, Tối đa: {ex.MaxBorrowDays})");
        }

        // 5. Trả một điều khiển máy chiếu không thuộc phiếu.
        Console.WriteLine(" 5. Trả một điều khiển máy chiếu không thuộc phiếu");
        var lec5 = new Lecturer("LEC501", "Nguyễn Văn A");
        var slip6 = new BorrowingSlip("SLIP105", lec5, "C9-101", DateTime.Today);
        var mic4 = new Microphone("MIC401", "Micro AKG", 2024, DeviceStatus.Available, 1);
        slip6.AddDevice(mic4, 1);

        try
        {
            slip6.ReturnDevice("PR99", DateTime.Today); // PR99 không nằm trong phiếu
        }
        catch (DeviceNotInSlipException ex)
        {
            Console.WriteLine($"{ex.Message} (Mã TB: {ex.DeviceId}, Mã phiếu: {ex.SlipId})");
        }

        // 6. Trả cùng một thiết bị hai lần.
        Console.WriteLine(" 6. Trả cùng một thiết bị hai lần");
        var lec6 = new Lecturer("LEC601", "Nguyễn Văn A");
        var slip7 = new BorrowingSlip("SLIP106", lec6, "C9-101", DateTime.Today);
        var mic5 = new Microphone("MIC601", "Micro AKG", 2024, DeviceStatus.Available, 1);
        slip7.AddDevice(mic5, 1);

        try
        {
            slip7.ReturnDevice("MIC601", DateTime.Today);
            slip7.ReturnDevice("MIC601", DateTime.Today);
        }
        catch (DeviceAlreadyReturnedException ex)
        {
            Console.WriteLine($"{ex.Message} (Mã TB: {ex.DeviceId}, Mã phiếu: {ex.SlipId})");
        }

        // 7. Trả thiết bị với ngày trả trước ngày mượn.
        Console.WriteLine(" 7. Trả thiết bị với ngày trả trước ngày mượn");
        DateTime borrowDate = new DateTime(2026, 10, 10);
        var lec7 = new Lecturer("GV01", "Nguyễn Văn A");
        var slip8 = new BorrowingSlip("SLIP107", lec7, "A101", borrowDate);
        var mic6 = new Microphone("MIC01", "Micro AKG", 2024, DeviceStatus.Available, 1);

        slip8.AddDevice(mic6, 1);
        try
        {
            slip8.ReturnDevice("MIC01", new DateTime(2026, 10, 05)); // Ngày trả 05/10 trước ngày mượn 10/10
        }
        catch (BorrowingException ex)
        {
            Console.WriteLine($"{ex.Message}");
        }       
    }
    static void SeperateLine()
    {
        Console.WriteLine(new string('-', 30));
    }
}