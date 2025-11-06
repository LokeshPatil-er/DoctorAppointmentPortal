

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
GO

CREATE TABLE DAP_Roles
(
	RoleId INT PRIMARY KEY IDENTITY(1,1),
	[Role] NVARCHAR(50) NOT NULL UNIQUE,
	ShortCode NVARCHAR(10) NOT NULL,
	IsActive BIT DEFAULT 1,
	CreatedOn DATETIME2 NULL,
	ModifiedOn DATETIME2 NULL,
);
GO

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

GO
select * from DAP_Users
CREATE  TABLE DAP_Countries (
    CountryId INT IDENTITY(1,1) PRIMARY KEY,
    CountryName NVARCHAR(100) NOT NULL UNIQUE,
    IsActive BIT DEFAULT 1,
	CreatedBy INT FOREIGN KEY REFERENCES DAP_Users(UserId) NULL,
	CreatedOn DATETIME2 NOT NULL,
	ModifiedBy INT FOREIGN KEY REFERENCES DAP_Users(UserId) NULL,
	ModifiedOn DATETIME2 NULL,
);

GO

CREATE  TABLE  DAP_States (
    StateId INT IDENTITY(1,1) PRIMARY KEY,
    StateName NVARCHAR(100) NOT NULL UNIQUE,
    CountryId INT NOT NULL FOREIGN KEY REFERENCES DAP_Countries(CountryId),
    IsActive BIT DEFAULT 1,
	CreatedBy INT FOREIGN KEY REFERENCES DAP_Users(UserId) NULL,
	CreatedOn DATETIME2 NOT NULL,
	ModifiedBy INT FOREIGN KEY REFERENCES DAP_Users(UserId) NULL,
	ModifiedOn DATETIME2 NULL,
);

GO

CREATE  TABLE DAP_Districts (
    DistrictId INT IDENTITY(1,1) PRIMARY KEY,
    DistrictName NVARCHAR(100) NOT NULL UNIQUE,
    StateId INT NOT NULL FOREIGN KEY REFERENCES DAP_States(StateId),
    IsActive BIT DEFAULT 1,
	CreatedBy INT FOREIGN KEY REFERENCES DAP_Users(UserId) NULL,
	CreatedOn DATETIME2 NOT NULL,
	ModifiedBy INT FOREIGN KEY REFERENCES DAP_Users(UserId) NULL,
	ModifiedOn DATETIME2 NULL,
);

GO

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

GO

CREATE  TABLE DAP_Addresses (
    AddressId INT IDENTITY PRIMARY KEY,
    AddressLine1 NVARCHAR(200) NOT NULL,
    Pincode NVARCHAR(6) NOT NULL,
    TalukaId INT NOT NULL FOREIGN KEY REFERENCES DAP_Talukas(TalukaId),
	IsActive BIT DEFAULT 1,
	CreatedBy INT FOREIGN KEY REFERENCES DAP_Users(UserId) NULL,
	CreatedOn DATETIME2 NOT NULL,
	ModifiedBy INT FOREIGN KEY REFERENCES DAP_Users(UserId) NULL,
	ModifiedOn DATETIME2 NULL,
);

GO

CREATE  TABLE DAP_Specializations (
    SpecializationId INT IDENTITY PRIMARY KEY,
    Specialization NVARCHAR(150) NOT NULL UNIQUE, 
	[Description] NVARCHAR(255) NOT NULL,
	IsActive BIT,
	CreatedBy INT FOREIGN KEY REFERENCES DAP_Users(UserId),
	CreatedOn DATETIME2,
	ModifiedBy INT FOREIGN KEY REFERENCES DAP_Users(UserId),
	ModifiedOn DATETIME2,
);

GO

CREATE  TABLE DAP_BloodGroups (
    BloodGroupId INT IDENTITY PRIMARY KEY,
    BloodGroupName NVARCHAR(100) NOT NULL UNIQUE,
    IsActive BIT DEFAULT 1,
	CreatedOn DATETIME2 NOT NULL,
);

GO

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

GO

CREATE  TABLE DAP_DoctorQualifications (
    DoctorQualificationId INT PRIMARY KEY IDENTITY(1,1),
    DoctorId INT FOREIGN KEY REFERENCES DAP_Doctors(DoctorId) NOT NULL,
    QualificationId INT FOREIGN KEY REFERENCES DAP_Qualifications(QualificationId) NOT NULL,
	[Description] NVARCHAR(255) NULL,
	IsActive BIT DEFAULT 1,
	CreatedBy INT FOREIGN KEY REFERENCES DAP_Users(UserId) NULL,
	CreatedOn DATETIME2 NULL,
	ModifiedBy INT FOREIGN KEY REFERENCES DAP_Users(UserId) NULL,
	ModifiedOn DATETIME2 NULL,
);

GO

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

GO

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

GO

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

GO

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

GO

