/************************
Họ và tên: Phạm Văn Vinh
MSSV: 202419018
*************************/

using System;

public class BorrowingException : Exception
{
    public BorrowingException(string message)
    : base(message)
    {
    }
    public BorrowingException(
        string message,
        Exception? innerException
    )
    : base(message, innerException)
    {
    }
}

public class DeviceUnavailableException : BorrowingException
{
    public string DeviceId { get; }
    public DeviceStatus CurrentStatus { get; }

    public DeviceUnavailableException(
        string message,
        Exception innerException,
        string id,
        DeviceStatus status
    )
    : base(message, innerException)
    {
        DeviceId = id;
        CurrentStatus = status;
    }
}

public class BorrowLimitExceededException : BorrowingException
{
    public string BorrowerId { get; }
    public int MaximumDevices { get; }

    public BorrowLimitExceededException(
        string message,
        Exception? innerException,
        string id,
        int maxDevices
    )
    : base(message, innerException)
    {
        BorrowerId = id;
        MaximumDevices = maxDevices;
    }


    public BorrowLimitExceededException(
        string message,
        string id,
        int maxDevices
    )
    : this(message, null, id, maxDevices)
    {
    }
}

public class InvalidBorrowPeriodException: BorrowingException
{
    public string DeviceId { get; }
    public int BorrowDays { get; }
    public int MaxBorrowDays { get; }

    public InvalidBorrowPeriodException(
        string message,
        Exception? innerException,
        string id,
        int borrowDays,
        int maxDays
    )
    : base(message, innerException)
    {
        DeviceId = id;
        BorrowDays = borrowDays;
        MaxBorrowDays = maxDays;
    }

    public InvalidBorrowPeriodException(
        string message,
        string id,
        int borrowDays,
        int maxDays
    )
    : this(message, null, id, borrowDays, maxDays)
    {
    }
}

public class DuplicateDeviceException: BorrowingException
{
    public string DeviceId { get; }
    public string SlipId { get; }

    public DuplicateDeviceException(
        string message,
        Exception? innerException,
        string deviceId,
        string slipId
    )
    : base(message, innerException)
    {
        DeviceId = deviceId;
        SlipId = slipId;
    }

    public DuplicateDeviceException(
        string message,
        string deviceId,
        string slipId
    )
    : this(message, null, deviceId, slipId)
    {
    }
}

public class DeviceNotInSlipException: BorrowingException
{
    public string DeviceId { get; }
    public string SlipId { get; }

    public DeviceNotInSlipException(
        string message,
        Exception? innerException,
        string deviceId,
        string slipId
    )
    : base(message, innerException)
    {
        DeviceId = deviceId;
        SlipId = slipId;
    }

    public DeviceNotInSlipException(
        string message,
        string deviceId,
        string slipId
    )
    : this(message, null, deviceId, slipId)
    {
    }
}

public class DeviceAlreadyReturnedException: BorrowingException
{
    public string DeviceId { get; }
    public string SlipId { get; }

    public DeviceAlreadyReturnedException(
        string message,
        Exception? innerException,
        string deviceId,
        string slipId
    )
    : base(message, innerException)
    {
        DeviceId = deviceId;
        SlipId = slipId;
    }    

    public DeviceAlreadyReturnedException(
        string message,
        string deviceId,
        string slipId
    )
    : this(message, null, deviceId, slipId)
    {
    }  
}