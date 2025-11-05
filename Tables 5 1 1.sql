
--0
CREATE TABLE DAP_Genders
(
   GenderId INT PRIMARY KEY IDENTITY(1,1),
   Gender NVARCHAR(50) NOT NULL UNIQUE,
   IsActive BIT DEFAULT 1,
   CreatedBy INT FOREIGN KEY REFERENCES DAP_Users(UserId) NULL,
   CreatedOn DATETIME2 NOT NULL,
   ModifiedBy INT FOREIGN KEY REFERENCES DAP_Users(UserId) NULL,
   ModifiedOn DATETIME2 NULL,
);


--1
CREATE TABLE DAP_Roles
(
	RoleId INT PRIMARY KEY IDENTITY(1,1),
	[Role] NVARCHAR(50) NOT NULL UNIQUE,
	ShortCode NVARCHAR(10) NOT NULL,
	IsActive BIT DEFAULT 1,
	CreatedOn DATETIME2 NULL,
	ModifiedOn DATETIME2 NULL,
);

--2
CREATE TABLE DAP_Users
(
	UserId INT PRIMARY KEY IDENTITY(1,1),
	RoleId INT FOREIGN KEY REFERENCES DAP_Roles(RoleId) NOT NULL,
	Email NVARCHAR(320) NOT NULL,
	[Password] NVARCHAR(256) NOT NULL,
	IsActive BIT DEFAULT 1,
	IsFirstLogin BIT DEFAULT 0,
	CreatedBy INT FOREIGN KEY REFERENCES DAP_Users(UserId) NULL,
	CreatedOn DATETIME2 NOT NULL,
	ModifiedBy INT FOREIGN KEY REFERENCES DAP_Users(UserId) NULL,
	ModifiedOn DATETIME2 NULL,
	
);

--3
CREATE  TABLE DAP_Countries (
    CountryId INT IDENTITY(1,1) PRIMARY KEY,
    CountryName NVARCHAR(100) NOT NULL UNIQUE,
    IsActive BIT DEFAULT 1,
	CreatedBy INT FOREIGN KEY REFERENCES DAP_Users(UserId) NULL,
	CreatedOn DATETIME2 NOT NULL,
	ModifiedBy INT FOREIGN KEY REFERENCES DAP_Users(UserId) NULL,
	ModifiedOn DATETIME2 NULL,
);




--4
CREATE  TABLE DELETE FROM  DAP_States (
    StateId INT IDENTITY(1,1) PRIMARY KEY,
    StateName NVARCHAR(100) NOT NULL UNIQUE,
    CountryId INT NOT NULL FOREIGN KEY REFERENCES DAP_Countries(CountryId),
    IsActive BIT DEFAULT 1,
	CreatedBy INT FOREIGN KEY REFERENCES DAP_Users(UserId) NULL,
	CreatedOn DATETIME2 NOT NULL,
	ModifiedBy INT FOREIGN KEY REFERENCES DAP_Users(UserId) NULL,
	ModifiedOn DATETIME2 NULL,
);



--5
CREATE  TABLE DELETE FROM DAP_Districts (
    DistrictId INT IDENTITY(1,1) PRIMARY KEY,
    DistrictName NVARCHAR(100) NOT NULL UNIQUE,
    StateId INT NOT NULL FOREIGN KEY REFERENCES DAP_States(StateId),
    IsActive BIT DEFAULT 1,
	CreatedBy INT FOREIGN KEY REFERENCES DAP_Users(UserId) NULL,
	CreatedOn DATETIME2 NOT NULL,
	ModifiedBy INT FOREIGN KEY REFERENCES DAP_Users(UserId) NULL,
	ModifiedOn DATETIME2 NULL,
);

DBCC CHECKIDENT ('dbo.DAP_Districts', RESEED, 0);

--6
CREATE  TABLE DAP_Talukas (
    TalukaId INT IDENTITY PRIMARY KEY,
    TalukaName NVARCHAR(100) NOT NULL UNIQUE,
    DistrictId INT NOT NULL FOREIGN KEY REFERENCES DAP_Districts(DistrictId),
    IsActive BIT DEFAULT 1,
	CreatedBy INT FOREIGN KEY REFERENCES DAP_Users(UserId) NULL,
	CreatedOn DATETIME2 NOT NULL,
	ModifiedBy INT FOREIGN KEY REFERENCES DAP_Users(UserId) NULL,
	ModifiedOn DATETIME2 NULL,
);


--7
CREATE  TABLE DAP_Addresses (
    AddressId INT IDENTITY PRIMARY KEY,
    AddressLine1 NVARCHAR(200) NOT NULL,
    Pincode NVARCHAR(10) NOT NULL,
    TalukaId INT NOT NULL FOREIGN KEY REFERENCES DAP_Talukas(TalukaId),
	IsActive BIT DEFAULT 1,
	CreatedBy INT FOREIGN KEY REFERENCES DAP_Users(UserId) NULL,
	CreatedOn DATETIME2 NOT NULL,
	ModifiedBy INT FOREIGN KEY REFERENCES DAP_Users(UserId) NULL,
	ModifiedOn DATETIME2 NULL,
);

CREATE  TABLE DAP_Specializations (
    SpecializationId INT IDENTITY PRIMARY KEY,
    Specialization NVARCHAR(150) NOT NULL UNIQUE, 
	[Description] NVARCHAR(MAX) NOT NULL,
	IsActive BIT,
	CreatedBy INT FOREIGN KEY REFERENCES DAP_Users(UserId),
	CreatedOn DATETIME2,
	ModifiedBy INT FOREIGN KEY REFERENCES DAP_Users(UserId),
	ModifiedOn DATETIME2,
);



--9
CREATE  TABLE DAP_BloodGroups (
    BloodGroupId INT IDENTITY PRIMARY KEY,
    BloodGroupName NVARCHAR(100) NOT NULL UNIQUE,
    IsActive BIT DEFAULT 1,
	CreatedOn DATETIME2 NOT NULL,
);