CREATE TABLE DAP_Appointments
(
	AppointmentId INT PRIMARY KEY IDENTITY(0,1),
	PatientId INT FOREIGN KEY REFERENCES DAP_Patients(PatientId) NOT NULL,
	DoctorId INT FOREIGN KEY REFERENCES DAP_Doctors(DoctorId) NOT NULL,
	ReasonForVisit NVARCHAR(255) NOT NULL,
	AppointmentStatusId INT FOREIGN KEY REFERENCES DAP_AppointmentStatus(AppointmentStatusId) NOT NULL,
	PatientInsuranceId INT FOREIGN KEY REFERENCES DAP_PatientInsurancesInfo(InsuranceId) NULL,
	MedicalHistory NVARCHAR(255),
	IsActive BIT DEFAULT 1,
	CreatedBy INT FOREIGN KEY REFERENCES DAP_Users(UserId) NULL,
	CreatedOn DATETIME2 NULL,
	ModifiedBy INT FOREIGN KEY REFERENCES DAP_Users(UserId) NULL,
	ModifiedOn DATETIME2 NULL,

);

GO

CREATE TABLE DAP_PreferredSlots
(
	PreferredSlotId INT PRIMARY KEY IDENTITY(0,1),
	AppointmentId INT FOREIGN KEY REFERENCES DAP_Appointments(AppointmentId),
	PreferredDate DATE NOT NULL,
	PreferredStartTime TIME NOT NULL,
	PreferredEndTime TIME NOT NULL,
	IsApproved BIT DEFAULT 0,
	IsActive BIT DEFAULT 1,
	IsAlternate BIT DEFAULT 0,
	CreatedBy INT FOREIGN KEY REFERENCES DAP_Users(UserId) NULL,
	CreatedOn DATETIME2 NULL,
	ModifiedBy INT FOREIGN KEY REFERENCES DAP_Users(UserId) NULL,
	ModifiedOn DATETIME2 NULL,
	
);

GO

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

GO

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
GO

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

GO

CREATE TABLE DAP_AppointmentAlternateSlotActionToken
(
	ActionTokenId INT PRIMARY KEY IDENTITY(1,1),
	Token NVARCHAR(100) NOT NULL,
	AppointmentId INT FOREIGN KEY REFERENCES DAP_Appointments(AppointmentId),
	PreferredSlotId INT FOREIGN KEY REFERENCES DAP_PreferredSlots(PreferredSlotId),
	ExpiryDate DATETIME NOT NULL,
	IsUsed BIT ,
	CreatedOn DATETIME NOT NULL
);

GO

CREATE TABLE DAP_ExceptionLog
(
	ExceptionLogId INT IDENTITY(1,1) PRIMARY KEY,
    ExceptionMessage NVARCHAR(MAX) NOT NULL,
    ExceptionStackTrace NVARCHAR(MAX) NULL,
    InnerException NVARCHAR(MAX) NULL,
    Source NVARCHAR(MAX) NULL,
    TargetSite NVARCHAR(MAX) NULL,
    UserId INT NULL FOREIGN KEY REFERENCES DAP_Users(UserId),
    LoggedOn DATETIME NOT NULL,
    AdditionalInfo NVARCHAR(MAX) NULL,
    IsResolved BIT NULL
);

GO

--SP's 
CREATE OR ALTER   PROCEDURE dap_usersLoadByEmail     
/*      
******************************************************************************************************      
Date            Created By         Purpose Of Creation      
10Oct2025       Lokesh Patil       To Get User By Email       
******************************************************************************************************      
*/      
@email NVARCHAR(320)    
AS    
BEGIN    
    
  SELECT     
   u.UserId,    
   d.DoctorId,  
   u.Email,    
   u.[Password],    
   u.RoleId,    
   r.[Role],    
   r.ShortCode,    
   u.IsFirstLogin,    
   u.IsActive AS IsActive,    
   ISNULL (u.CreatedBy,0) AS CreatedBy,    
   u.CreatedOn,    
   ISNULL (u.ModifiedBy,0) AS ModifiedBy,    
   ISNULL (u.ModifiedOn,'') AS ModifiedOn    
  FROM DAP_Users as u    
   LEFT JOIN DAP_Doctors d ON d.UserId=u.UserId  
   LEFT JOIN DAP_Roles r ON u.RoleId=r.RoleId    
  WHERE u.IsActive=1  AND  u.Email=@email  AND r.IsActive=1  
END;
GO

CREATE OR ALTER   PROCEDURE dap_usersCheckEmailExists    
/*    
******************************************************************************************************    
Date            Created By         Purpose Of Creation    
10 OCT 2025       Lokesh Patil       To check if a user email exists in the system    
******************************************************************************************************    
*/    
@email NVARCHAR(320),   
@userId INT =NULL,  
@IsEmailExists BIT OUTPUT    
AS    
BEGIN    
    SET NOCOUNT ON;    
    
    IF EXISTS (    
        SELECT 1     
        FROM DAP_Users     
        WHERE Email = @email AND IsActive = 1  AND (@UserId IS NULL OR UserId <> @UserId)  
    )    
        SET @IsEmailExists = 1;    
    ELSE    
        SET @IsEmailExists = 0;    
END;
GO

