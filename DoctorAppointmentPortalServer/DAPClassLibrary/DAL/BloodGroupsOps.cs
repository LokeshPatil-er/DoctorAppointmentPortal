using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace DAPClassLibrary
{
    public class BloodGroupsOps
    {
        public int BloodGroupId { get; set; }
        public string BloodGroupName { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }

        private Database db;
        public BloodGroupsOps()
        {
            db = DatabaseFactory.CreateDatabase("constr");
        }

        public BloodGroupsOps(int bloodGroupId)
        {
            db = DatabaseFactory.CreateDatabase("constr");
            this.BloodGroupId = bloodGroupId;
        }

        public List<BloodGroups> GetBloodGroupsList()
        {
            List<BloodGroups> bloodGroupsList = new List<BloodGroups>();
            try
            {
               
                DbCommand dbCommand = this.db.GetStoredProcCommand("dap_bloodGroupsGetAll");

               
                DataSet ds = this.db.ExecuteDataSet(dbCommand);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        BloodGroups bloodGroup = new BloodGroups
                        {
                            BloodGroupId = dr["BloodGroupId"] != DBNull.Value ? Convert.ToInt32(dr["BloodGroupId"]) : 0,
                            BloodGroupName = dr["BloodGroupName"] != DBNull.Value ? dr["BloodGroupName"].ToString() : string.Empty,
                            IsActive = dr["IsActive"] != DBNull.Value && Convert.ToBoolean(dr["IsActive"]),
                            CreatedOn = dr["CreatedOn"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedOn"]) : DateTime.MinValue
                        };

                        bloodGroupsList.Add(bloodGroup);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while fetching blood groups list: " + ex.Message, ex);
            }

            return bloodGroupsList;
        }

    }
}
