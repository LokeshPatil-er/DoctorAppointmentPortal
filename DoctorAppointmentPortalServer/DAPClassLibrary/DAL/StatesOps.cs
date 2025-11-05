using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.EnterpriseLibrary.Data;
using DAPClassLibrary.Helpers.Services;

namespace DAPClassLibrary
{
    public class StatesOps
    {
        public int StateId { get; set; }
        public string StateName { get; set; }
        public int CountryId { get; set; }
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }

        private Database db;
        public StatesOps() 
        {
            db= DatabaseFactory.CreateDatabase("constr");
        }

        public StatesOps(int stateId)
        {
            db = DatabaseFactory.CreateDatabase("constr");
            this.StateId= stateId;
        }

        public List<States> GetStatesListAllOrByCountryId()
        {
            List<States> statesList = new List<States>();
            try
            {
               
                DbCommand dbCommand = this.db.GetStoredProcCommand("dap_statesGetAllOrByCountryId");
                if(this.CountryId>0)
                {
                    this.db.AddInParameter(dbCommand, "CountryId", DbType.Int32, this.CountryId);
                }
                else
                {
                    this.db.AddInParameter(dbCommand, "CountryId", DbType.Int32, DBNull.Value);
                }


                    DataSet ds = this.db.ExecuteDataSet(dbCommand);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        States state = new States
                        {
                            StateId = dr["StateId"] != DBNull.Value ? Convert.ToInt32(dr["StateId"]) : 0,
                            StateName = dr["StateName"] != DBNull.Value ? dr["StateName"].ToString() : string.Empty,
                            CountryId = dr["CountryId"] != DBNull.Value ? Convert.ToInt32(dr["CountryId"]) : 0,
                            IsActive = dr["IsActive"] != DBNull.Value && Convert.ToBoolean(dr["IsActive"]),
                            CreatedBy = dr["CreatedBy"] != DBNull.Value ? Convert.ToInt32(dr["CreatedBy"]) : 0,
                            CreatedOn = dr["CreatedOn"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedOn"]) : DateTime.MinValue,
                            ModifiedBy = dr["ModifiedBy"] != DBNull.Value ? Convert.ToInt32(dr["ModifiedBy"]) : 0,
                            ModifiedOn = dr["ModifiedOn"] != DBNull.Value ? Convert.ToDateTime(dr["ModifiedOn"]) : DateTime.MinValue
                        };

                        statesList.Add(state);
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLogService.LogExceptionInDB(ex, nameof(StatesOps), nameof(GetStatesListAllOrByCountryId));
                
                throw new Exception("Error occurred while fetching states list: " + ex.Message, ex);
            }

            return statesList;
        }

    }
}