ALTER TABLE DAP_BloodGroups
ALTER COLUMN BloodGroupName NVARCHAR(10) NOT NULL;


--10
CREATE  TABLE DAP_Qualifications (
    QualificationId INT PRIMARY KEY IDENTITY(1,1),
    Degree VARCHAR(100) NOT NULL UNIQUE,
    DegreeDescription VARCHAR(255),
	IsActive BIT DEFAULT 1,
	CreatedBy INT FOREIGN KEY REFERENCES DAP_Users(UserId) NULL,
	CreatedOn DATETIME2 NULL,
	ModifiedBy INT FOREIGN KEY REFERENCES DAP_Users(UserId) NULL,
	ModifiedOn DATETIME2 NULL
);

--11
CREATE  TABLE DAP_DoctorQualifications (
    DoctorQualificationId INT PRIMARY KEY IDENTITY(1,1),
    DoctorId INT FOREIGN KEY REFERENCES DAP_Doctors(DoctorId) NOT NULL,
    QualificationId INT FOREIGN KEY REFERENCES DAP_Qualifications(QualificationId) NOT NULL,
	[Description] NVARCHAR(MAX) NULL,
	IsActive BIT DEFAULT 1,
	CreatedBy INT FOREIGN KEY REFERENCES DAP_Users(UserId) NULL,
	CreatedOn DATETIME2 NULL,
	ModifiedBy INT FOREIGN KEY REFERENCES DAP_Users(UserId) NULL,
	ModifiedOn DATETIME2 NULL,
);

--12
CREATE  TABLE DAP_Doctors
(
	DoctorId INT PRIMARY KEY IDENTITY(1,1),
	UserId INT FOREIGN KEY REFERENCES DAP_Users(UserId) NOT NULL,
	FirstName NVARCHAR(50) NOT NULL,
	LastName NVARCHAR(50) NOT NULL,
	DOB DATE NOT NULL,
	ContactNo NVARCHAR(10) NOT NULL,
	BloodGroupId INT FOREIGN KEY REFERENCES DAP_BloodGroups(BloodGroupId) NOT NULL,
	GenderId INT FOREIGN KEY REFERENCES DAP_Genders(GenderId) NOT NULL,
	AddressId INT FOREIGN KEY REFERENCES DAP_Addresses(AddressId) NOT NULL,
	YearOfExperience INT NOT NULL,
	ConsultancyFee INT NOT NULL,
	IsActive BIT DEFAULT 1,
	CreatedBy INT FOREIGN KEY REFERENCES DAP_Users(UserId) NOT NULL,
	CreatedOn DATETIME2 NOT NULL,
	ModifiedBy INT FOREIGN KEY REFERENCES DAP_Users(UserId) NULL,
	ModifiedOn DATETIME2 NULL,

);

CREATE  TABLE DAP_DoctorSpecializations (
  DoctorSpecializationId INT PRIMARY KEY IDENTITY(1,1),
  DoctorId INT FOREIGN KEY REFERENCES DAP_Doctors(DoctorId),
  SpecializationId INT FOREIGN KEY REFERENCES DAP_Specializations(SpecializationId),
  IsActive BIT DEFAULT 1,
  CreatedBy INT FOREIGN KEY REFERENCES DAP_Users(UserId) NOT NULL,
  CreatedOn DATETIME2 NOT NULL,
  ModifiedBy INT FOREIGN KEY REFERENCES DAP_Users(UserId) NULL,
  ModifiedOn DATETIME2 NULL,
);

--13
CREATE  TABLE DAP_DoctorAvailableSlots
(
	SlotId INT PRIMARY KEY IDENTITY(1,1),
	DoctorId INT FOREIGN KEY REFERENCES DAP_Doctors(DoctorId) NOT NULL,
	[DayOfWeek] NVARCHAR(20) NOT NULL,
	StartTime TIME NOT NULL,
	EndTime TIME NOT NULL,
	IsActive BIT DEFAULT 1,
	CreatedBy INT FOREIGN KEY REFERENCES DAP_Users(UserId) NOT NULL,
	CreatedOn DATETIME2 NOT NULL,
	ModifiedBy INT FOREIGN KEY REFERENCES DAP_Users(UserId) NULL,
	ModifiedOn DATETIME2 NULL,
);


--14
CREATE TABLE DAP_Patients
(
	PatientId INT PRIMARY KEY IDENTITY(1,1),
	FirstName NVARCHAR(50) NOT NULL ,
	LastName NVARCHAR(50) NOT NULL ,
	GenderId INT FOREIGN KEY REFERENCES DAP_Genders(GenderId) NOT NULL,
	DOB DATE NOT NULL,
	BloodGroupId INT FOREIGN KEY REFERENCES DAP_BloodGroups(BloodGroupId) NOT NULL,
	AddressId INT FOREIGN KEY REFERENCES DAP_Addresses(AddressId) NOT NULL,
	ContactNo NVARCHAR(10) NOT NULL ,
	Email NVARCHAR(320) NOT NULL ,
	IsActive BIT DEFAULT 1,
	CreatedOn DATETIME NOT NULL,
);

--15
CREATE TABLE DAP_AppointmentStatus
(
	AppointmentStatusId INT PRIMARY KEY IDENTITY(1,1),
	[Status] NVARCHAR(50) NOT NULL,
	ShortCode NVARCHAR(10) NOT NULL,
	IsActive BIT DEFAULT 1,
	CreatedBy INT FOREIGN KEY REFERENCES DAP_Users(UserId) NULL,
	CreatedOn DATETIME2 NULL,
	ModifiedBy INT FOREIGN KEY REFERENCES DAP_Users(UserId) NULL,
	ModifiedOn DATETIME2 NULL,

);




