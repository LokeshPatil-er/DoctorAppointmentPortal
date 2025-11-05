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



INSERT INTO DAP_AppointmentStatus 
([Status], ShortCode, IsActive, CreatedBy, CreatedOn)
VALUES
('Pending', 'PEND', 1, NULL, GETDATE()),
('Accepted', 'ACPT', 1, NULL, GETDATE()),
('Rejected', 'REJT', 1, NULL, GETDATE()),
('Alternate Slot', 'ALTS', 1, NULL, GETDATE());



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

-- Table Type for Preferred Slots
CREATE TYPE DAP_PreferredSlotsTVP AS TABLE
(
    PreferredDate DATE NOT NULL,
    PreferredStartTime TIME NOT NULL,
    PreferredEndTime TIME NOT NULL
);

-- Table Type for Reports
CREATE TYPE DAP_PatientReportsTVP AS TABLE
(
    ReportName NVARCHAR(50),
    ReportFileName NVARCHAR(255),
    FileType NVARCHAR(50)
);


CREATE OR ALTER PROC dap_appointmentInsert
/*
------------------------------------------------------------------------------------------
Date		Create By		Purpose Of Creation
14OCT2025	Lokesh Patil	To insert  patient and appointment related info
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