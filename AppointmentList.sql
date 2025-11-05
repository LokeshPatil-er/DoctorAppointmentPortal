

CREATE OR ALTER PROC dap_appointmentsGetAllOrByDoctorId 
/*
-----------------------------------------------------------------------------------------------------
DATE			Created By		Purpose of creation
13 OCT 2025		Lokesh Patil	To get all appointments info or based on DoctorId,AppointmentStatus
								Date Range with pagination
------------------------------------------------------------------------------------------------------
*/
(
    @DoctorId INT = NULL,
    @AppointmentStatusShortCode NVARCHAR(20) = NULL,
	@SpecializationId INT=NULL,
	@PatientName NVARCHAR(100)=NULL,
    @FromDate DATE = NULL,
    @ToDate DATE = NULL,
    @PageNumber INT = 1,
    @PageSize INT = 10
)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @AppointmentStatusId INT;

   
    IF @AppointmentStatusShortCode IS NOT NULL AND @AppointmentStatusShortCode <> ''
        SELECT @AppointmentStatusId = AppointmentStatusId 
        FROM DAP_AppointmentStatus
        WHERE ShortCode = @AppointmentStatusShortCode;

    ------------------------------------------------------------
    -- Create and insert filtered records into temp table
    ------------------------------------------------------------
    SELECT 
        a.AppointmentId,
        a.DoctorId,
        d.FirstName AS DoctorFirstName,
        d.LastName AS DoctorLastName,
        p.PatientId,
        p.FirstName AS PatientFirstName,
        p.LastName AS PatientLastName,
        g.Gender AS Gender,
        p.DOB,
        bg.BloodGroupName,
        p.ContactNo,
        p.Email,
        addr.AddressLine1,
        addr.Pincode,
        t.TalukaName,
        a.ReasonForVisit,
        ast.ShortCode AS AppointmentStatusShortCode,
        pii.ProviderName AS InsuranceProvider,
        pii.PolicyNumber,
        pii.PolicyName,
        pii.ValidTill AS InsuranceValidTill,
        a.MedicalHistory,   
        ROW_NUMBER() OVER (ORDER BY a.AppointmentId ASC) AS RowNum
    INTO #AppointmentsTemp
    FROM DAP_Appointments a
        INNER JOIN DAP_Patients p ON a.PatientId = p.PatientId
        INNER JOIN DAP_Doctors d ON a.DoctorId = d.DoctorId
        INNER JOIN DAP_Genders g ON p.GenderId = g.GenderId
        INNER JOIN DAP_BloodGroups bg ON p.BloodGroupId = bg.BloodGroupId
        INNER JOIN DAP_Addresses addr ON p.AddressId = addr.AddressId
        INNER JOIN DAP_Talukas t ON addr.TalukaId = t.TalukaId
        INNER JOIN DAP_AppointmentStatus ast ON a.AppointmentStatusId = ast.AppointmentStatusId
		LEFT JOIN DAP_DoctorSpecializations ds ON d.DoctorId = ds.DoctorId 
        LEFT JOIN DAP_PatientInsurancesInfo pii ON a.PatientInsuranceId = pii.InsuranceId
    WHERE 
        a.IsActive = 1 
        AND p.IsActive = 1
        AND (@DoctorId IS NULL OR a.DoctorId = @DoctorId)
        AND (@AppointmentStatusId IS NULL OR a.AppointmentStatusId = @AppointmentStatusId)
		AND (@SpecializationId IS NULL OR ds.SpecializationId = @SpecializationId)
        AND (@PatientName IS NULL OR 
            p.FirstName LIKE '%' + @PatientName + '%' OR 
            p.LastName LIKE '%' + @PatientName + '%'
        )
        AND (@FromDate IS NULL OR @ToDate IS NULL OR
				EXISTS (
					SELECT 1
					FROM DAP_PreferredSlots ps
					WHERE ps.AppointmentId = a.AppointmentId
					  AND ps.IsActive = 1
					  AND CAST(ps.PreferredDate AS DATE) =@FromDate
					  AND CAST(ps.PreferredDate AS DATE)=@ToDate
				));

    ------------------------------------------------------------
    -- Pagination logic
    ------------------------------------------------------------
    DECLARE @StartRow INT = ((@PageNumber - 1) * @PageSize) + 1;
    DECLARE @EndRow INT = @PageNumber * @PageSize;

    ------------------------------------------------------------
    -- Main appointments with pagination
    ------------------------------------------------------------
    SELECT 
        AppointmentId,
        DoctorId,
        DoctorFirstName,
        DoctorLastName,
        PatientId,
        PatientFirstName,
        PatientLastName,
        Gender,
        DOB,
        BloodGroupName,
        ContactNo,
        Email,
        AddressLine1,
        Pincode,
        TalukaName,
        ReasonForVisit,
        AppointmentStatusShortCode,
        InsuranceProvider,
        PolicyNumber,
        PolicyName,
        InsuranceValidTill,
        MedicalHistory
    FROM #AppointmentsTemp a
		
    WHERE RowNum BETWEEN @StartRow AND @EndRow
		 
		  
    ORDER BY a.AppointmentId ASC;

    ------------------------------------------------------------
    -- Related Preferred Slots
    ------------------------------------------------------------
    SELECT 
        ps.PreferredSlotId,
        ps.AppointmentId,
        ps.PreferredDate,
        ps.PreferredStartTime,
        ps.PreferredEndTime,
        ps.IsApproved,
		ps.IsAlternateSlot
    FROM DAP_PreferredSlots ps
        INNER JOIN #AppointmentsTemp a ON ps.AppointmentId = a.AppointmentId
    WHERE ps.IsActive = 1
		  AND a.RowNum BETWEEN @StartRow AND @EndRow
		  AND((@AppointmentStatusShortCode = 'ACPT' AND ps.IsApproved = 1)
			    OR
			  (@AppointmentStatusShortCode <> 'ACPT' OR @AppointmentStatusShortCode IS NULL)
			 )
    ORDER BY ps.AppointmentId, ps.PreferredDate, ps.PreferredStartTime;

    ------------------------------------------------------------
    -- Related Reports
    ------------------------------------------------------------
    SELECT 
        r.ReportId,
        r.AppointmentId,
        r.ReportName,
        r.ReportFileName,
        r.FileType
    FROM DAP_PatientExistingReports r
        INNER JOIN #AppointmentsTemp a ON r.AppointmentId = a.AppointmentId
    WHERE r.IsActive = 1
		  AND a.RowNum BETWEEN @StartRow AND @EndRow
    ORDER BY r.AppointmentId, r.ReportName;

	 ------------------------------------------------------------
	 -- Doctor Specializations
	 ------------------------------------------------------------
	 SELECT 
		ds.DoctorId,
		s.Specialization
	 FROM DAP_DoctorSpecializations ds
	 INNER JOIN DAP_Specializations s ON ds.SpecializationId = s.SpecializationId
	 WHERE ds.IsActive = 1
		AND ds.DoctorId IN (SELECT DoctorId FROM #AppointmentsTemp)
	 ORDER BY ds.DoctorId, s.Specialization;

  
    SELECT COUNT(1) AS TotalRecords FROM #AppointmentsTemp;

    DROP TABLE #AppointmentsTemp;
END
GO