CREATE OR ALTER PROCEDURE dap_doctorsListAllOrBySpecialization
/*  
-----------------------------------------------------------------------------------------------------------------------
Date			Created By		Purpose of creation
10 OCT 2025		Lokesh Patil	To get doctor list all or based on specialization
-----------------------------------------------------------------------------------------------------------------------
*/
@SpecializationId INT = NULL
AS
BEGIN
    SELECT
        D.DoctorId,
        D.FirstName + ' ' + D.LastName AS FullName,
        D.ConsultancyFee,
        ISNULL(QQ.Qualifications, 'N/A') AS Qualifications
    FROM DAP_Doctors D
    INNER JOIN DAP_DoctorSpecializations DS
        ON D.DoctorId = DS.DoctorId
    INNER JOIN DAP_Users U
        ON D.UserId = U.UserId
    INNER JOIN DAP_Roles R
        ON U.RoleId = R.RoleId
    LEFT JOIN (
        SELECT 
            DQ.DoctorId,
            STRING_AGG(Q.Degree, ', ') AS Qualifications
        FROM DAP_DoctorQualifications DQ
        INNER JOIN DAP_Qualifications Q
            ON DQ.QualificationId = Q.QualificationId
        WHERE DQ.IsActive = 1
        GROUP BY DQ.DoctorId
    ) AS QQ
        ON D.DoctorId = QQ.DoctorId
    WHERE
        D.IsActive = 1
        AND DS.IsActive = 1
        AND R.IsActive = 1
        AND (@SpecializationId IS NULL OR DS.SpecializationId = @SpecializationId)
    GROUP BY
        D.DoctorId,
        D.FirstName,
        D.LastName,
        D.ConsultancyFee,
        D.ExperienceStartDate,
        QQ.Qualifications
    ORDER BY
        D.ExperienceStartDate ASC;
END;
GO

CREATE OR ALTER   PROCEDURE dap_doctorsGetAll           
/*  
-----------------------------------------------------------------------------------------------------------------  
Date			Created By		Purpose of creation  
13 OCT 2025		Lokesh Patil	To get all doctor details or based on filter Doctor Name, Specializations  
								with pagination  
------------------------------------------------------------------------------------------------------------------  
*/  
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
		D.ExperienceStartDate,        
        ISNULL(D.ConsultancyFee, 0) AS ConsultancyFee,        
        ISNULL(D.IsActive, 0) AS IsActive,        
        ISNULL(G.GenderId, 0) AS GenderId,        
        ISNULL(G.Gender, '') AS Gender,        
        ISNULL(BG.BloodGroupId, 0) AS BloodGroupId,        
        ISNULL(BG.BloodGroupName, '') AS BloodGroupName,       
		 D.AddressId AS AddressId,    
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
        ExperienceStartDate,        
        ConsultancyFee,        
        IsActive,        
        GenderId,        
        Gender,        
        BloodGroupId,        
        BloodGroupName,       
  AddressId,    
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

