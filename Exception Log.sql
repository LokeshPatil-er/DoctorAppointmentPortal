CREATE TABLE DAP_ExceptionLog
(
    ExceptionLogId INT IDENTITY(1,1) PRIMARY KEY,
    ExceptionMessage NVARCHAR(MAX) NOT NULL,
    ExceptionStackTrace NVARCHAR(MAX) NULL,
    InnerException NVARCHAR(MAX) NULL,
    Source NVARCHAR(MAX) NULL,
    TargetSite NVARCHAR(MAX) NULL,
    UserId INT FOREIGN KEY REFERENCES DAP_Users(UserId) NULL,
    LoggedOn DATETIME DEFAULT GETDATE(),
    AdditionalInfo NVARCHAR(MAX) NULL,
    IsResolved BIT DEFAULT 0
);

GO

CREATE OR ALTER PROC dap_exceptionLogInsert
(
    @ExceptionMessage NVARCHAR(MAX),
    @ExceptionStackTrace NVARCHAR(MAX) = NULL,
    @InnerException NVARCHAR(MAX) = NULL,
    @Source NVARCHAR(MAX) = NULL,
    @TargetSite NVARCHAR(MAX) = NULL,
    @UserId INT = NULL,
    @AdditionalInfo NVARCHAR(MAX) = NULL
)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        INSERT INTO DAP_ExceptionLog
        (
            ExceptionMessage,
            ExceptionStackTrace,
            InnerException,
            Source,
            TargetSite,
            UserId,
            AdditionalInfo
        )
        VALUES
        (
            @ExceptionMessage,
            @ExceptionStackTrace,
            @InnerException,
            @Source,
            @TargetSite,
            @UserId,
            @AdditionalInfo
        );
    END TRY
    BEGIN CATCH
       
   
    END CATCH
END
GO

-- Test 1: Simple insert without optional fields
EXEC dap_exceptionLogInsert
    @ExceptionMessage = N'Test Exception: NullReferenceException occurred',
    @ExceptionStackTrace = N'at AppointmentService.GetAvailableSlots() line 45',
    @InnerException = N'Object reference not set to an instance of an object',
    @Source = N'AppointmentService',
    @TargetSite = N'GetAvailableSlots()',
    @UserId = 1,  -- Assuming UserId=1 exists in DAP_Users
    @AdditionalInfo = N'Occurred while fetching available slots for DoctorId=3'
   

-- ✅ Check inserted data
SELECT TOP 10 * 
FROM DAP_ExceptionLog
ORDER BY ExceptionLogId DESC;
