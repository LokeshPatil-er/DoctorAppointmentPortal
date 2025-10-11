using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace DAPServerLibrary
{
    public class UsersOps
    {
        public int UserId { get; set; }
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
                    this.UserId = Convert.ToInt32(row["UserId"]);
                    this.Email = Convert.ToString(row["Email"]);
                    this.Password = Convert.ToString(row["Password"]);
                    this.RoleId = Convert.ToInt32(row["RoleId"]);
                    this.Role = Convert.ToString(row["Role"]);
                    this.RoleShortCode = Convert.ToString(row["ShortCode"]);
                    this.IsFirstLogin = Convert.ToBoolean(row["IsFirstLogin"]);
                    this.IsActive = Convert.ToBoolean(row["IsActive"]);
                    this.CreatedBy = Convert.ToInt32(row["CreatedBy"]);
                    this.CreatedOn = Convert.ToDateTime(row["CreatedOn"]);
                    this.ModifiedBy = Convert.ToInt32(row["ModifiedBy"]);
                    this.ModifiedOn = Convert.ToDateTime(row["ModifiedOn"]);

                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
           
        }
    }
}