CREATE OR ALTER   PROCEDURE dap_doctorInsertOrUpdate        
/*       
----------------------------------------------------------------------------------       
Create Date  Created By		Purpose Of Creation       
10OCT2025	 Lokesh Patil	To insert or update data related to doctor        
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
        
@ExperienceStartDate DATE,        
@ConsultancyFee INT,        
        
@CreatedBy INT = NULL,        
@ModifiedBy INT = NULL,        
        
@AvailableSlotList DAP_DoctorAvailableSlotsTVP READONLY,        
@DoctorSpecializationList DAP_DoctorSpecializationsTVP READONLY,        
@DoctorQualificationList DAP_DoctorQualificationsTVP READONLY          
AS        
BEGIN        
    SET NOCOUNT ON;        
        
    DECLARE @RoleId INT,@ExistingAddressId INT;        
        
    BEGIN TRY        
        BEGIN TRANSACTION;        
        
        ----------------------------------------------------------------------------------      
        -- USER TABLE (DAP_Users)      
        ----------------------------------------------------------------------------------      
             
        
        IF @UserId IS NULL  OR @UserId=0      
        BEGIN        
			
			 SELECT @RoleId = RoleId       
			 FROM DAP_Roles       
			 WHERE ShortCode = @RoleShortCode;  

            INSERT INTO DAP_Users ([Password], Email, RoleId, CreatedBy, CreatedOn)        
            VALUES (@Password, @Email, @RoleId, @CreatedBy, SYSDATETIME());        
      
            SET @UserId = SCOPE_IDENTITY();        
        END        
        ELSE        
        BEGIN        
            UPDATE DAP_Users        
            SET               
                Email = @Email,        
                ModifiedBy = @ModifiedBy,        
                ModifiedOn = GETDATE()        
            WHERE UserId = @UserId;        
        END        
        
        ----------------------------------------------------------------------------------      
        -- ADDRESS TABLE (DAP_Addresses)      
        ----------------------------------------------------------------------------------   
     
  
      
	   SELECT TOP 1 @ExistingAddressId = AddressId  
	   FROM DAP_Addresses  
	   WHERE   
		LOWER(AddressLine1) = LOWER(@AddressLine1)  
		AND TalukaId = @TalukaId  
		AND Pincode = @Pincode  
		AND IsActive = 1;   
  
	   IF @ExistingAddressId>0 AND @ExistingAddressId  IS NOT NULL  
		BEGIN  
		 SET @AddressId=@ExistingAddressId  
		END  
	   ELSE  
		BEGIN  
  
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
		END  
            
        
        ----------------------------------------------------------------------------------      
        --  DOCTOR TABLE (DAP_Doctors)      
        ----------------------------------------------------------------------------------      
        IF @DoctorId IS NULL  OR @DoctorId = 0      
        BEGIN        
            INSERT INTO DAP_Doctors   
          (UserId, FirstName, LastName, DOB, ContactNo, BloodGroupId, GenderId,       
             AddressId, ExperienceStartDate, ConsultancyFee, CreatedBy, CreatedOn)        
            VALUES        
            (@UserId, @FirstName, @LastName, @DOB, @ContactNo, @BloodGroupId, @GenderId,       
             @AddressId, @ExperienceStartDate, @ConsultancyFee, @CreatedBy, GETDATE());        
      
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
                ExperienceStartDate = @ExperienceStartDate,        
                ConsultancyFee = @ConsultancyFee,        
                ModifiedBy = @ModifiedBy,        
                ModifiedOn = GETDATE()        
            WHERE DoctorId = @DoctorId;        
        END        
        
        ----------------------------------------------------------------------------------      
        -- DOCTOR AVAILABLE SLOTS (DAP_DoctorAvailableSlots)      
        ----------------------------------------------------------------------------------      
        -- Deactivate slots not in the current list        
        UPDATE s        
        SET s.IsActive = 0,        
            s.ModifiedBy = @ModifiedBy,        
            s.ModifiedOn = SYSDATETIME()        
        FROM DAP_DoctorAvailableSlots s        
        WHERE s.DoctorId = @DoctorId        
			  AND NOT EXISTS (    
		 SELECT 1 FROM @AvailableSlotList tvp    
		 WHERE tvp.SlotId = s.SlotId    
		)    
          AND s.IsActive = 1;        
        
	  UPDATE s    
	   SET s.DayOfWeek = tvp.DayOfWeek,    
		s.StartTime = tvp.StartTime,    
		s.EndTime = tvp.EndTime,    
		s.IsActive = tvp.IsAvailable,    
		s.ModifiedBy = @ModifiedBy,    
		s.ModifiedOn = SYSDATETIME()    
	   FROM DAP_DoctorAvailableSlots s    
	   JOIN @AvailableSlotList tvp ON s.SlotId = tvp.SlotId    
	   WHERE s.DoctorId = @DoctorId;    
    
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
		SELECT     
		@DoctorId,    
		tvp.DayOfWeek,    
		tvp.StartTime,    
		tvp.EndTime,    
		tvp.IsAvailable,    
		@CreatedBy,    
		SYSDATETIME()    
		FROM @AvailableSlotList tvp    
		WHERE tvp.SlotId IS NULL OR tvp.SlotId=0;     
        
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

CREATE OR ALTER   PROCEDURE dap_doctorGetAvailabilityAndAcceptedSlots  
/*  
--------------------------------------------------------------------------------------------------------------------  
 Date			Created By		Purpose of creation  
 15 OCT 2025	Lokesh Patil	To get doctor Availability and accepted appointment slot related to them   
								based on Doctor id and requested date  
---------------------------------------------------------------------------------------------------------------------  
  
*/  
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
GO

CREATE OR ALTER   PROCEDURE dap_doctorDeleteById  
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

CREATE OR ALTER   PROCEDURE dap_appointmentInsert  
/*  
------------------------------------------------------------------------------------------  
Date  Create By  Purpose Of Creation  
14OCT2025 Lokesh Patil To insert  patient and appointment related info  
------------------------------------------------------------------------------------------  
*/  
  
 @DoctorId INT,  
   
 @FirstName NVARCHAR(50),  
 @LastName NVARCHAR(50),  
 @DOB DATE,  
 @GenderId INT,  
 @BloodGroupId INT,  
 @Email NVARCHAR(320),  
 @ContactNo NVARCHAR(10),  
 @AddressLine1 NVARCHAR(200),  
 @TalukaId INT,  
 @PinCode NVARCHAR(10),  
  
 @MedicalHistory NVARCHAR(MAX),  
 @ReasonOfAppointment NVARCHAR(MAX),  
  
 @ProviderName NVARCHAR(100),  
 @PolicyNumber NVARCHAR(50),  
 @PolicyName NVARCHAR(255),  
 @ValidTill DATE,  
 @OutPatientId INT OUTPUT,  
 @OutAppointmentId INT OUTPUT,  
  
 @preferredSlotsTVP DAP_PreferredSlotsTVP READONLY,  
 @proviousReportsTVP DAP_PatientReportsTVP READONLY  
  
