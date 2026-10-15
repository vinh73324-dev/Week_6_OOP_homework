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
        return $"Mã thiết bị: {DeviceId}\nTên thiết bị: {DeviceName}\nGiá mua: {PurchasePrice}\nNăm đưa vào sử dụng: {PurchaseYear}\nTrạng thái hoạt động: {Status}\n";
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
        int toPeriod,
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
        
    }
    #endregion
}

public class BorrowingSlip
{
    
}
