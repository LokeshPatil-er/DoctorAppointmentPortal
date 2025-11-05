using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAPClassLibrary.Helpers.Services;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace DAPClassLibrary
{
    public class DoctorsOps
    {
        public int DoctorId { get; set; }
        public int UserId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string RoleShortCode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string ContactNo { get; set; }
        public int BloodGroupId { get; set; }
        public string BloodGroupName { get; set; }
        public int GenderId { get; set; }
        public string GenderName { get; set; }
        public int AddressId { get; set; }
        public string AddressLine1 { get; set; }
        public string Pincode { get; set; }
        public DateTime ExperienceStartDate { get; set; }
        public int ConsultancyFee { get; set; }
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }

        public int SpecializationId { get; set; }


        private Database db;

        public DoctorsOps()
        {
            db = DatabaseFactory.CreateDatabase("constr");
        }

        public DoctorsOps(int doctorId)
        {
            db = DatabaseFactory.CreateDatabase("constr");
            this.DoctorId = doctorId;
        }

        public Doctors loadDoctor()
        {
            Doctors doctor = new Doctors();

            try
            {
                if(this.DoctorId <= 0)
                {
                    return new Doctors();
                }

                Database db = DatabaseFactory.CreateDatabase();

              
                DbCommand dbCommand = db.GetStoredProcCommand("dap_doctorGetById");
                db.AddInParameter(dbCommand, "@DoctorId", DbType.Int32, this.DoctorId);

                DataSet ds = db.ExecuteDataSet(dbCommand);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DataRow row = ds.Tables[0].Rows[0];

                    doctor = new Doctors
                    {
                        DoctorId = Convert.ToInt32(row["DoctorId"]),
                        UserId = Convert.ToInt32(row["UserId"]),
                        Email = Convert.ToString(row["Email"]),
                        RoleShortCode = Convert.ToString(row["RoleShortCode"]),
                        FirstName = Convert.ToString(row["FirstName"]),
                        LastName = Convert.ToString(row["LastName"]),
                        DateOfBirth = row["DOB"] != DBNull.Value ? Convert.ToDateTime(row["DOB"]) : DateTime.MinValue,
                        ContactNo = Convert.ToString(row["ContactNo"]),
                        ExperienceStartDate = Convert.ToDateTime(row["ExperienceStartDate"]),
                        ConsultancyFee = Convert.ToInt32(row["ConsultancyFee"]),
                        IsActive = Convert.ToBoolean(row["IsActive"]),
                        GenderId = Convert.ToInt32(row["GenderId"]),
                        GenderName = Convert.ToString(row["Gender"]),
                        BloodGroupId = Convert.ToInt32(row["BloodGroupId"]),
                        BloodGroupName = Convert.ToString(row["BloodGroupName"]),
                        AddressId = Convert.ToInt32(row["AddressId"]),
                        AddressLine1 = Convert.ToString(row["AddressLine1"]),
                        TalukaId = Convert.ToInt32(row["TalukaId"]),
                        DistrictId = Convert.ToInt32(row["DistrictId"]),
                        StateId = Convert.ToInt32(row["StateId"]),
                        CountryId = Convert.ToInt32(row["CountryId"]),
                        Pincode = Convert.ToString(row["Pincode"]),
                        CreatedBy = Convert.ToInt32(row["CreatedBy"]),
                        CreatedOn = row["CreatedOn"] != DBNull.Value ? Convert.ToDateTime(row["CreatedOn"]) : DateTime.MinValue,
                        ModifiedBy = row["ModifiedBy"] != DBNull.Value ? Convert.ToInt32(row["ModifiedBy"]) : 0,
                        ModifiedOn = row["ModifiedOn"] != DBNull.Value ? Convert.ToDateTime(row["ModifiedOn"]) : (DateTime?)null
                    };

                  

                    
                    if (ds.Tables.Count > 1)
                    {
                        doctor.DoctorSpecializationsIdList = ds.Tables[1]
                            .AsEnumerable()
                            .Where(s => Convert.ToInt32(s["DoctorId"]) == doctor.DoctorId)
                            .Select(s => Convert.ToInt32(s["SpecializationId"]))
                            .ToList();
                    }

                    
                    if (ds.Tables.Count > 2)
                    {
                        doctor.DoctorQulificationsIdList = ds.Tables[2]
                            .AsEnumerable()
                            .Where(q => Convert.ToInt32(q["DoctorId"]) == doctor.DoctorId)
                            .Select(q => Convert.ToInt32(q["QualificationId"]))
                            .ToList();
                    }

                    
                    if (ds.Tables.Count > 3)
                    {
                        doctor.DoctorAvailableSlots = ds.Tables[3]
                            .AsEnumerable()
                            .Where(sl => Convert.ToInt32(sl["DoctorId"]) == doctor.DoctorId)
                            .Select(sl => new DoctorAvailableSlots
                            {
                                AvaliableSlotId = Convert.ToInt32(sl["SlotId"]),
                                DayOfWeek = Convert.ToString(sl["DayOfWeek"]),
                                StartTime = Convert.ToString(sl["StartTime"]),
                                EndTime = Convert.ToString(sl["EndTime"]),
                                IsActive = Convert.ToBoolean(sl["IsActive"]),
                                CreatedBy = Convert.ToInt32(sl["CreatedBy"]),
                                CreatedOn = sl["CreatedOn"] != DBNull.Value ? Convert.ToDateTime(sl["CreatedOn"]) : DateTime.MinValue,
                                ModifiedBy = sl["ModifiedBy"] != DBNull.Value ? Convert.ToInt32(sl["ModifiedBy"]) : 0,
                                ModifiedOn = sl["ModifiedOn"] != DBNull.Value ? Convert.ToDateTime(sl["ModifiedOn"]) : DateTime.MinValue
                            }).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLogService.LogExceptionInDB(ex, nameof(DoctorsOps), nameof(loadDoctor));
                
                throw new Exception("Error while loading doctor details: " + ex.Message, ex);
            }

            return doctor;
        }


        public bool insertOrUpdateDoctors(Doctors doctorModel)
        {
            try
            {
                

                DbCommand dbCommand = db.GetStoredProcCommand("dap_doctorInsertOrUpdate");
                
                   
                    if (doctorModel.DoctorId > 0)
                        db.AddInParameter(dbCommand, "@DoctorId", DbType.Int32, doctorModel.DoctorId);
                    else
                        db.AddInParameter(dbCommand, "@DoctorId", DbType.Int32, DBNull.Value);

                    if (doctorModel.UserId > 0)
                        db.AddInParameter(dbCommand, "@UserId", DbType.Int32, doctorModel.UserId);
                    else
                        db.AddInParameter(dbCommand, "@UserId", DbType.Int32, DBNull.Value);

                    if (!string.IsNullOrWhiteSpace(doctorModel.RoleShortCode))
                        db.AddInParameter(dbCommand, "@RoleShortCode", DbType.String, doctorModel.RoleShortCode);
                    else
                        db.AddInParameter(dbCommand, "@RoleShortCode", DbType.String, DBNull.Value);

                    if (!string.IsNullOrWhiteSpace(doctorModel.Password))
                        db.AddInParameter(dbCommand, "@Password", DbType.String, doctorModel.Password);
                    else
                        db.AddInParameter(dbCommand, "@Password", DbType.String, DBNull.Value);

                    if (!string.IsNullOrWhiteSpace(doctorModel.Email))
                        db.AddInParameter(dbCommand, "@Email", DbType.String, doctorModel.Email);
                    else
                        db.AddInParameter(dbCommand, "@Email", DbType.String, DBNull.Value);

                  
                    if (!string.IsNullOrWhiteSpace(doctorModel.FirstName))
                        db.AddInParameter(dbCommand, "@FirstName", DbType.String, doctorModel.FirstName);
                    else
                        db.AddInParameter(dbCommand, "@FirstName", DbType.String, DBNull.Value);

                    if (!string.IsNullOrWhiteSpace(doctorModel.LastName))
                        db.AddInParameter(dbCommand, "@LastName", DbType.String, doctorModel.LastName);
                    else
                        db.AddInParameter(dbCommand, "@LastName", DbType.String, DBNull.Value);

                    if (doctorModel.DateOfBirth != default(DateTime))
                        db.AddInParameter(dbCommand, "@DOB", DbType.Date, doctorModel.DateOfBirth);
                    else
                        db.AddInParameter(dbCommand, "@DOB", DbType.Date, DBNull.Value);

                    if (!string.IsNullOrWhiteSpace(doctorModel.ContactNo))
                        db.AddInParameter(dbCommand, "@ContactNo", DbType.String, doctorModel.ContactNo);
                    else
                        db.AddInParameter(dbCommand, "@ContactNo", DbType.String, DBNull.Value);

                    if (doctorModel.BloodGroupId > 0)
                        db.AddInParameter(dbCommand, "@BloodGroupId", DbType.Int32, doctorModel.BloodGroupId);
                    else
                        db.AddInParameter(dbCommand, "@BloodGroupId", DbType.Int32, DBNull.Value);

                    if (doctorModel.GenderId > 0)
                        db.AddInParameter(dbCommand, "@GenderId", DbType.Int32, doctorModel.GenderId);
                    else
                        db.AddInParameter(dbCommand, "@GenderId", DbType.Int32, DBNull.Value);

                   
                    if (doctorModel.AddressId > 0)
                        db.AddInParameter(dbCommand, "@AddressId", DbType.Int32, doctorModel.AddressId);
                    else
                        db.AddInParameter(dbCommand, "@AddressId", DbType.Int32, DBNull.Value);

                    if (!string.IsNullOrWhiteSpace(doctorModel.AddressLine1))
                        db.AddInParameter(dbCommand, "@AddressLine1", DbType.String, doctorModel.AddressLine1);
                    else
                        db.AddInParameter(dbCommand, "@AddressLine1", DbType.String, DBNull.Value);

                    if (doctorModel.TalukaId > 0)
                        db.AddInParameter(dbCommand, "@TalukaId", DbType.Int32, doctorModel.TalukaId);
                    else
                        db.AddInParameter(dbCommand, "@TalukaId", DbType.Int32, DBNull.Value);

                    if (!string.IsNullOrWhiteSpace(doctorModel.Pincode))
                        db.AddInParameter(dbCommand, "@Pincode", DbType.String, doctorModel.Pincode);
                    else
                        db.AddInParameter(dbCommand, "@Pincode", DbType.String, DBNull.Value);

                   
                    if (doctorModel.ExperienceStartDate != default(DateTime))
                        db.AddInParameter(dbCommand, "@ExperienceStartDate", DbType.Date, doctorModel.ExperienceStartDate);
                    else
                        db.AddInParameter(dbCommand, "@ExperienceStartDate", DbType.Date, DBNull.Value);

                    if (doctorModel.ConsultancyFee > 0)
                        db.AddInParameter(dbCommand, "@ConsultancyFee", DbType.Int32, doctorModel.ConsultancyFee);
                    else
                        db.AddInParameter(dbCommand, "@ConsultancyFee", DbType.Int32, DBNull.Value);

                    if (doctorModel.CreatedBy > 0)
                        db.AddInParameter(dbCommand, "@CreatedBy", DbType.Int32, doctorModel.CreatedBy);
                    else
                        db.AddInParameter(dbCommand, "@CreatedBy", DbType.Int32, DBNull.Value);

                    if (doctorModel.ModifiedBy > 0)
                        db.AddInParameter(dbCommand, "@ModifiedBy", DbType.Int32, doctorModel.ModifiedBy);
                    else
                        db.AddInParameter(dbCommand, "@ModifiedBy", DbType.Int32, DBNull.Value);


                TVPTableService objTVPTableService = new TVPTableService();

                    DataTable availableSlotsTable = objTVPTableService.CreateAvailableSlotsDataTable(doctorModel.DoctorAvailableSlots);
                    db.AddParameter(dbCommand, "@AvailableSlotList", DbType.Object, ParameterDirection.Input,null, DataRowVersion.Current, availableSlotsTable);
                    ((SqlParameter)dbCommand.Parameters["@AvailableSlotList"]).SqlDbType = SqlDbType.Structured;
                    ((SqlParameter)dbCommand.Parameters["@AvailableSlotList"]).TypeName = "dbo.DAP_DoctorAvailableSlotsTVP";

                    DataTable specializationTable = objTVPTableService.IdsListToDataTable(doctorModel.DoctorSpecializationsIdList, "SpecializationId");
                    db.AddParameter(dbCommand, "@DoctorSpecializationList", DbType.Object, ParameterDirection.Input,null, DataRowVersion.Current, specializationTable);
                    ((SqlParameter)dbCommand.Parameters["@DoctorSpecializationList"]).SqlDbType = SqlDbType.Structured;
                    ((SqlParameter)dbCommand.Parameters["@DoctorSpecializationList"]).TypeName = "dbo.DAP_DoctorSpecializationsTVP";

                    DataTable qualificationTable = objTVPTableService.IdsListToDataTable(doctorModel.DoctorQulificationsIdList, "QualificationId");
                    db.AddParameter(dbCommand, "@DoctorQualificationList", DbType.Object, ParameterDirection.Input,null, DataRowVersion.Current, qualificationTable);
                    ((SqlParameter)dbCommand.Parameters["@DoctorQualificationList"]).SqlDbType = SqlDbType.Structured;
                    ((SqlParameter)dbCommand.Parameters["@DoctorQualificationList"]).TypeName = "dbo.DAP_DoctorQualificationsTVP";


                   
                    db.ExecuteNonQuery(dbCommand);
                    return true;
                
            }
            catch (Exception ex)
            {
                ExceptionLogService.LogExceptionInDB(ex, nameof(DoctorsOps), nameof(insertOrUpdateDoctors));
                
                throw new ApplicationException("Error adding doctor details.", ex);
               
            }
        }


        public DoctorTableList getDoctorsDetailsWithFilters(DoctorTableListFilter doctorFilter)
        {
            DoctorTableList result = new DoctorTableList();

            try
            {
                DbCommand dbCommand = db.GetStoredProcCommand("dap_doctorsGetAll");

                if (!string.IsNullOrWhiteSpace(doctorFilter.SearchDoctorName))
                    db.AddInParameter(dbCommand, "@SearchDoctorName", DbType.String, doctorFilter.SearchDoctorName);
                else
                    db.AddInParameter(dbCommand, "@SearchDoctorName", DbType.String, DBNull.Value);

                if (!string.IsNullOrWhiteSpace(doctorFilter.DoctorSpecializationIds))
                    db.AddInParameter(dbCommand, "@SpecializationIds", DbType.String, doctorFilter.DoctorSpecializationIds);
                else
                    db.AddInParameter(dbCommand, "@SpecializationIds", DbType.String, DBNull.Value);

                if (doctorFilter.PageNumber > 0)
                    db.AddInParameter(dbCommand, "@PageNumber", DbType.Int32, doctorFilter.PageNumber);
                else
                    db.AddInParameter(dbCommand, "@PageNumber", DbType.Int32, 1);

                if (doctorFilter.PageSize > 0)
                    db.AddInParameter(dbCommand, "@PageSize", DbType.Int32, doctorFilter.PageSize);
                else
                    db.AddInParameter(dbCommand, "@PageSize", DbType.Int32, DBNull.Value);

                DataSet ds = db.ExecuteDataSet(dbCommand);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        Doctors doctor = new Doctors
                        {
                            DoctorId = Convert.ToInt32(row["DoctorId"]),
                            UserId = Convert.ToInt32(row["UserId"]),
                            Email = Convert.ToString(row["Email"]),
                            RoleShortCode = Convert.ToString(row["RoleShortCode"]),
                            FirstName = Convert.ToString(row["FirstName"]),
                            LastName = Convert.ToString(row["LastName"]),
                            DateOfBirth = row["DOB"] != DBNull.Value ? Convert.ToDateTime(row["DOB"]) : DateTime.MinValue,
                            ContactNo = Convert.ToString(row["ContactNo"]),
                            ExperienceStartDate = Convert.ToDateTime(row["ExperienceStartDate"]),
                            ConsultancyFee = Convert.ToInt32(row["ConsultancyFee"]),
                            IsActive = Convert.ToBoolean(row["IsActive"]),
                            GenderName = Convert.ToString(row["Gender"]),
                            BloodGroupName = Convert.ToString(row["BloodGroupName"]),
                            AddressId = Convert.ToInt32(row["AddressId"]),
                            AddressLine1 = Convert.ToString(row["AddressLine1"]),
                            TalukaId = Convert.ToInt32(row["TalukaId"]),
                            Pincode = Convert.ToString(row["Pincode"]),
                            CreatedBy = Convert.ToInt32(row["CreatedBy"]),
                            CreatedOn = row["CreatedOn"] != DBNull.Value ? Convert.ToDateTime(row["CreatedOn"]) : DateTime.MinValue,
                            ModifiedBy = row["ModifiedBy"] != DBNull.Value ? Convert.ToInt32(row["ModifiedBy"]) : 0,
                            ModifiedOn = row["ModifiedOn"] != DBNull.Value ? Convert.ToDateTime(row["ModifiedOn"]) : (DateTime?)null
                        };

                        doctor.DoctorSpecializationsList = new List<Specializations>();
                        doctor.DoctorQualificationsList = new List<Qualifications>();
                        doctor.DoctorAvailableSlots = new List<DoctorAvailableSlots>();

                        if (ds.Tables.Count > 1)
                        {
                            foreach (DataRow sRow in ds.Tables[1].Select($"DoctorId = {doctor.DoctorId}"))
                            {
                                doctor.DoctorSpecializationsList.Add(new Specializations
                                {
                                    SpecializationId = Convert.ToInt32(sRow["SpecializationId"]),
                                    Specialization = Convert.ToString(sRow["Specialization"])
                                });
                            }
                        }

                        if (ds.Tables.Count > 2)
                        {
                            foreach (DataRow qRow in ds.Tables[2].Select($"DoctorId = {doctor.DoctorId}"))
                            {
                                doctor.DoctorQualificationsList.Add(new Qualifications
                                {
                                    QualificationId = Convert.ToInt32(qRow["QualificationId"]),
                                    Degree = Convert.ToString(qRow["Degree"])
                                });
                            }
                        }

                        if (ds.Tables.Count > 3)
                        {
                            foreach (DataRow slotRow in ds.Tables[3].Select($"DoctorId = {doctor.DoctorId}"))
                            {
                                doctor.DoctorAvailableSlots.Add(new DoctorAvailableSlots
                                {
                                    AvaliableSlotId = Convert.ToInt32(slotRow["SlotId"]),
                                    DayOfWeek = Convert.ToString(slotRow["DayOfWeek"]),
                                    StartTime = Convert.ToString(slotRow["StartTime"]),
                                    EndTime = Convert.ToString(slotRow["EndTime"]),
                                    IsAvailable = Convert.ToBoolean(slotRow["IsActive"]),
                                    CreatedBy = Convert.ToInt32(slotRow["CreatedBy"]),
                                    CreatedOn = slotRow["CreatedOn"] != DBNull.Value ? Convert.ToDateTime(slotRow["CreatedOn"]) : DateTime.MinValue,
                                    ModifiedBy = slotRow["ModifiedBy"] != DBNull.Value ? Convert.ToInt32(slotRow["ModifiedBy"]) : 0,
                                    ModifiedOn = slotRow["ModifiedOn"] != DBNull.Value ? Convert.ToDateTime(slotRow["ModifiedOn"]) : DateTime.MinValue
                                });
                            }
                        }

                        result.Doctors.Add(doctor);
                    }
                }

                if (ds.Tables.Count > 4 && ds.Tables[4].Rows.Count > 0)
                {
                    result.TotalRecords = Convert.ToInt32(ds.Tables[4].Rows[0]["TotalRecords"]);
                }
            }
            catch (Exception ex)
            {
                ExceptionLogService.LogExceptionInDB(ex, nameof(DoctorsOps), nameof(getDoctorsDetailsWithFilters));
                
                throw new Exception("Error while fetching doctor details: " + ex.Message, ex);
            }

            return result;
        }

        public bool deleteDoctorById()
        {
            try
            {
                if(this.DoctorId<=0 || this.ModifiedBy<=0)
                {
                    return false;
                }

                DbCommand dbCommand = db.GetStoredProcCommand("dap_doctorDeleteById");

               
                db.AddInParameter(dbCommand, "@DoctorId", DbType.Int32, this.DoctorId);
                db.AddInParameter(dbCommand, "@DeletedBy", DbType.Int32, this.ModifiedBy);

                int rowsAffected = db.ExecuteNonQuery(dbCommand);
                return rowsAffected > 0;
                
            }
            catch (Exception ex)
            {
                ExceptionLogService.LogExceptionInDB(ex, nameof(DoctorsOps), nameof(deleteDoctorById));
               
                throw new ApplicationException("Error deleting doctor record.", ex);
            }
        }

        public List<DoctorsDropDownList> getDoctorsListAllOrBySpecialization()
        {
            List<DoctorsDropDownList> doctorsLists = new List<DoctorsDropDownList>();
            try
            {
              

                DbCommand dbCommand = db.GetStoredProcCommand("dap_doctorsListAllOrBySpecialization");

                if(SpecializationId>0)
                {
                    db.AddInParameter(dbCommand,"@SpecializationId", DbType.Int32, this.SpecializationId);
                }
                else
                {
                    db.AddInParameter(dbCommand, "@SpecializationId", DbType.Int32, DBNull.Value);
                }

                

                DataSet ds=db.ExecuteDataSet(dbCommand);

                if(ds != null && ds.Tables[0].Rows.Count>0 )
                {
                    foreach(DataRow row  in ds.Tables[0].Rows)
                    {
                        doctorsLists.Add(new DoctorsDropDownList ()
                        {
                            DoctorId = Convert.ToInt32(row["DoctorId"]),
                            FullName=Convert.ToString(row["FullName"]),
                            DoctorQualifications = Convert.ToString(row["Qualifications"]),
                            ConsultancyFee = Convert.ToInt32(row["ConsultancyFee"])
                        });
                    }
                }

                return doctorsLists;
            }
            catch (Exception ex)
            {
                ExceptionLogService.LogExceptionInDB(ex, nameof(DoctorsOps), nameof(getDoctorsListAllOrBySpecialization));
               
                throw new ApplicationException("Error while feaching doctors based on specializations",ex);
            }
        }


       

    }
}
