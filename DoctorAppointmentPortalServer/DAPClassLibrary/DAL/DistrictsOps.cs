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
    public class DistrictsOps
    {
        public int DistrictId { get; set; }
        public string DistrictName { get; set; }
        public int StateId { get; set; }
        public string StateName { get; set; }
        public bool IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }

        private Database db;
        public DistrictsOps()
        {
            db = DatabaseFactory.CreateDatabase("constr");
        }

        public DistrictsOps(int districtId)
        {
            db = DatabaseFactory.CreateDatabase("constr");
            this.DistrictId = districtId;
        }

        public List<Districts> GetDistrictsListAllOrByStateId()
        {
            List<Districts> districtsList = new List<Districts>();
            try
            {
               
                DbCommand dbCommand = this.db.GetStoredProcCommand("dap_districtsGetAllOrByStateId");
               
                if (this.StateId > 0)
                {
                    this.db.AddInParameter(dbCommand, "StateId", DbType.Int32, this.StateId);
                }
                else
                {
                    this.db.AddInParameter(dbCommand, "StateId", DbType.Int32, DBNull.Value);
                }

                DataSet ds = this.db.ExecuteDataSet(dbCommand);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        Districts district = new Districts
                        {
                            DistrictId = dr["DistrictId"] != DBNull.Value ? Convert.ToInt32(dr["DistrictId"]) : 0,
                            DistrictName = dr["DistrictName"] != DBNull.Value ? dr["DistrictName"].ToString() : string.Empty,
                            StateId = dr["StateId"] != DBNull.Value ? Convert.ToInt32(dr["StateId"]) : 0,
                            IsActive = dr["IsActive"] != DBNull.Value && Convert.ToBoolean(dr["IsActive"]),
                            CreatedBy = dr["CreatedBy"] != DBNull.Value ? Convert.ToInt32(dr["CreatedBy"]) : 0,
                            CreatedOn = dr["CreatedOn"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedOn"]) : DateTime.MinValue,
                            ModifiedBy = dr["ModifiedBy"] != DBNull.Value ? Convert.ToInt32(dr["ModifiedBy"]) : 0,
                            ModifiedOn = dr["ModifiedOn"] != DBNull.Value ? Convert.ToDateTime(dr["ModifiedOn"]) : DateTime.MinValue
                        };

                        districtsList.Add(district);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while fetching districts list: " + ex.Message, ex);
            }

            return districtsList;
        }

    }
}
