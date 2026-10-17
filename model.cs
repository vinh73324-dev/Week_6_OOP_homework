/************************
Họ và tên: Phạm Văn Vinh
MSSV: 202419018
*************************/

using System;
using System.Reflection.Metadata;

public enum DeviceStatus
{
    Available,
    Borrowed,
    UnderMaintenance
}

public abstract class TeachingDevice
{
    public string DeviceId {get;}
    protected string DeviceName {get; set;}
    protected int PurchaseYear {get; set;}
    public DeviceStatus Status {get; set;}
    public int MaxBorrowDays {get;}

    #region Constructor
    public TeachingDevice(
        string id,
        string name,
        int year,
        DeviceStatus status,
        int maxBorrowDays
    )
    {
        if (string.IsNullOrWhiteSpace(id))
            throw new ArgumentException("Mã thiết bị không được rỗng.");
        if (CalculateYearsOfUse() <= 0)
            throw new ArgumentOutOfRangeException("Năm đưa vào sử dụng không được lớn hơn năm hiện tại.");
        if (maxBorrowDays <= 0)
            throw new ArgumentOutOfRangeException("Số ngày mượn tối đa phải dương");

        DeviceId = id;
        DeviceName = name;
        PurchaseYear = year;
        Status = status;
        MaxBorrowDays = maxBorrowDays;
    }
    #endregion

    #region Additional method
    //Tính số năm sử dụng
    public int CalculateYearsOfUse()
    {
        int yearsUsed = DateTime.Now.Year - PurchaseYear;
        return yearsUsed;
    }
    #endregion

    #region Override method
    // Ghi đè ToString để trả về thông tin của thiết bị
    public override string ToString()
    {
        return $"Mã thiết bị: {DeviceId}\nTên thiết bị: {DeviceName}\nNăm đưa vào sử dụng: {PurchaseYear}\nTrạng thái hoạt động: {Status}\n";
    }
    #endregion
}

public class AirConditionerRemote: TeachingDevice
{
    public AirConditionerRemote(string id, string name, int year, DeviceStatus status, int maxBorrowDays)
    : base(id, name, year, status, maxBorrowDays)
    {
    }
}


public class Microphone: TeachingDevice
{
    public Microphone(string id, string name, int year, DeviceStatus status, int maxBorrowDays)
    : base(id, name, year, status, maxBorrowDays)
    {
    }    
}

public class ProjectorRemote: TeachingDevice
{
    public ProjectorRemote(string id, string name, int year, DeviceStatus status, int maxBorrowDays)
    : base(id, name, year, status, maxBorrowDays)
    {
    }   
}

public class ClassRoom
{
    public string RoomId;
    public string RoomName;
    public int Capacity;
    public List<TeachingDevice> Devices;

    #region Constructor
    public ClassRoom(string id, string name, int capacity)
    {
        if (string.IsNullOrWhiteSpace(id))
            throw new ArgumentException("Mã phòng thí nghiệm không được để trống");
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Tên phòng thí nghiệm không được để trống");
        if (capacity <= 0)
            throw new ArgumentOutOfRangeException("Sức chứa phải dương");
        RoomId = id;
        RoomName = name;
        Capacity = capacity;
        Devices = new List<TeachingDevice>();    
    }
}

public interface IBorrower
{    
    string BorrowerId { get; }
    string FullName { get; }
    int GetMaximumDevices();
}

public class Lecturer: IBorrower
{
    public string BorrowerId {get;}
    public string FullName {get;}

    #region Constructor
    public Lecturer(string id, string name)
    {
        if (string.IsNullOrWhiteSpace(id))
            throw new ArgumentException("Mã người mượn không được rỗng.");
        BorrowerId = id;
        FullName = name;
    }
    #endregion

    #region public method
    public int GetMaximumDevices()
    {
        return 3;
    }    
    #endregion
}

public class TeachingAssistant: IBorrower
{
    public string BorrowerId {get;}
    public string FullName {get;}
    
    public TeachingAssistant(string id, string name)
    {
        if (string.IsNullOrWhiteSpace(id))
            throw new ArgumentException("Mã người mượn không được rỗng.");
        BorrowerId = id;
        FullName = name;
    }
    #endregion

    #region public method
    public int GetMaximumDevices()
    {
        return 2;
    }    
    #endregion
}

public class BorrowItem
{
    public TeachingDevice Device {get;}
    public DateTime BorrowedDate {get;}
    public DateTime DueDate {get;}
    public int FromPeriod {get; set;}
    public int ToPeriod {get; set;}
    public DateTime? ReturnedDate {get; private set;}
    public bool IsReturned {get; private set;}

    #region Constructor
    public BorrowItem(
        TeachingDevice device,
        DateTime borrowedDate, 
        DateTime dueDate,
        int fromPeriod,
        int toPeriod
    )
    {
        if (device == null)
            throw new ArgumentNullException("Thiết bị không thể trống");
        if (dueDate < borrowedDate)
            throw new ArgumentOutOfRangeException("Ngày hẹn trả không thể xảy ra trước ngày mượn.");
        if (fromPeriod > toPeriod)
            throw new ArgumentOutOfRangeException("Tiết trả không thể xảy ra trước tiết mượn");
        Device = device;
        BorrowedDate = borrowedDate;
        DueDate = dueDate;
        FromPeriod = fromPeriod;
        ToPeriod = toPeriod;
        ReturnedDate = null;
        IsReturned = false;
    }
    #endregion