AS  
BEGIN  
 SET NOCOUNT ON;  
  
 DECLARE @AddressId INT, @PatientId INT,@AppointmentStatusId INT, @PatientInsuranceId INT, @AppointmentId INT;  
 BEGIN TRY  
 BEGIN TRANSACTION  
		SELECT @AddressId = AddressId  
		FROM DAP_Addresses  
		WHERE   
		   LOWER(AddressLine1) = LOWER(@AddressLine1)  
		   AND Pincode = @Pincode  
		   AND TalukaId = @TalukaId  
		   AND IsActive = 1;
	   
	  IF @AddressId IS NULL 
		BEGIN  
		   INSERT INTO DAP_Addresses  
			(  
			 AddressLine1,  
			 Pincode,  
			 TalukaId,  
			 CreatedOn  
			)  
		   VALUES  
			(  
			 @AddressLine1,  
			 @PinCode,  
			 @TalukaId,  
			 GETDATE()  
			);  
  
	   SET @AddressId=SCOPE_IDENTITY();  
	   END  
  
    SELECT @PatientId = PatientId   
          FROM DAP_Patients  
          WHERE  LOWER(FirstName) = LOWER(@FirstName) AND LOWER(LastName)=LOWER(@LastName) AND DOB = @DOB  
			AND ( Email = @Email OR ContactNo = @ContactNo)  
			AND IsActive = 1;  
  
		IF @PatientId IS NULL  
		  BEGIN  
		INSERT INTO DAP_Patients  
		 (  
		  FirstName,  
		  LastName,  
		  Email,  
		  ContactNo,  
		  GenderId,  
		  BloodGroupId,  
		  DOB,  
		  AddressId,  
		  CreatedOn  
		 )  
		 VALUES   
		 (  
		  @FirstName,  
		  @LastName,  
		  @Email,  
		  @ContactNo,  
		  @GenderId,  
		  @BloodGroupId,  
		  @DOB,  
		  @AddressId,  
		  GETDATE()  
		 );  
  
		SET @PatientId=SCOPE_IDENTITY();  
	   END  
	 IF (@ProviderName IS NOT NULL AND LTRIM(RTRIM(@ProviderName)) <> '') AND (@PolicyNumber IS NOT NULL AND LTRIM(RTRIM(@PolicyNumber)) <> '')  
	  BEGIN  
	   SELECT @PatientInsuranceId = InsuranceId  
	   FROM DAP_PatientInsurancesInfo  
	   WHERE   
		PatientId = @PatientId  
		AND LOWER(ProviderName)= LOWER(@ProviderName)  
		AND LOWER(PolicyNumber) = LOWER(@PolicyNumber)  
		AND ValidTill=@ValidTill  
		AND IsActive = 1;  
  
    
   IF @PatientInsuranceId IS NULL  
   BEGIN  
    INSERT INTO DAP_PatientInsurancesInfo  
    (  
     PatientId,  
     ProviderName,  
     PolicyName,  
     PolicyNumber,  
     ValidTill,  
     CreatedOn  
    )  
    VALUES  
    (  
     @PatientId,  
     @ProviderName,  
     @PolicyName,  
     @PolicyNumber,  
     @ValidTill,  
     GETDATE()  
    );  
  
    SET @PatientInsuranceId = SCOPE_IDENTITY();  
   END  
  END  
   SELECT @AppointmentStatusId=AppointmentStatusId FROM DAP_AppointmentStatus WHERE ShortCode='PEND';  
  
   INSERT INTO DAP_Appointments  
    (  
     PatientId,  
     DoctorId,  
     ReasonForVisit,  
     AppointmentStatusId,  
     PatientInsuranceId,  
     MedicalHistory,  
     CreatedOn  
    )  
   VALUES   
    (  
     @PatientId,  
     @DoctorId,  
     @ReasonOfAppointment,  
     @AppointmentStatusId,  
     @PatientInsuranceId,  
     @MedicalHistory,  
     GETDATE()  
    );  
   SET @AppointmentId=SCOPE_IDENTITY();  
  
   INSERT INTO DAP_PreferredSlots  
    (  
     AppointmentId,  
     PreferredDate,  
     PreferredStartTime,  
     PreferredEndTime,  
     CreatedOn  
    )  
   SELECT   
    @AppointmentId, ps.PreferredDate,ps.PreferredStartTime,ps.PreferredEndTime,GETDATE()  
   FROM @preferredSlotsTVP ps  
  
   IF EXISTS (SELECT 1 FROM @proviousReportsTVP)  
     BEGIN  
    INSERT INTO DAP_PatientExistingReports  
     (  
      AppointmentId,  
      ReportName,  
      ReportFileName,  
      FileType,  
      CreatedOn  
     )  
    SELECT   
     @AppointmentId,  
     pr.ReportName,  
     pr.ReportFileName,  
     pr.FileType,  
     GETDATE()  
    FROM @proviousReportsTVP pr  
   END  
  
     SET @OutPatientId = @PatientId;  
        SET @OutAppointmentId = @AppointmentId;  
  
 COMMIT TRANSACTION  
  END TRY  
  
  BEGIN CATCH  
   IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;  
  END CATCH  
  
END;
GO

 CREATE OR ALTER   PROCEDURE dap_appointmentsGetAllOrByDoctorId   
