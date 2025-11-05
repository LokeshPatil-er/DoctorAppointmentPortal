CREATE    PROCEDURE dap_appointmentAcceptedSlotsGetByDate    
(    
    @DoctorId INT,    
    @RequestedDate DATE    
)    
AS    
BEGIN    
    SET NOCOUNT ON;    
    
    SELECT     
        PS.PreferredSlotId,    
        PS.AppointmentId,    
		A.DoctorId,  
		A.PatientId,  
        PS.PreferredDate,    
        PS.PreferredStartTime,    
        PS.PreferredEndTime,    
        PS.IsApproved,    
        A.AppointmentStatusId,    
        AST.Status AS AppointmentStatus,    
        AST.ShortCode    
    FROM DAP_PreferredSlots PS    
    INNER JOIN DAP_Appointments A     
        ON PS.AppointmentId = A.AppointmentId    
    LEFT JOIN DAP_AppointmentStatus AST     
        ON A.AppointmentStatusId = AST.AppointmentStatusId    
    WHERE     
        A.DoctorId = @DoctorId    
        AND PS.PreferredDate = @RequestedDate    
        AND PS.IsActive = 1    
        AND A.IsActive = 1    
        AND PS.IsApproved = 1    
        AND AST.ShortCode = 'ACPT'         
    ORDER BY     
        PS.PreferredStartTime;    
END;

GO

CREATE   PROCEDURE dap_doctorAvailableSlotsGetByDate  
(  
    @DoctorId INT,  
    @RequestedDate DATE  
)  
AS  
BEGIN  
    SET NOCOUNT ON;  
  
    DECLARE @DayOfWeek NVARCHAR(20);  
    SET @DayOfWeek = DATENAME(WEEKDAY, @RequestedDate);  
  
    SELECT   
        DAS.SlotId,  
        DAS.DoctorId,  
        DAS.DayOfWeek,  
        DAS.StartTime,  
        DAS.EndTime,  
        DAS.IsActive  
    FROM DAP_DoctorAvailableSlots DAS  
    WHERE   
        DAS.DoctorId = @DoctorId  
        AND DAS.DayOfWeek = @DayOfWeek  
        AND DAS.IsActive = 1  
    ORDER BY   
        DAS.StartTime;  
END;  

GO

CREATE OR ALTER PROC dap_GetDoctorAvailabilityAndAcceptedSlots
(
    @DoctorId INT,
    @RequestedDate DATE
)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @DayOfWeek NVARCHAR(20);
    SET @DayOfWeek = DATENAME(WEEKDAY, @RequestedDate);

    -- Doctor available slots
    SELECT 
        DAS.SlotId,
        DAS.DoctorId,
        DAS.DayOfWeek,
        DAS.StartTime,
        DAS.EndTime,
        DAS.IsActive
    FROM DAP_DoctorAvailableSlots DAS
    WHERE 
        DAS.DoctorId = @DoctorId
        AND DAS.DayOfWeek = @DayOfWeek
        AND DAS.IsActive = 1
    ORDER BY 
        DAS.StartTime;

    -- Accepted appointments
    SELECT     
        PS.PreferredSlotId,    
        PS.AppointmentId,    
        A.DoctorId,  
        A.PatientId,  
        PS.PreferredDate,    
        PS.PreferredStartTime,    
        PS.PreferredEndTime,    
        PS.IsApproved,    
        A.AppointmentStatusId,    
        AST.Status AS AppointmentStatus,    
        AST.ShortCode    
    FROM DAP_PreferredSlots PS    
    INNER JOIN DAP_Appointments A     
        ON PS.AppointmentId = A.AppointmentId    
    LEFT JOIN DAP_AppointmentStatus AST     
        ON A.AppointmentStatusId = AST.AppointmentStatusId    
    WHERE     
        A.DoctorId = @DoctorId    
        AND PS.PreferredDate = @RequestedDate    
        AND PS.IsActive = 1    
        AND A.IsActive = 1    
        AND PS.IsApproved = 1    
        AND AST.ShortCode = 'ACPT'         
    ORDER BY     
        PS.PreferredStartTime;
END;