    #region public method
    public void Return(DateTime returnedDate)
    {
        if (IsReturned)
            throw new ArgumentException("Thiết bị đã được trả trước đó.");
        if (returnedDate < BorrowedDate)
            throw new ArgumentOutOfRangeException("Không thể trả vào trước ngày mượn.");
        ReturnedDate = returnedDate;
        Device.Status = DeviceStatus.Available;
        
    }
    #endregion
}

public class BorrowingSlip
{
    public string SlipId { get; }
    public IBorrower Borrower { get; }
    public string TeachingRoom { get; }
    public DateTime BorrowedDate { get; }
    private readonly List<BorrowItem> _items = new List<BorrowItem>();
    public IReadOnlyList<BorrowItem> Items => _items.AsReadOnly();

    #region Constructor
    public BorrowingSlip(
        string id,
        IBorrower borrower,
        string teachingRoom,
        DateTime borrowedDate
    )
    {
        if (string.IsNullOrWhiteSpace(id))
            throw new ArgumentException("Mã phiếu mượn không được rỗng.");
        if (borrower == null)
            throw new ArgumentNullException("Người mượn không được null.");
        if (string.IsNullOrWhiteSpace(teachingRoom))
            throw new ArgumentException("Tên phòng dạy không được để trống.");
        
        SlipId = id;
        Borrower = borrower;
        TeachingRoom = teachingRoom;
        BorrowedDate = borrowedDate;
    }
    #endregion

    #region Public method
    public void AddDevice(TeachingDevice device, int borrowDays, int fromPeriod = 1, int toPeriod = 12)
    {
        ValidateAddDevice(device, borrowDays);
        
        DateTime dueDate = BorrowedDate.AddDays(borrowDays);
        var item = new BorrowItem(device, BorrowedDate,dueDate, fromPeriod, toPeriod);
        device.Status = DeviceStatus.Borrowed;

        _items.Add(item);

    }
    public decimal ReturnDevice(string deviceId, DateTime returnedDate)
    {
        if (string.IsNullOrWhiteSpace(deviceId))
            throw new ArgumentException("Mã thiết bị không được để trống");
        
        // Kiểm tra có mã thiết bị tương ứng trong danh sách hay không
        var item = _items.FirstOrDefault(i => i.Device.DeviceId.Equals(deviceId, StringComparison.OrdinalIgnoreCase));
        if (item == null)
            throw new DeviceNotInSlipException("Thiết bị không trong phiếu mượn.", deviceId, SlipId);
        // Kiểm tra thiết bị đã được trả trước đó chưa
        if (item.IsReturned)
            throw new DeviceAlreadyReturnedException("Thiết bị đã được trả trước đó.", deviceId, SlipId);
        // Thực hiện trả
        item.Return(returnedDate);
        
        // Tính phí trả muộn với thiết bị
        return CalculateLateFeeForItem(item);
    }
    public decimal CalculateTotalLateFee()
    {
        decimal totalFee = 0m;
        foreach (var item in _items)
        {
            totalFee += CalculateLateFeeForItem(item);
        }
        return totalFee;
    }

    #endregion


    #region Additional method
    // Kiểm tra đầu vào trước khi thêm thiết bị vào phiếu
    private void ValidateAddDevice(TeachingDevice device, int borrowDays)
    {
        if (device == null)
            throw new ArgumentNullException("Thiết bị không được null.");
        if (borrowDays <= 0)
            throw new InvalidBorrowPeriodException("Số ngày mượn phải là số dương.", device.DeviceId, borrowDays, device.MaxBorrowDays);
        if (borrowDays > device.MaxBorrowDays)
            throw new InvalidBorrowPeriodException("Không thể mượn quá giới hạn ngày mượn của thiết bị", device.DeviceId, borrowDays, device.MaxBorrowDays);
        // Kiểm tra trùng lặp mã
        bool isDuplicate = _items.Any(i => i.Device.DeviceId.Equals(device.DeviceId, StringComparison.OrdinalIgnoreCase));
        if (isDuplicate)
            throw new DuplicateDeviceException("Thiết bị đã có trong phiếu mượn.", device.DeviceId, SlipId);
        if (_items.Count >= Borrower.GetMaximumDevices())
            throw new BorrowLimitExceededException("Người mượn đã đạt tối đa số thiết bị có thể mượn.", Borrower.BorrowerId, Borrower.GetMaximumDevices());
        if (device.Status != DeviceStatus.Available)
            throw new DeviceUnavailableException("Thiết bị hiện tại không thể mượn.", device.DeviceId, device.Status);
    }
    
    private decimal CalculateLateFeeForItem(BorrowItem item)
    {
        if (!item.ReturnedDate.HasValue || item.ReturnedDate.Value <= item.DueDate)
            return 0m;
        
        int lateDays = (item.ReturnedDate.Value - item.BorrowedDate).Days;
        // Giả sử tiền phạt trả muộn mỗi ngày là 50k VNĐ
        decimal dailyLateFee = 50000m;

        return lateDays * dailyLateFee;
    }
    #endregion
}