--16
CREATE TABLE DAP_PatientInsurancesInfo (
    InsuranceId INT PRIMARY KEY IDENTITY(1,1),
    PatientId INT FOREIGN KEY REFERENCES DAP_Patients(PatientId) NOT NULL,
    ProviderName NVARCHAR(100) NOT NULL,
    PolicyNumber NVARCHAR(50) NOT NULL ,
	PolicyName NVARCHAR(255) NOT NULL,
    ValidTill DATE NULL,
    IsActive BIT DEFAULT 1,
	CreatedBy INT FOREIGN KEY REFERENCES DAP_Users(UserId) NULL,
	CreatedOn DATETIME2 NULL,
	ModifiedBy INT FOREIGN KEY REFERENCES DAP_Users(UserId) NULL,
	ModifiedOn DATETIME2 NULL,
);

--17
CREATE TABLE DAP_Appointments
(
	AppointmentId INT PRIMARY KEY IDENTITY(0,1),
	PatientId INT FOREIGN KEY REFERENCES DAP_Patients(PatientId) NOT NULL,
	DoctorId INT FOREIGN KEY REFERENCES DAP_Doctors(DoctorId) NOT NULL,
	ReasonForVisit NVARCHAR(MAX) NOT NULL,
	AppointmentStatusId INT FOREIGN KEY REFERENCES DAP_AppointmentStatus(AppointmentStatusId) NOT NULL,
	PatientInsuranceId INT FOREIGN KEY REFERENCES DAP_PatientInsurancesInfo(InsuranceId) NULL,
	MedicalHistory NVARCHAR(MAX),
	IsActive BIT DEFAULT 1,
	CreatedBy INT FOREIGN KEY REFERENCES DAP_Users(UserId) NULL,
	CreatedOn DATETIME2 NULL,
	ModifiedBy INT FOREIGN KEY REFERENCES DAP_Users(UserId) NULL,
	ModifiedOn DATETIME2 NULL,

);

--18
CREATE TABLE DAP_PreferredSlots
(
	PreferredSlotId INT PRIMARY KEY IDENTITY(0,1),
	AppointmentId INT FOREIGN KEY REFERENCES DAP_Appointments(AppointmentId),
	PreferredDate DATE NOT NULL,
	PreferredStartTime TIME NOT NULL,
	PreferredEndTime TIME NOT NULL,
	IsApproved BIT DEFAULT 0,
	IsActive BIT DEFAULT 1,
	CreatedBy INT FOREIGN KEY REFERENCES DAP_Users(UserId) NULL,
	CreatedOn DATETIME2 NULL,
	ModifiedBy INT FOREIGN KEY REFERENCES DAP_Users(UserId) NULL,
	ModifiedOn DATETIME2 NULL,
	
);



--19
CREATE TABLE DAP_PatientExistingReports
(
	ReportId INT PRIMARY KEY IDENTITY(0,1),
	AppointmentId INT FOREIGN KEY REFERENCES DAP_Appointments(AppointmentId),
	ReportName NVARCHAR(50),
	ReportFileName NVARCHAR(255),
	FileType NVARCHAR(50),
	IsActive BIT DEFAULT 1,
	CreatedBy INT FOREIGN KEY REFERENCES DAP_Users(UserId) NULL,
	CreatedOn DATETIME2 NULL,
	ModifiedBy INT FOREIGN KEY REFERENCES DAP_Users(UserId) NULL,
	ModifiedOn DATETIME2 NULL,
);

--20
CREATE TABLE NotificationEmailTrack
(
	NotificationEmailId INT PRIMARY KEY IDENTITY(0,1),
	EmailTo NVARCHAR(320) NOT NULL,
	EmailFrom NVARCHAR(320) NOT NULL,
	EmailBody NVARCHAR(MAX) NOT NULL,
	NotificationReason NVARCHAR(MAX) NOT NULL,
	IsSend BIT DEFAULT 1,
	CreatedBy INT FOREIGN KEY REFERENCES DAP_Users(UserId) NULL,
	CreatedOn DATETIME2 NULL,
	ModifiedBy INT FOREIGN KEY REFERENCES DAP_Users(UserId) NULL,
	ModifiedOn DATETIME2 NULL,
	
);


CREATE TYPE DAP_DoctorQualificationsTVP AS TABLE
(
    QualificationId INT
);
GO



CREATE TYPE DAP_DoctorAvailableSlotsTVP AS TABLE
(
	SlotId INT NULL,
    [DayOfWeek] NVARCHAR(20),
    StartTime TIME(3),
    EndTime TIME(3),
	IsAvailable  BIT
	
);
GO


CREATE TYPE DAP_DoctorSpecializationsTVP AS TABLE
(
    SpecializationId INT
	
);
GO

CREATE OR ALTER PROCEDURE dap_doctorInsertOrUpdate  
/* 
---------------------------------------------------------------------------------- 
Create Date		Created By			Purpose Of Creation 
10OCT2025		Lokesh Patil		To insert data into doctor related 
---------------------------------------------------------------------------------- 
*/
@DoctorId INT = NULL,  
@UserId INT = NULL,  
@RoleShortCode NVARCHAR(10),      
@Password NVARCHAR(256),  
@Email NVARCHAR(320),  
  
@FirstName NVARCHAR(50),  
@LastName NVARCHAR(50),  
@DOB DATE,  
@ContactNo NVARCHAR(10),  
@BloodGroupId INT,  
@GenderId INT,  
  
@AddressId INT = NULL,  
@AddressLine1 NVARCHAR(255),  
@TalukaId INT,  
@Pincode NVARCHAR(10),  
  
@YearOfExperience INT,  
@ConsultancyFee INT,  
  
@CreatedBy INT = NULL,  
@ModifiedBy INT = NULL,  
  
