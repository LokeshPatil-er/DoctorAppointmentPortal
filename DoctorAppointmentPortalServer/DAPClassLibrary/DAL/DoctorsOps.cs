using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public int YearOfExperience { get; set; }
        public int ConsultancyFee { get; set; }
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }


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


        public bool InsertOrUpdateDoctors(Doctors doctorModel)
        {
            try
            {
                

                DbCommand dbCommand = db.GetStoredProcCommand("dap_doctorInsertOrUpdate");
                
                    //------------------------------------------------------
                    // DoctorId
                    //------------------------------------------------------
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

                    //------------------------------------------------------
                    // Personal Details
                    //------------------------------------------------------
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

                    //------------------------------------------------------
                    // Address
                    //------------------------------------------------------
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

                    //------------------------------------------------------
                    // Professional Info
                    //------------------------------------------------------
                    if (doctorModel.YearOfExperience > 0)
                        db.AddInParameter(dbCommand, "@YearOfExperience", DbType.Int32, doctorModel.YearOfExperience);
                    else
                        db.AddInParameter(dbCommand, "@YearOfExperience", DbType.Int32, DBNull.Value);

                    if (doctorModel.ConsultancyFee > 0)
                        db.AddInParameter(dbCommand, "@ConsultancyFee", DbType.Int32, doctorModel.ConsultancyFee);
                    else
                        db.AddInParameter(dbCommand, "@ConsultancyFee", DbType.Int32, DBNull.Value);

                    //------------------------------------------------------
                    // Audit Info
                    //------------------------------------------------------
                    if (doctorModel.CreatedBy > 0)
                        db.AddInParameter(dbCommand, "@CreatedBy", DbType.Int32, 1);
                    else
                        db.AddInParameter(dbCommand, "@CreatedBy", DbType.Int32, 1);

                    if (doctorModel.ModifiedBy > 0)
                        db.AddInParameter(dbCommand, "@ModifiedBy", DbType.Int32, 1);
                    else
                        db.AddInParameter(dbCommand, "@ModifiedBy", DbType.Int32, 1);

                    // Available Slots TVP
                    DataTable availableSlotsTable = CreateAvailableSlotsDataTable(doctorModel.DoctorAvailableSlots);
                    db.AddParameter(dbCommand, "@AvailableSlotList", DbType.Object, ParameterDirection.Input,null, DataRowVersion.Current, availableSlotsTable);
                    ((SqlParameter)dbCommand.Parameters["@AvailableSlotList"]).SqlDbType = SqlDbType.Structured;
                    ((SqlParameter)dbCommand.Parameters["@AvailableSlotList"]).TypeName = "dbo.DAP_DoctorAvailableSlotsTVP";

                    // Specializations TVP
                    DataTable specializationTable = ConvertListToDataTable(doctorModel.DoctorSpecializationsIdList, "SpecializationId");
                    db.AddParameter(dbCommand, "@DoctorSpecializationList", DbType.Object, ParameterDirection.Input,null, DataRowVersion.Current, specializationTable);
                    ((SqlParameter)dbCommand.Parameters["@DoctorSpecializationList"]).SqlDbType = SqlDbType.Structured;
                    ((SqlParameter)dbCommand.Parameters["@DoctorSpecializationList"]).TypeName = "dbo.DAP_DoctorSpecializationsTVP";

                    // Qualifications TVP
                    DataTable qualificationTable = ConvertListToDataTable(doctorModel.DoctorQulificationsIdList, "QualificationId");
                    db.AddParameter(dbCommand, "@DoctorQualificationList", DbType.Object, ParameterDirection.Input,null, DataRowVersion.Current, qualificationTable);
                    ((SqlParameter)dbCommand.Parameters["@DoctorQualificationList"]).SqlDbType = SqlDbType.Structured;
                    ((SqlParameter)dbCommand.Parameters["@DoctorQualificationList"]).TypeName = "dbo.DAP_DoctorQualificationsTVP";


                    //------------------------------------------------------
                    // Execute Stored Procedure
                    //------------------------------------------------------
                    db.ExecuteNonQuery(dbCommand);
                    return true;
                
            }
            catch (Exception ex)
            {
                // Optionally log exception details here
                return false;
            }
        }


        private DataTable ConvertListToDataTable(List<int> idList, string columnName)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(columnName, typeof(int));

            if (idList != null)
            {
                foreach (int id in idList)
                    dt.Rows.Add(id);
            }

            return dt;
        }

        private DataTable CreateAvailableSlotsDataTable(List<AvailableSlots> slotList)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("DayOfWeek", typeof(string));
            dt.Columns.Add("StartTime", typeof(string));
            dt.Columns.Add("EndTime", typeof(string));
            dt.Columns.Add("IsAvailable", typeof(bool));
            
            if (slotList != null)
            {
                foreach (var slot in slotList)
                {
                    dt.Rows.Add(slot.dayOfWeek, slot.StartTime, slot.EndTime, slot.IsAvailable);
                }
            }

            return dt;


        }
    }
}