/*  
-----------------------------------------------------------------------------------------------------  
DATE		 Created By      Purpose of creation  
13 OCT 2025  Lokesh Patil   To get all appointments info or based on Filters with pagination  
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
    
        LEFT JOIN DAP_PatientInsurancesInfo pii ON a.PatientInsuranceId = pii.InsuranceId  
    WHERE   
        a.IsActive = 1   
        AND p.IsActive = 1  
        AND (@DoctorId IS NULL OR a.DoctorId = @DoctorId)  
        AND (@AppointmentStatusId IS NULL OR a.AppointmentStatusId = @AppointmentStatusId)  
  AND ( @SpecializationId IS NULL  
    OR EXISTS (  
     SELECT 1  
     FROM DAP_DoctorSpecializations ds  
     WHERE ds.DoctorId = a.DoctorId   
       AND ds.SpecializationId = @SpecializationId  
       AND ds.IsActive = 1  
    )  
   )  
        AND (@PatientName IS NULL OR   
            p.FirstName LIKE '%' + @PatientName + '%' OR   
            p.LastName LIKE '%' + @PatientName + '%'  
        )  
        AND(@FromDate IS NULL AND @ToDate IS NULL  
   OR EXISTS (  
    SELECT 1  
    FROM DAP_PreferredSlots ps  
    WHERE ps.AppointmentId = a.AppointmentId  
     AND ps.IsActive = 1  
     AND (  
      (@FromDate IS NULL OR CAST(ps.PreferredDate AS DATE) >= @FromDate)  
     AND (@ToDate IS NULL OR CAST(ps.PreferredDate AS DATE) <= @ToDate)  
     )  
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
   AND (  
   (@AppointmentStatusShortCode = 'ACPT' AND ps.IsApproved = 1)  
   OR  
   (@AppointmentStatusShortCode = 'ALTS' AND ps.IsAlternateSlot = 1)  
   OR  
   (@AppointmentStatusShortCode IN ('PEND', 'REJT') OR @AppointmentStatusShortCode IS NULL)  
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

 CREATE OR ALTER   PROCEDURE dap_appointmentsGetById   
/*  
-----------------------------------------------------------------------------------------------------------------  
Date		 Created By		Purpose of creation  
13 OCT 2025  Lokesh Patil	To appointment details by appointment id  
------------------------------------------------------------------------------------------------------------------  
*/  
(  
    @AppointmentId INT  
)   
AS  
BEGIN  
    SET NOCOUNT ON;  
  
    SELECT   
        a.AppointmentId,  
        a.DoctorId,  
        d.FirstName AS DoctorFirstName,  
        d.LastName AS DoctorLastName,  
        p.PatientId,  
        p.FirstName AS PatientFirstName,  
        p.LastName AS PatientLastName,  
  p.GenderId AS GenderId,  
        g.Gender AS Gender,  
        p.DOB,  
  p.BloodGroupId AS BloodGroupId,  
        bg.BloodGroupName,  
        p.ContactNo,  
        p.Email,  
        addr.AddressLine1,  
        addr.Pincode,  
  addr.TalukaId AS TalukaId,  
        t.TalukaName,  
        a.ReasonForVisit,  
        ast.ShortCode AS AppointmentStatusShortCode,  
        pii.ProviderName AS InsuranceProvider,  
        pii.PolicyNumber,  
        pii.PolicyName,  
        pii.ValidTill AS InsuranceValidTill,  
        a.MedicalHistory  
    FROM DAP_Appointments a  
        INNER JOIN DAP_Patients p ON a.PatientId = p.PatientId  
        INNER JOIN DAP_Doctors d ON a.DoctorId = d.DoctorId  
        INNER JOIN DAP_Genders g ON p.GenderId = g.GenderId  
        INNER JOIN DAP_BloodGroups bg ON p.BloodGroupId = bg.BloodGroupId  
        INNER JOIN DAP_Addresses addr ON p.AddressId = addr.AddressId  
        INNER JOIN DAP_Talukas t ON addr.TalukaId = t.TalukaId  
        INNER JOIN DAP_AppointmentStatus ast ON a.AppointmentStatusId = ast.AppointmentStatusId  
        LEFT JOIN DAP_PatientInsurancesInfo pii ON a.PatientInsuranceId = pii.InsuranceId  
    WHERE a.IsActive = 1 AND p.IsActive = 1  
      AND a.AppointmentId = @AppointmentId;  
  
    SELECT   
        ps.PreferredSlotId,  
        ps.AppointmentId,  
        ps.PreferredDate,  
        ps.PreferredStartTime,  
        ps.PreferredEndTime,  
  ps.IsAlternateSlot,  
        ps.IsApproved  
    FROM DAP_PreferredSlots ps  
    WHERE ps.IsActive = 1 AND ps.AppointmentId = @AppointmentId  
    ORDER BY ps.PreferredDate, ps.PreferredStartTime;  
  
    SELECT   
        r.ReportId,  
        r.AppointmentId,  
        r.ReportName,  
        r.ReportFileName,  
        r.FileType  
    FROM DAP_PatientExistingReports r  
    WHERE r.IsActive = 1 AND r.AppointmentId = @AppointmentId;  
END
GO

CREATE OR ALTER   PROCEDURE dap_appointmentStatusGetList  
/*  
---------------------------------------------------------------------------------------  
Date		  Created By		Purpose of creation  
10 OCT 2025   Lokesh Patil		To get all appointment related status   
---------------------------------------------------------------------------------------  
*/  
AS  
BEGIN  
 SET NOCOUNT ON;  
  
 SELECT  
  AppointmentStatusId,  
  [Status],  
  ISNULL(ShortCode, '') AS ShortCode,  
  IsActive,  
  ISNULL(CreatedBy, 0) AS CreatedBy,  
  ISNULL(CreatedOn, '') AS CreatedOn,  
  ISNULL(ModifiedBy, 0) AS ModifiedBy,  
  ISNULL(ModifiedOn,'') AS ModifiedOn  
 FROM   
  DAP_AppointmentStatus  
 WHERE   
  IsActive = 1  
 ORDER BY   
  [Status];  