@AvailableSlotList DAP_DoctorAvailableSlotsTVP READONLY,  
@DoctorSpecializationList DAP_DoctorSpecializationsTVP READONLY,  
@DoctorQualificationList DAP_DoctorQualificationsTVP READONLY    
AS  
BEGIN  
    SET NOCOUNT ON;  
  
    DECLARE @RoleId INT;  
  
    BEGIN TRY  
        BEGIN TRANSACTION;  
  
        ----------------------------------------------------------------------------------
        -- USER TABLE (DAP_Users)
        ----------------------------------------------------------------------------------
        SELECT @RoleId = RoleId 
        FROM DAP_Roles 
        WHERE ShortCode = @RoleShortCode;  
  
        IF @UserId IS NULL  OR @UserId=0
        BEGIN  
            INSERT INTO DAP_Users ([Password], Email, RoleId, CreatedBy, CreatedOn)  
            VALUES (@Password, @Email, @RoleId, @CreatedBy, SYSDATETIME());  

            SET @UserId = SCOPE_IDENTITY();  
        END  
        ELSE  
        BEGIN  
            UPDATE DAP_Users  
            SET [Password] = @Password,  
                RoleId = @RoleId,  
                Email = @Email,  
                ModifiedBy = @ModifiedBy,  
                ModifiedOn = GETDATE()  
            WHERE UserId = @UserId;  
        END  
  
        ----------------------------------------------------------------------------------
        -- ADDRESS TABLE (DAP_Addresses)
        ----------------------------------------------------------------------------------
        IF @AddressId IS NULL OR @AddressId=0 
        BEGIN  
            INSERT INTO DAP_Addresses (AddressLine1, TalukaId, Pincode, CreatedBy, CreatedOn)  
            VALUES (@AddressLine1, @TalukaId, @Pincode, @CreatedBy, GETDATE());  

            SET @AddressId = SCOPE_IDENTITY();  
        END  
        ELSE  
        BEGIN  
            UPDATE DAP_Addresses  
            SET AddressLine1 = @AddressLine1,  
                TalukaId = @TalukaId,  
                Pincode = @Pincode,  
                ModifiedBy = @ModifiedBy,  
                ModifiedOn = GETDATE()  
            WHERE AddressId = @AddressId;  
        END  
  
        ----------------------------------------------------------------------------------
        -- DOCTOR TABLE (DAP_Doctors)
        ----------------------------------------------------------------------------------
        IF @DoctorId IS NULL  OR @DoctorId = 0
        BEGIN  
            INSERT INTO DAP_Doctors  
            (UserId, FirstName, LastName, DOB, ContactNo, BloodGroupId, GenderId, 
             AddressId, YearOfExperience, ConsultancyFee, CreatedBy, CreatedOn)  
            VALUES  
            (@UserId, @FirstName, @LastName, @DOB, @ContactNo, @BloodGroupId, @GenderId, 
             @AddressId, @YearOfExperience, @ConsultancyFee, @CreatedBy, GETDATE());  

            SET @DoctorId = SCOPE_IDENTITY();  
        END  
        ELSE  
        BEGIN  
            UPDATE DAP_Doctors  
            SET FirstName = @FirstName,  
                LastName = @LastName,  
                DOB = @DOB,  
                ContactNo = @ContactNo,  
                BloodGroupId = @BloodGroupId,  
                GenderId = @GenderId,  
                AddressId = @AddressId,  
                YearOfExperience = @YearOfExperience,  
                ConsultancyFee = @ConsultancyFee,  
                ModifiedBy = @ModifiedBy,  
                ModifiedOn = GETDATE()  
            WHERE DoctorId = @DoctorId;  
        END  
  
        ----------------------------------------------------------------------------------
        --  DOCTOR AVAILABLE SLOTS (DAP_DoctorAvailableSlots)
        ----------------------------------------------------------------------------------
        -- Deactivate slots not in the current list  
        UPDATE s  
        SET s.IsActive = 0,  
            s.ModifiedBy = @ModifiedBy,  
            s.ModifiedOn = SYSDATETIME()  
        FROM DAP_DoctorAvailableSlots s  
        WHERE s.DoctorId = @DoctorId  
          AND NOT EXISTS (  
              SELECT 1  
              FROM @AvailableSlotList tvp  
              WHERE tvp.DayOfWeek = s.DayOfWeek  
                AND tvp.StartTime = s.StartTime  
                AND tvp.EndTime = s.EndTime  
          )  
          AND s.IsActive = 1;  
  
        -- Reactivate slots if previously inactive  
        UPDATE s  
        SET s.IsActive = 1,  
            s.ModifiedBy = @ModifiedBy,  
            s.ModifiedOn = SYSDATETIME()  
        FROM DAP_DoctorAvailableSlots s  
        JOIN @AvailableSlotList tvp  
          ON s.DoctorId = @DoctorId   
         AND s.DayOfWeek = tvp.DayOfWeek   
         AND s.StartTime = tvp.StartTime   
         AND s.EndTime = tvp.EndTime  
        WHERE s.IsActive = 0;  
  
        -- Insert new available slots  
        INSERT INTO DAP_DoctorAvailableSlots 
            (DoctorId, DayOfWeek, StartTime, EndTime, IsActive, CreatedBy, CreatedOn)  
        SELECT @DoctorId, tvp.DayOfWeek, tvp.StartTime, tvp.EndTime, tvp.IsAvailable, @CreatedBy, SYSDATETIME()  
        FROM @AvailableSlotList tvp  
        WHERE NOT EXISTS (  
            SELECT 1 FROM DAP_DoctorAvailableSlots s  
            WHERE s.DoctorId = @DoctorId  
              AND s.DayOfWeek = tvp.DayOfWeek  
              AND s.StartTime = tvp.StartTime  
              AND s.EndTime = tvp.EndTime  
        );  
  
        ----------------------------------------------------------------------------------
        --  DOCTOR SPECIALIZATIONS (DAP_DoctorSpecializations)
        ----------------------------------------------------------------------------------
        -- Deactivate removed ones  
        UPDATE s  
        SET s.IsActive = 0,  
            s.ModifiedBy = @ModifiedBy,  
            s.ModifiedOn = SYSDATETIME()  
        FROM DAP_DoctorSpecializations s  
        WHERE s.DoctorId = @DoctorId  
          AND NOT EXISTS (  
              SELECT 1 FROM @DoctorSpecializationList tvp  
              WHERE tvp.SpecializationId = s.SpecializationId  
          )  
          AND s.IsActive = 1;  
  
        -- Insert new ones  
        INSERT INTO DAP_DoctorSpecializations (DoctorId, SpecializationId, CreatedBy, CreatedOn)  
        SELECT @DoctorId, tvp.SpecializationId, @CreatedBy, SYSDATETIME()  
        FROM @DoctorSpecializationList tvp  
        WHERE NOT EXISTS (  
            SELECT 1 FROM DAP_DoctorSpecializations s  
            WHERE s.DoctorId = @DoctorId  
              AND s.SpecializationId = tvp.SpecializationId  
        );  
  
        ----------------------------------------------------------------------------------
        --  DOCTOR QUALIFICATIONS (DAP_DoctorQualifications)
        ----------------------------------------------------------------------------------
        -- Deactivate removed ones  
        UPDATE dq  
        SET dq.IsActive = 0,  
            dq.ModifiedBy = @ModifiedBy,  
            dq.ModifiedOn = SYSDATETIME()  
        FROM DAP_DoctorQualifications dq  
        WHERE dq.DoctorId = @DoctorId  
          AND NOT EXISTS (  
              SELECT 1 FROM @DoctorQualificationList tvp  
              WHERE tvp.QualificationId = dq.QualificationId  
          )  
          AND dq.IsActive = 1;  
  
        -- Insert new ones  
        INSERT INTO DAP_DoctorQualifications 
            (DoctorId, QualificationId, IsActive, CreatedBy, CreatedOn)  
        SELECT @DoctorId, tvp.QualificationId, 1, @CreatedBy, SYSDATETIME()  
        FROM @DoctorQualificationList tvp  
        WHERE NOT EXISTS (  
            SELECT 1 FROM DAP_DoctorQualifications dq  
            WHERE dq.DoctorId = @DoctorId  
              AND dq.QualificationId = tvp.QualificationId  
        );  
  
        ----------------------------------------------------------------------------------
        COMMIT TRANSACTION;  
    END TRY  
    BEGIN CATCH  
        ROLLBACK TRANSACTION;  
        THROW;  
    END CATCH;  
