using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAPClassLibrary;
using DAPClassLibrary.Helpers.Services;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace DAPServerLibrary
{
    public class UsersOps
    {
        public int UserId { get; set; }
        public int DoctorId {  get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int RoleId { get; set; }
        public string Role { get; set; }
        public string RoleShortCode { get; set; }
        public bool IsFirstLogin { get; set; }
        public bool IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }

        private Database db= DatabaseFactory.CreateDatabase("constr");

        public UsersOps()
        {
            db = DatabaseFactory.CreateDatabase("constr");
        }

        public UsersOps(int userId)
        {
            db = DatabaseFactory.CreateDatabase("constr");
            this.UserId = UserId;
        }

        public bool LoadUser()
        {
            
            try
            {
                DbCommand dbCommand = this.db.GetStoredProcCommand("dap_usersLoadByEmail");
                this.db.AddInParameter(dbCommand, "email", DbType.String, this.Email);

                DataSet ds = this.db.ExecuteDataSet(dbCommand);

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DataRow row = ds.Tables[0].Rows[0];
                    this.UserId = row["UserId"] != DBNull.Value ? Convert.ToInt32(row["UserId"]) : 0;
                    this.DoctorId = row["DoctorId"] != DBNull.Value ? Convert.ToInt32(row["DoctorId"]) : 0;
                    this.Email = row["Email"] != DBNull.Value ? Convert.ToString(row["Email"]) : string.Empty;
                    this.Password = row["Password"] != DBNull.Value ? Convert.ToString(row["Password"]) : string.Empty;
                    this.RoleId = row["RoleId"] != DBNull.Value ? Convert.ToInt32(row["RoleId"]) : 0;
                    this.Role = row["Role"] != DBNull.Value ? Convert.ToString(row["Role"]) : string.Empty;
                    this.RoleShortCode = row["ShortCode"] != DBNull.Value ? Convert.ToString(row["ShortCode"]) : string.Empty;
                    this.IsFirstLogin = row["IsFirstLogin"] != DBNull.Value ? Convert.ToBoolean(row["IsFirstLogin"]) : false;
                    this.IsActive = row["IsActive"] != DBNull.Value ? Convert.ToBoolean(row["IsActive"]) : false;
                    this.CreatedBy = row["CreatedBy"] != DBNull.Value ? Convert.ToInt32(row["CreatedBy"]) : 0;
                    this.CreatedOn = row["CreatedOn"] != DBNull.Value ? Convert.ToDateTime(row["CreatedOn"]) : DateTime.MinValue;
                    this.ModifiedBy = row["ModifiedBy"] != DBNull.Value ? Convert.ToInt32(row["ModifiedBy"]) : 0;
                    this.ModifiedOn = row["ModifiedOn"] != DBNull.Value ? Convert.ToDateTime(row["ModifiedOn"]) : DateTime.MinValue;

                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                ExceptionLogService.LogExceptionInDB(ex, nameof(UsersOps), nameof(LoadUser));
            
                return false;
            }
           
        }

        public bool CheckEmailExists()
        {
            try
            {
                DbCommand dbCommand = this.db.GetStoredProcCommand("dap_usersCheckEmailExists");
                
                if(!string.IsNullOrEmpty(this.Email))
                {
                    this.db.AddInParameter(dbCommand, "@email", DbType.String, this.Email);
                }
                else
                {
                    this.db.AddInParameter(dbCommand, "@email", DbType.String, DBNull.Value);
                }

                if(this.UserId>0)
                {
                    this.db.AddInParameter(dbCommand, "@userId", DbType.Int32, this.UserId);
                }
                else
                {
                    this.db.AddInParameter(dbCommand, "@userId", DbType.Int32, DBNull.Value);
                }
                
                this.db.AddOutParameter(dbCommand, "@IsEmailExists", DbType.Boolean, 1);

               
                this.db.ExecuteNonQuery(dbCommand);

                
                bool isEmailExists = Convert.ToBoolean(this.db.GetParameterValue(dbCommand, "@IsEmailExists"));

                return isEmailExists;
            }
            catch (Exception ex)
            {
                ExceptionLogService.LogExceptionInDB(ex, nameof(UsersOps), nameof(CheckEmailExists));
                return false;
            }
        }

    }
}