END;
GO

CREATE OR ALTER   PROCEDURE dap_appointmentUpdateStatus
/*
----------------------------------------------------------------------------------------------------------------------------
Date			Created By		Purpose of creation
13 OCT 2025		Lokesh Patil	To update the status of appointment after doctor accept/reject/alternate slot appointment
----------------------------------------------------------------------------------------------------------------------------
*/
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
GO

CREATE OR ALTER   PROCEDURE dap_appointmentAlternateSlotActionTokenInsert
/*
---------------------------------------------------------------------------------------------------
Date			Created By		Purpose of creation
16 OCT 2025		Lokesh Patil	To insert data into DAP_AppointmentAlternateSlotActionToken table
---------------------------------------------------------------------------------------------------
*/
    @Token NVARCHAR(100),
    @AppointmentId INT,
    @PreferredSlotId INT = NULL,
    @ExpiryDate DATETIME,
	@ActionTokenId INT OUT
AS
BEGIN
  

    INSERT INTO DAP_AppointmentAlternateSlotActionToken
    (Token, AppointmentId, PreferredSlotId, ExpiryDate)
    VALUES
    (@Token, @AppointmentId, @PreferredSlotId, @ExpiryDate);

	SET @ActionTokenId=SCOPE_IDENTITY() 
    
END;
GO

CREATE OR ALTER   PROCEDURE dap_appointmentAlternateSlotActionTokenLoad
/*
---------------------------------------------------------------------------------------------------
Date            Created By          Purpose of creation
03 NOV 2025     Lokesh Patil        To select data from DAP_AppointmentAlternateSlotActionToken table
---------------------------------------------------------------------------------------------------
*/
       
@Token NVARCHAR(100) = NULL 
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
		ActionTokenId,
        Token,
        AppointmentId,
        PreferredSlotId,
        ExpiryDate,
        IsUsed,
        CreatedOn
    FROM DAP_AppointmentAlternateSlotActionToken
    WHERE (@Token IS NULL OR Token = @Token) 
END;
GO

CREATE OR ALTER   PROCEDURE dap_appointmentAlternateSlotActionTokenUpdate
/*
---------------------------------------------------------------------------------------------------
Date			Created By		Purpose of creation
16 OCT 2025		Lokesh Patil	To update data into DAP_AppointmentAlternateSlotActionToken table
---------------------------------------------------------------------------------------------------
*/
    @ActionTokenId INT,
    @IsUsed BIT
AS
BEGIN
   

    UPDATE DAP_AppointmentAlternateSlotActionToken
    SET IsUsed = @IsUsed
    WHERE ActionTokenId = @ActionTokenId;

   
END;
GO

CREATE OR ALTER   PROCEDURE dap_bloodGroupsGetAll
/*    
******************************************************************************************************    
Date            Created By         Purpose Of Creation    
10Oct2025       Lokesh Patil       To Get All BloodGroups    
******************************************************************************************************    
*/ 
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        BloodGroupId,
        ISNULL(BloodGroupName, '') AS BloodGroupName,
        ISNULL(IsActive, 0) AS IsActive,
        CreatedOn
    FROM DAP_BloodGroups
    WHERE IsActive = 1
    ORDER BY BloodGroupName;
END
GO

CREATE OR ALTER   PROCEDURE dap_countriesGetAll
/*    
******************************************************************************************************    
Date            Created By         Purpose Of Creation    
10Oct2025       Lokesh Patil       To Get All Countries   
******************************************************************************************************    
*/ 
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        CountryId,
        ISNULL(CountryName, '') AS CountryName,
        ISNULL(IsActive, 0) AS IsActive,
        ISNULL(CreatedBy, 0) AS CreatedBy,
        CreatedOn,
        ISNULL(ModifiedBy, 0) AS ModifiedBy,
        ModifiedOn
    FROM DAP_Countries
    WHERE  IsActive = 1
    ORDER BY CountryName;
END
GO

CREATE OR ALTER   PROCEDURE dap_districtsGetAllOrByStateId
/*    
******************************************************************************************************    
Date            Created By         Purpose Of Creation    
10Oct2025       Lokesh Patil       To Get All Districts Or By State Id    
******************************************************************************************************    
*/ 

@StateId INT = NULL

AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        DistrictId,
        ISNULL(DistrictName, '') AS DistrictName,
        ISNULL(StateId, 0) AS StateId,
        ISNULL(IsActive, 0) AS IsActive,
        ISNULL(CreatedBy, 0) AS CreatedBy,
        CreatedOn,
        ISNULL(ModifiedBy, 0) AS ModifiedBy,
        ModifiedOn
    FROM DAP_Districts
    WHERE (@StateId IS NULL OR StateId = @StateId)
      AND IsActive = 1
    ORDER BY DistrictName;
END
GO