END  

GO



CREATE OR ALTER PROCEDURE dap_doctorsGet
(
    @DoctorId INT = NULL
)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        D.DoctorId,
        D.UserId,

        ISNULL(U.Email, '') AS Email,
        ISNULL(R.ShortCode, '') AS RoleShortCode,

        ISNULL(D.FirstName, '') AS FirstName,
        ISNULL(D.LastName, '') AS LastName,
         DOB,
        ISNULL(D.ContactNo, '') AS ContactNo,
        ISNULL(D.YearOfExperience, 0) AS YearOfExperience,
        ISNULL(D.ConsultancyFee, 0) AS ConsultancyFee,
        ISNULL(D.IsActive, 0) AS IsActive,

        ISNULL(G.Gender, '') AS Gender,
        ISNULL(BG.BloodGroupName, '') AS BloodGroupName,

        ISNULL(A.AddressLine1, '') AS AddressLine1,
        ISNULL(A.TalukaId, 0) AS TalukaId,
        ISNULL(A.Pincode, '') AS Pincode,

        ISNULL(D.CreatedBy, 0) AS CreatedBy,
      
        D.CreatedOn,

        D.ModifiedOn,
        
        ISNULL(D.ModifiedBy,0) AS ModifiedBy

    FROM 
        DAP_Doctors D
        INNER JOIN DAP_Users U ON D.UserId = U.UserId
		INNER JOIN DAP_Roles R ON R.RoleId=U.RoleId
        INNER JOIN DAP_Genders G ON D.GenderId = G.GenderId
        INNER JOIN DAP_BloodGroups BG ON D.BloodGroupId = BG.BloodGroupId
        INNER JOIN DAP_Addresses A ON D.AddressId = A.AddressId
        INNER JOIN DAP_Users UC ON D.CreatedBy = UC.UserId
        LEFT JOIN DAP_Users UM ON D.ModifiedBy = UM.UserId

    WHERE D.IsActive=1 AND
        (@DoctorId IS NULL OR D.DoctorId = @DoctorId)

    ORDER BY 
        D.DoctorId DESC;

	 SELECT 
        DS.DoctorSpecializationId,
        DS.DoctorId,
        DS.SpecializationId,
        ISNULL(S.Specialization, '') AS Specialization,
        ISNULL(DS.IsActive, 0) AS IsActive,
        ISNULL(DS.CreatedBy, 0) AS CreatedBy,
        DS.CreatedOn,
        ISNULL(DS.ModifiedBy, 0) AS ModifiedBy,
        DS.ModifiedOn
    FROM 
        DAP_DoctorSpecializations DS
        INNER JOIN DAP_Specializations S ON DS.SpecializationId = S.SpecializationId
        INNER JOIN DAP_Users UCS ON DS.CreatedBy = UCS.UserId
        LEFT JOIN DAP_Users UMS ON DS.ModifiedBy = UMS.UserId
    WHERE 
        DS.IsActive = 1
        AND (@DoctorId IS NULL OR DS.DoctorId = @DoctorId)
    ORDER BY 
        DS.DoctorSpecializationId DESC;


    SELECT 
        DQ.DoctorQualificationId,
        DQ.DoctorId,
        DQ.QualificationId,
        ISNULL(Q.Degree, '') AS Degree,
        ISNULL(DQ.IsActive, 0) AS IsActive,
        ISNULL(DQ.CreatedBy, 0) AS CreatedBy,
        
        DQ.CreatedOn,
        ISNULL(DQ.ModifiedBy, 0) AS ModifiedBy,
        
        DQ.ModifiedOn
    FROM 
        DAP_DoctorQualifications DQ
        INNER JOIN DAP_Qualifications Q ON DQ.QualificationId = Q.QualificationId
        LEFT JOIN DAP_Users UQ ON DQ.CreatedBy = UQ.UserId
        LEFT JOIN DAP_Users UMQ ON DQ.ModifiedBy = UMQ.UserId
    WHERE 
        DQ.IsActive = 1
        AND (@DoctorId IS NULL OR DQ.DoctorId = @DoctorId)
    ORDER BY 
        DQ.DoctorQualificationId DESC;


   
    SELECT 
        S.SlotId,
        S.DoctorId,
        ISNULL(S.[DayOfWeek], '') AS [DayOfWeek],
        ISNULL(S.StartTime, '00:00') AS StartTime,
        ISNULL(S.EndTime, '00:00') AS EndTime,
        ISNULL(S.IsActive, 0) AS IsActive,
        ISNULL(S.CreatedBy, 0) AS CreatedBy,
         S.CreatedOn,
        ISNULL(S.ModifiedBy, 0) AS ModifiedBy,
        S.ModifiedOn
    FROM 
        DAP_DoctorAvailableSlots S
        INNER JOIN DAP_Users UCSlot ON S.CreatedBy = UCSlot.UserId
        LEFT JOIN DAP_Users UMSlot ON S.ModifiedBy = UMSlot.UserId
    WHERE 
        S.IsActive = 1
        AND (@DoctorId IS NULL OR S.DoctorId = @DoctorId)
    ORDER BY 
        S.SlotId DESC;
