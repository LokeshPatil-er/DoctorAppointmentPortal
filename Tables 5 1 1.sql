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

INSERT INTO DAP_Genders (Gender, IsActive, CreatedBy, CreatedOn)
VALUES
('Male', 1, NULL, GETDATE()),
('Female', 1, NULL, GETDATE()),
('Transgender', 1, NULL, GETDATE()),
('Non-Binary', 1, NULL, GETDATE()),
('Prefer Not to Say', 1, NULL, GETDATE()),
('Other', 1, NULL, GETDATE());


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

INSERT INTO DAP_Countries (CountryName, CreatedBy, CreatedOn)
VALUES
('India', 1, SYSDATETIME()),
('United States', 1, SYSDATETIME()),
('Canada', 1, SYSDATETIME()),
('United Kingdom', 1, SYSDATETIME()),
('Australia', 1, SYSDATETIME());


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


INSERT INTO DAP_States (StateName, CountryId, CreatedBy, CreatedOn)
VALUES
('Maharashtra', 1, 1, SYSDATETIME()),
('Gujarat', 1, 1, SYSDATETIME()),
('Karnataka', 1, 1, SYSDATETIME()),
('Madhya Pradesh', 1, 1, SYSDATETIME()),
('Rajasthan', 1, 1, SYSDATETIME());

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

INSERT INTO DAP_Districts (DistrictName, StateId, CreatedBy, CreatedOn)
VALUES
('Pune', 1, 1, SYSDATETIME()),
('Mumbai', 1, 1, SYSDATETIME()),
('Nagpur', 1, 1, SYSDATETIME()),
('Nashik', 1, 1, SYSDATETIME()),
('Aurangabad', 1, 1, SYSDATETIME());


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

INSERT INTO DAP_Talukas (TalukaName, DistrictId, CreatedBy, CreatedOn)
VALUES
('Haveli', 1, 1, SYSDATETIME()),
('Mulshi', 1, 1, SYSDATETIME()),
('Bhor', 1, 1, SYSDATETIME()),
('Baramati', 1, 1, SYSDATETIME()),
('Maval', 1, 1, SYSDATETIME());

INSERT INTO DAP_Addresses 
(
    AddressLine1,
    Pincode,
    TalukaId,
    CreatedBy,
    CreatedOn
)
VALUES
('123 Shivaji Nagar', '411005', 1, 1, SYSDATETIME()),
('56 MG Road', '411001', 1, 1, SYSDATETIME()),
('Plot No 45, Baramati Industrial Area', '413102', 4, 1, SYSDATETIME()),
('Near Central Park, Mulshi', '412108', 2, 1, SYSDATETIME()),
('Main Street, Haveli', '411041', 3, 1, SYSDATETIME());
--8
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
INSERT INTO DAP_Specializations
(
    Specialization,
    [Description],
    IsActive,
    CreatedBy,
    CreatedOn
)
VALUES
('Cardiology', 'Specialist in heart and blood vessel disorders', 1, 1, SYSDATETIME()),
('Dermatology', 'Specialist in skin, hair, and nail conditions', 1, 1, SYSDATETIME()),
('Neurology', 'Specialist in brain, spinal cord, and nerve disorders', 1, 1, SYSDATETIME()),
('Pediatrics', 'Specialist in medical care for children', 1, 1, SYSDATETIME()),
('Orthopedics', 'Specialist in bones, joints, ligaments, tendons, and muscles', 1, 1, SYSDATETIME()),
('Gynecology', 'Specialist in female reproductive health', 1, 1, SYSDATETIME()),
('Ophthalmology', 'Specialist in eye and vision care', 1, 1, SYSDATETIME()),
('ENT', 'Specialist in ear, nose, and throat disorders', 1, 1, SYSDATETIME());


--9
CREATE  TABLE DAP_BloodGroups (
    BloodGroupId INT IDENTITY PRIMARY KEY,
    BloodGroupName NVARCHAR(100) NOT NULL UNIQUE,
    IsActive BIT DEFAULT 1,
	CreatedOn DATETIME2 NOT NULL,
);

ALTER TABLE DAP_BloodGroups
ALTER COLUMN BloodGroupName NVARCHAR(10) NOT NULL;