CREATE OR ALTER   PROC  dap_exceptionLogInsert
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

CREATE OR ALTER   PROCEDURE  dap_gendersGetAll
/*    
******************************************************************************************************    
Date            Created By         Purpose Of Creation    
10Oct2025       Lokesh Patil       To Get All Genders    
******************************************************************************************************    
*/ 
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        GenderId,
        ISNULL(Gender, '') AS Gender,
        ISNULL(IsActive, 0) AS IsActive,
		ISNULL(CreatedBy, 0) AS CreatedBy,
        CreatedOn,
		ISNULL(ModifiedBy, 0) AS ModifiedBy,
		ModifiedOn
    FROM DAP_Genders
    WHERE IsActive = 1
    ORDER BY Gender;
END
GO

CREATE OR ALTER PROC dap_perferredSlotIsApproved
/*
------------------------------------------------------------------------------
Date			Created By			Purpose of creation
12OCT2025		Lokesh Patil		To check requested slot is approved or not
------------------------------------------------------------------------------
*/
@PreferredSlotId INT,
@ApprovedSlotId INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE 
        @PreferredDate DATE,
        @StartTime TIME,
        @EndTime TIME;

    
    SELECT 
        @PreferredDate = ps.PreferredDate,
        @StartTime = ps.PreferredStartTime,
        @EndTime = ps.PreferredEndTime
    FROM DAP_PreferredSlots ps
    WHERE ps.PreferredSlotId = @PreferredSlotId;

   
    SELECT TOP 1 
        @ApprovedSlotId = ps2.PreferredSlotId
    FROM DAP_PreferredSlots ps2
    WHERE 
        ps2.PreferredDate = @PreferredDate
        AND ps2.PreferredStartTime = @StartTime
        AND ps2.PreferredEndTime = @EndTime
        AND ps2.IsApproved = 1;

    
    IF @ApprovedSlotId IS NULL
        SET @ApprovedSlotId = 0;
END;
GO

CREATE OR ALTER   PROCEDURE dap_qualificationsGetAll
/*    
******************************************************************************************************    
Date            Created By         Purpose Of Creation    
10Oct2025       Lokesh Patil       To Get All Qualifications    
******************************************************************************************************    
*/ 
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        QualificationId,
        ISNULL(Degree, '') AS Degree,
        ISNULL(DegreeDescription, '') AS DegreeDescription,
        ISNULL(IsActive, 0) AS IsActive,
        ISNULL(CreatedBy, 0) AS CreatedBy,
        CreatedOn,
        ISNULL(ModifiedBy, 0) AS ModifiedBy,
        ModifiedOn
    FROM DAP_Qualifications
    WHERE IsActive = 1
    ORDER BY Degree;
END
GO

CREATE OR ALTER   PROCEDURE dap_specializationsGetAll
/*    
******************************************************************************************************    
Date            Created By         Purpose Of Creation    
10Oct2025       Lokesh Patil       To Get All Specializations    
******************************************************************************************************    
*/    
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        SpecializationId,
        ISNULL(Specialization, '') AS Specialization,
        ISNULL([Description], '') AS [Description],
        ISNULL(IsActive, 0) AS IsActive,
        ISNULL(CreatedBy, 0) AS CreatedBy,
        CreatedOn,
        ISNULL(ModifiedBy, 0) AS ModifiedBy,
        ModifiedOn
    FROM DAP_Specializations
    WHERE IsActive = 1
    ORDER BY Specialization;
END
GO

CREATE OR ALTER   PROCEDURE dap_statesGetAllOrByCountryId
/*    
******************************************************************************************************    
Date            Created By         Purpose Of Creation    
10Oct2025       Lokesh Patil       To Get All Statas Or By Country Id    
******************************************************************************************************    
*/ 
@CountryId INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        StateId,
        ISNULL(StateName, '') AS StateName,
        ISNULL(CountryId, 0) AS CountryId,
        ISNULL(IsActive, 0) AS IsActive,
        ISNULL(CreatedBy, 0) AS CreatedBy,
        CreatedOn,
        ISNULL(ModifiedBy, 0) AS ModifiedBy,
        ModifiedOn
    FROM DAP_States
    WHERE (@CountryId IS NULL OR CountryId = @CountryId) AND IsActive = 1
    ORDER BY StateName;
END
GO

CREATE OR ALTER   PROCEDURE dap_talukasGetAllOrByDistrictId

/*    
******************************************************************************************************    
Date            Created By         Purpose Of Creation    
10Oct2025       Lokesh Patil       To Get All Talukas Or By District Id    
******************************************************************************************************    
*/ 

@DistrictId INT = NULL

AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        TalukaId,
        ISNULL(TalukaName, '') AS TalukaName,
        ISNULL(DistrictId, 0) AS DistrictId,
        ISNULL(IsActive, 0) AS IsActive,
        ISNULL(CreatedBy, 0) AS CreatedBy,
		CreatedOn,
        ISNULL(ModifiedBy, 0) AS ModifiedBy,
		ModifiedOn
    FROM DAP_Talukas
    WHERE (@DistrictId IS NULL OR DistrictId = @DistrictId)
      AND IsActive = 1
    ORDER BY TalukaName;
END
GO