END;
GO

CREATE OR ALTER PROCEDURE dap_doctorsGet   
(
	@SearchDoctorName NVARCHAR(100) = NULL,
    @SpecializationIds NVARCHAR(MAX) = NULL,
    @PageNumber INT = 1,
    @PageSize INT 
)
AS
BEGIN
    SET NOCOUNT ON;

	IF @PageSize IS NULL OR @PageSize =0
	BEGIN 
	  SELECT @PageSize=COUNT(DoctorId) FROM DAP_Doctors 
	  WHERE IsActive=1
	END

    ----------------------------------------------------------
    -- Get Doctor Basic Info into Temp Table
    ----------------------------------------------------------
    SELECT 
		ROW_NUMBER() OVER (ORDER BY D.DoctorId) AS RowNum,
        D.DoctorId,
        D.UserId,
        ISNULL(U.Email, '') AS Email,
        ISNULL(R.ShortCode, '') AS RoleShortCode,
        ISNULL(D.FirstName, '') AS FirstName,
        ISNULL(D.LastName, '') AS LastName,
        D.DOB,
        ISNULL(D.ContactNo, '') AS ContactNo,
        ISNULL(D.YearOfExperience, 0) AS YearOfExperience,
        ISNULL(D.ConsultancyFee, 0) AS ConsultancyFee,
        ISNULL(D.IsActive, 0) AS IsActive,
        ISNULL(G.GenderId, 0) AS GenderId,
        ISNULL(G.Gender, '') AS Gender,
        ISNULL(BG.BloodGroupId, 0) AS BloodGroupId,
        ISNULL(BG.BloodGroupName, '') AS BloodGroupName,
        ISNULL(A.AddressLine1, '') AS AddressLine1,
        ISNULL(A.TalukaId, 0) AS TalukaId,
        ISNULL(A.Pincode, '') AS Pincode,
        ISNULL(D.CreatedBy, 0) AS CreatedBy,
        D.CreatedOn,
        ISNULL(D.ModifiedBy, 0) AS ModifiedBy,
        D.ModifiedOn
    INTO #Doctors
    FROM 
        DAP_Doctors D
        INNER JOIN DAP_Users U ON D.UserId = U.UserId
        INNER JOIN DAP_Roles R ON R.RoleId = U.RoleId
        INNER JOIN DAP_Genders G ON D.GenderId = G.GenderId
        INNER JOIN DAP_BloodGroups BG ON D.BloodGroupId = BG.BloodGroupId
        INNER JOIN DAP_Addresses A ON D.AddressId = A.AddressId
        INNER JOIN DAP_Talukas T ON A.TalukaId = T.TalukaId
        INNER JOIN DAP_Users UC ON D.CreatedBy = UC.UserId
        LEFT JOIN DAP_Users UM ON D.ModifiedBy = UM.UserId
    WHERE 
        D.IsActive = 1
		AND (
            @SearchDoctorName IS NULL 
            OR D.FirstName LIKE '%' + @SearchDoctorName + '%'
            OR D.LastName LIKE '%' + @SearchDoctorName + '%'
			)
		AND (
        @SpecializationIds IS NULL
        OR EXISTS (
            SELECT 1 
            FROM DAP_DoctorSpecializations DS
            WHERE DS.DoctorId = D.DoctorId
              AND DS.IsActive = 1
              AND DS.SpecializationId IN (SELECT TRY_CAST(value AS INT) FROM STRING_SPLIT(@SpecializationIds, ','))
        )
    );
		

    ----------------------------------------------------------
    -- Return Doctor Basic Info
    ----------------------------------------------------------
    SELECT 
        DoctorId,
        UserId,
        Email,
        RoleShortCode,
        FirstName,
        LastName,
        DOB,
        ContactNo,
        YearOfExperience,
        ConsultancyFee,
        IsActive,
        GenderId,
        Gender,
        BloodGroupId,
        BloodGroupName,
        AddressLine1,
        TalukaId,
        Pincode,
        CreatedBy,
        CreatedOn,
        ModifiedBy,
        ModifiedOn
    FROM 
        #Doctors
    WHERE RowNum BETWEEN ((@PageNumber - 1) * @PageSize + 1) AND (@PageNumber * @PageSize);

    ----------------------------------------------------------
    -- Doctor Specializations
    ----------------------------------------------------------
    SELECT 
        DS.DoctorSpecializationId,
        DS.DoctorId,
        DS.SpecializationId,
        ISNULL(S.Specialization, '') AS Specialization,
        ISNULL(DS.IsActive, 0) AS IsActive,
        ISNULL(DS.CreatedBy, 0) AS CreatedBy,
        DS.CreatedOn,
        ISNULL(DS.ModifiedBy, 0) AS ModifiedBy,
        DS.ModifiedOn
    FROM 
        DAP_DoctorSpecializations DS
        INNER JOIN DAP_Specializations S ON DS.SpecializationId = S.SpecializationId
        INNER JOIN #Doctors D ON DS.DoctorId = D.DoctorId
    WHERE 
        DS.IsActive = 1
    

    ----------------------------------------------------------
    --  Doctor Qualifications
    ----------------------------------------------------------
    SELECT 
        DQ.DoctorQualificationId,
        DQ.DoctorId,
        DQ.QualificationId,
        ISNULL(Q.Degree, '') AS Degree,
        ISNULL(DQ.IsActive, 0) AS IsActive,
        ISNULL(DQ.CreatedBy, 0) AS CreatedBy,
        DQ.CreatedOn,
        ISNULL(DQ.ModifiedBy, 0) AS ModifiedBy,
        DQ.ModifiedOn
    FROM 
        DAP_DoctorQualifications DQ
        INNER JOIN DAP_Qualifications Q ON DQ.QualificationId = Q.QualificationId
        INNER JOIN #Doctors D ON DQ.DoctorId = D.DoctorId
    WHERE 
        DQ.IsActive = 1
   

    ----------------------------------------------------------
    -- Doctor Slots
    ----------------------------------------------------------
    SELECT 
        S.SlotId,
        S.DoctorId,
        ISNULL(S.[DayOfWeek], '') AS [DayOfWeek],
        ISNULL(S.StartTime, '00:00') AS StartTime,
        ISNULL(S.EndTime, '00:00') AS EndTime,
        ISNULL(S.IsActive, 0) AS IsActive,
        ISNULL(S.CreatedBy, 0) AS CreatedBy,
        S.CreatedOn,
        ISNULL(S.ModifiedBy, 0) AS ModifiedBy,
        S.ModifiedOn
    FROM 
        DAP_DoctorAvailableSlots S
        INNER JOIN #Doctors D ON S.DoctorId = D.DoctorId
    WHERE 
        S.IsActive = 1
    

	SELECT COUNT(*) AS TotalRecords FROM #Doctors;

    DROP TABLE #Doctors;