INSERT INTO DAP_BloodGroups (BloodGroupName, CreatedOn)
VALUES
('A+','2025-10-14 14:40:00'),
('A-','2025-10-14 14:40:00'),
('B+','2025-10-14 14:40:00'),
('B-','2025-10-14 14:40:00'),
('AB+','2025-10-14 14:40:00'),
('AB-','2025-10-14 14:40:00'),
('O+','2025-10-14 14:40:00'),
('O-','2025-10-14 14:40:00');

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
INSERT INTO DAP_Qualifications
(
    Degree,
    DegreeDescription,
    IsActive,
    CreatedBy,
    CreatedOn
)
VALUES
('MBBS', 'Bachelor of Medicine, Bachelor of Surgery', 1, 1, SYSDATETIME()),
('MD', 'Doctor of Medicine', 1, 1, SYSDATETIME()),
('MS', 'Master of Surgery', 1, 1, SYSDATETIME()),
('BDS', 'Bachelor of Dental Surgery', 1, 1, SYSDATETIME()),
('MDS', 'Master of Dental Surgery', 1, 1, SYSDATETIME()),
('DM', 'Doctorate in Medicine', 1, 1, SYSDATETIME()),
('MCh', 'Master of Chirurgical', 1, 1, SYSDATETIME()),
('PhD', 'Doctor of Philosophy', 1, 1, SYSDATETIME());

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
	SlotId INT PRIMARY KEY IDENTITY(0,1),
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
	PatientId INT PRIMARY KEY IDENTITY(0,1),
	FirstName NVARCHAR(50) NOT NULL UNIQUE,
	LastName NVARCHAR(50) NOT NULL UNIQUE,
	GenderId INT FOREIGN KEY REFERENCES DAP_Genders(GenderId) NOT NULL,
	DOB DATE NOT NULL,
	ContactNo NVARCHAR(10) NOT NULL ,
	Email NVARCHAR(320) NOT NULL,
	CreateOn DATETIME NOT NULL,
);

--15
CREATE TABLE DAP_AppointmentStatus
(
	StatusId INT PRIMARY KEY IDENTITY(0,1),
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
    PolicyNumber NVARCHAR(50) NOT NULL UNIQUE,
    ValidTill DATE NULL,
    CoverageType NVARCHAR(50) NULL,
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
	AppointmentReason NVARCHAR(MAX) NOT NULL,
	AppointmentStatusId INT FOREIGN KEY REFERENCES DAP_AppointmentStatus(StatusId) NOT NULL,
	PatientInsuranceId INT FOREIGN KEY REFERENCES DAP_PatientInsurancesInfo(InsuranceId) NULL,
	MedicalHistroy NVARCHAR(MAX),
	ConfirmedDate DATE NULL,
	ConfirmedStartTime TIME NULL,
	ConfirmedEndTime TIME NULL,
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
	AppointmentId INT FOREIGN KEY REFERENCES DAP_Appointments(AppointmentId) NOT NULL,
	PreferredDate DATE NOT NULL,
	PreferredStartTime TIME NOT NULL,
	PreferredEndTime TIME NOT NULL,
	IsActive BIT DEFAULT 1,
	CreatedBy INT FOREIGN KEY REFERENCES DAP_Users(UserId) NULL,
	CreatedOn DATETIME2 NULL,
	ModifiedBy INT FOREIGN KEY REFERENCES DAP_Users(UserId) NULL,
	ModifiedOn DATETIME2 NULL,
	
);


--19
CREATE TABLE DAP_PatientExsistingReports
(
	ReportId INT PRIMARY KEY IDENTITY(0,1),
	AppointmentId INT FOREIGN KEY REFERENCES DAP_Appointments(AppointmentId),
	ReportName NVARCHAR(50),
	ReportFullName NVARCHAR(50),
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

--detete tvp table of DoctorQualifications and create new with below


CREATE TYPE DAP_DoctorQualificationsTVP AS TABLE
(
    QualificationId INT
);
GO


CREATE TYPE DAP_DoctorAvailableSlotsTVP AS TABLE
(
    [DayOfWeek] NVARCHAR(20),
    StartTime TIME,
    EndTime TIME,
	IsAvailable  BIT
	
);
GO


CREATE TYPE DAP_DoctorSpecializationsTVP AS TABLE
(
    SpecializationId INT
	
);
GO

--ADD check for user email is already exists or not with is active before add in data into user table
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
        -- 1️ USER TABLE (DAP_Users)
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
        -- 2️⃣ ADDRESS TABLE (DAP_Addresses)
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
        -- 3 DOCTOR TABLE (DAP_Doctors)
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
        -- 4 DOCTOR AVAILABLE SLOTS (DAP_DoctorAvailableSlots)
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
        -- 5️ DOCTOR SPECIALIZATIONS (DAP_DoctorSpecializations)
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
        -- 6️ DOCTOR QUALIFICATIONS (DAP_DoctorQualifications)
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

