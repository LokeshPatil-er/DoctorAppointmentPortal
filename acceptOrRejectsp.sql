

CREATE OR ALTER  PROCEDURE dap_appointmentAcceptedSlotsGetByDate  
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

CREATE  OR ALTER  PROCEDURE dap_appointmentUpdateStatus  
    @AppointmentId INT,  
    @NewAppointmentStatus NVARCHAR(20),        
 @PreferredSlotId INT,  
    @ModifiedBy INT,  
    @AlternateDate DATE = NULL,        
    @AlternateStartTime TIME = NULL,   
    @AlternateEndTime TIME = NULL     
AS  
BEGIN  
     
    BEGIN TRY  
        BEGIN TRANSACTION;  
  
        DECLARE @AppointmentStatusId INT;  
  
         
        IF @NewAppointmentStatus IS NOT NULL AND @NewAppointmentStatus <>''  
   BEGIN  
    SELECT @AppointmentStatusId =AppointmentStatusId FROM DAP_AppointmentStatus WHERE ShortCode = @NewAppointmentStatus;  
   END  
         
         
  
        -------------------------------------------------  
        --  Update Appointment Status  
        -------------------------------------------------  
        UPDATE DAP_Appointments  
        SET AppointmentStatusId = @AppointmentStatusId,  
            ModifiedBy = @ModifiedBy,  
            ModifiedOn = SYSDATETIME()  
        WHERE AppointmentId = @AppointmentId;  
  
        -------------------------------------------------  
        --  Update Preferred Slots based on Action  
        -------------------------------------------------  
        IF @NewAppointmentStatus = 'ACPT'  
        BEGIN  
             
            UPDATE DAP_PreferredSlots  
            SET IsApproved = 1,  
                ModifiedBy = @ModifiedBy,  
                ModifiedOn = SYSDATETIME()  
            WHERE AppointmentId = @AppointmentId AND  
      PreferredSlotId=@PreferredSlotId;  
  
  
        END  
        ELSE IF @NewAppointmentStatus = 'ALTS'  
        BEGIN  
            -- Add new alternate slot  
            INSERT INTO DAP_PreferredSlots  
            (  
                AppointmentId,  
                PreferredDate,  
                PreferredStartTime,  
                PreferredEndTime,  
                IsApproved,  
				IsAlternateSlot,  
                CreatedBy,  
                CreatedOn  
            )  
            VALUES  
            (  
                @AppointmentId,  
                @AlternateDate,  
                @AlternateStartTime,  
                @AlternateEndTime,  
                0,     
				1,  
                @ModifiedBy,  
                SYSDATETIME()  
            );  
        END  
  
        COMMIT TRANSACTION;  
    END TRY  
    BEGIN CATCH  
        ROLLBACK TRANSACTION;  
    END CATCH  
END  