END;
GO

CREATE OR ALTER PROCEDURE dap_doctorGetById 
(
    @DoctorId INT
)
AS
BEGIN
    SET NOCOUNT ON;

    ----------------------------------------------------------
    --  Get Doctor Basic Info into Temp Table
    ----------------------------------------------------------
    SELECT 
        D.DoctorId,
        D.UserId,
        ISNULL(U.Email, '') AS Email,
        ISNULL(R.ShortCode, '') AS RoleShortCode,
        ISNULL(D.FirstName, '') AS FirstName,
        ISNULL(D.LastName, '') AS LastName,
        D.DOB,
        ISNULL(D.ContactNo, '') AS ContactNo,
        ISNULL(D.YearOfExperience, 0) AS YearOfExperience,
        ISNULL(D.ConsultancyFee, 0) AS ConsultancyFee,
        ISNULL(D.IsActive, 0) AS IsActive,
        ISNULL(G.GenderId, 0) AS GenderId,
        ISNULL(G.Gender, '') AS Gender,
        ISNULL(BG.BloodGroupId, 0) AS BloodGroupId,
        ISNULL(BG.BloodGroupName, '') AS BloodGroupName,
        ISNULL(A.AddressLine1, '') AS AddressLine1,
        ISNULL(A.TalukaId, 0) AS TalukaId,
        ISNULL(T.TalukaName, '') AS TalukaName,
        ISNULL(T.DistrictId, 0) AS DistrictId,
        ISNULL(DT.DistrictName, '') AS DistrictName,
        ISNULL(DT.StateId, 0) AS StateId,
        ISNULL(S.StateName, '') AS StateName,
        ISNULL(S.CountryId, 0) AS CountryId,
        ISNULL(C.CountryName, '') AS CountryName,
        ISNULL(A.Pincode, '') AS Pincode,
        ISNULL(D.CreatedBy, 0) AS CreatedBy,
        D.CreatedOn,
        ISNULL(D.ModifiedBy, 0) AS ModifiedBy,
        D.ModifiedOn
    INTO #Doctor
    FROM 
        DAP_Doctors D
        INNER JOIN DAP_Users U ON D.UserId = U.UserId
        INNER JOIN DAP_Roles R ON R.RoleId = U.RoleId
        INNER JOIN DAP_Genders G ON D.GenderId = G.GenderId
        INNER JOIN DAP_BloodGroups BG ON D.BloodGroupId = BG.BloodGroupId
        INNER JOIN DAP_Addresses A ON D.AddressId = A.AddressId
        INNER JOIN DAP_Talukas T ON A.TalukaId = T.TalukaId
        INNER JOIN DAP_Districts DT ON T.DistrictId = DT.DistrictId
        INNER JOIN DAP_States S ON DT.StateId = S.StateId
        INNER JOIN DAP_Countries C ON S.CountryId = C.CountryId
    WHERE 
        D.DoctorId = @DoctorId 
        AND D.IsActive = 1;

    ----------------------------------------------------------
    --  Doctor Basic Info
    ----------------------------------------------------------
    SELECT 
        DoctorId,
        UserId,
        Email,
        RoleShortCode,
        FirstName,
        LastName,
        DOB,
        ContactNo,
        YearOfExperience,
        ConsultancyFee,
        IsActive,
        GenderId,
        Gender,
        BloodGroupId,
        BloodGroupName,
        AddressLine1,
        TalukaId,
        TalukaName,
        DistrictId,
        DistrictName,
        StateId,
        StateName,
        CountryId,
        CountryName,
        Pincode,
        CreatedBy,
        CreatedOn,
        ModifiedBy,
        ModifiedOn
    FROM #Doctor;

    ----------------------------------------------------------
    -- Doctor Specializations
    ----------------------------------------------------------
    SELECT 
        DS.DoctorSpecializationId,
        DS.DoctorId,
        DS.SpecializationId,
        ISNULL(S.Specialization, '') AS Specialization,
        ISNULL(DS.IsActive, 0) AS IsActive,
        ISNULL(DS.CreatedBy, 0) AS CreatedBy,
        DS.CreatedOn,
        ISNULL(DS.ModifiedBy, 0) AS ModifiedBy,
        DS.ModifiedOn
    FROM 
        DAP_DoctorSpecializations DS
        INNER JOIN DAP_Specializations S ON DS.SpecializationId = S.SpecializationId
    WHERE 
        DS.DoctorId = @DoctorId 
        AND DS.IsActive = 1;

    ----------------------------------------------------------
    --  Doctor Qualifications
    ----------------------------------------------------------
    SELECT 
        DQ.DoctorQualificationId,
        DQ.DoctorId,
        DQ.QualificationId,
        ISNULL(Q.Degree, '') AS Degree,
        ISNULL(DQ.IsActive, 0) AS IsActive,
        ISNULL(DQ.CreatedBy, 0) AS CreatedBy,
        DQ.CreatedOn,
        ISNULL(DQ.ModifiedBy, 0) AS ModifiedBy,
        DQ.ModifiedOn
    FROM 
        DAP_DoctorQualifications DQ
        INNER JOIN DAP_Qualifications Q ON DQ.QualificationId = Q.QualificationId
    WHERE 
        DQ.DoctorId = @DoctorId 
        AND DQ.IsActive = 1;

    ----------------------------------------------------------
    -- Doctor Available Slots
    ----------------------------------------------------------
    SELECT 
        S.SlotId,
        S.DoctorId,
        ISNULL(S.[DayOfWeek], '') AS [DayOfWeek],
        ISNULL(S.StartTime, '00:00') AS StartTime,
        ISNULL(S.EndTime, '00:00') AS EndTime,
        ISNULL(S.IsActive, 0) AS IsActive,
        ISNULL(S.CreatedBy, 0) AS CreatedBy,
        S.CreatedOn,
        ISNULL(S.ModifiedBy, 0) AS ModifiedBy,
        S.ModifiedOn
    FROM 
        DAP_DoctorAvailableSlots S
    WHERE 
        S.DoctorId = @DoctorId 
        AND S.IsActive = 1;

   
    DROP TABLE #Doctor;
END;
GO



CREATE OR ALTER PROCEDURE dap_doctorDeleteById
(
    @DoctorId INT,
    @DeletedBy INT
)
AS
BEGIN
  

    DECLARE @UserId INT, @AddressId INT;

    BEGIN TRY
        BEGIN TRANSACTION;

       
        SELECT 
            @UserId = UserId,
            @AddressId = AddressId
        FROM DAP_Doctors
        WHERE DoctorId = @DoctorId;

       
        UPDATE DAP_Doctors
        SET IsActive = 0,
            ModifiedBy = @DeletedBy,
            ModifiedOn = SYSDATETIME()
        WHERE DoctorId = @DoctorId;

      
        UPDATE DAP_Users
        SET IsActive = 0,
            ModifiedBy = @DeletedBy,
            ModifiedOn = SYSDATETIME()
        WHERE UserId = @UserId;

      
        UPDATE DAP_Addresses
        SET IsActive = 0,
            ModifiedBy = @DeletedBy,
            ModifiedOn = SYSDATETIME()
        WHERE AddressId = @AddressId;

        ----------------------------------------------------------------------
        -- 4️⃣ Soft Delete Doctor’s Available Slots
        ----------------------------------------------------------------------
        UPDATE DAP_DoctorAvailableSlots
        SET IsActive = 0,
            ModifiedBy = @DeletedBy,
            ModifiedOn = SYSDATETIME()
        WHERE DoctorId = @DoctorId;

        UPDATE DAP_DoctorSpecializations
        SET IsActive = 0,
            ModifiedBy = @DeletedBy,
            ModifiedOn = SYSDATETIME()
        WHERE DoctorId = @DoctorId;

       
        UPDATE DAP_DoctorQualifications
        SET IsActive = 0,
            ModifiedBy = @DeletedBy,
            ModifiedOn = SYSDATETIME()
        WHERE DoctorId = @DoctorId;

       
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH;
END;

GO

CREATE OR ALTER PROCEDURE dap_doctorsBySpecialization
  @SpecializationId INT
AS
BEGIN
  SELECT D.DoctorId, 
		 D.FirstName + ' ' + D.LastName AS FullName

  FROM DAP_Doctors D
  INNER JOIN DAP_DoctorSpecializations DS
    ON D.DoctorId = DS.DoctorId
  WHERE D.IsActive = 1 AND DS.SpecializationId = @SpecializationId AND DS.IsActive = 1;
END
GO;

CREATE OR ALTER PROCEDURE dap_appointmentAcceptedSlotsGetByDate
(
    @DoctorId INT,
    @RequestedDate DATE
)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        PS.PreferredSlotId,
		A.DoctorId,
		A.PatientId,
        PS.AppointmentId,
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

CREATE OR ALTER PROCEDURE dap_doctorAvailableSlotsGetByDate
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

