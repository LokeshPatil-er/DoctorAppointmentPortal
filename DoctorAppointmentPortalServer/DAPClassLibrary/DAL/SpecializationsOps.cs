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
    public class SpecializationsOps
    {
        public int SpecializationId { get; set; }
        public string Specialization { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }

        private Database db;

        public SpecializationsOps() 
        {
            db = DatabaseFactory.CreateDatabase("constr");
        }

        public SpecializationsOps(int specializationId)
        {
            db = DatabaseFactory.CreateDatabase("constr");
            this.SpecializationId = specializationId;
        }

        public List<Specializations> GetSpecializationsList()
        {
            List<Specializations> specializationsList = new List<Specializations>();
            try
            {
               
                DbCommand dbCommand = this.db.GetStoredProcCommand("dap_specializationsGetAll");

                DataSet ds = this.db.ExecuteDataSet(dbCommand);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        Specializations specialization = new Specializations
                        {
                            SpecializationId = dr["SpecializationId"] != DBNull.Value ? Convert.ToInt32(dr["SpecializationId"]) : 0,
                            Specialization = dr["Specialization"] != DBNull.Value ? dr["Specialization"].ToString() : string.Empty,
                            Description = dr["Description"] != DBNull.Value ? dr["Description"].ToString() : string.Empty,
                            IsActive = dr["IsActive"] != DBNull.Value && Convert.ToBoolean(dr["IsActive"]),
                            CreatedBy = dr["CreatedBy"] != DBNull.Value ? Convert.ToInt32(dr["CreatedBy"]) : 0,
                            CreatedOn = dr["CreatedOn"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedOn"]) : DateTime.MinValue,
                            ModifiedBy = dr["ModifiedBy"] != DBNull.Value ? Convert.ToInt32(dr["ModifiedBy"]) : 0,
                            ModifiedOn = dr["ModifiedOn"] != DBNull.Value ? Convert.ToDateTime(dr["ModifiedOn"]) : DateTime.MinValue
                        };

                        specializationsList.Add(specialization);
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLogService.LogExceptionInDB(ex, nameof(SpecializationsOps), nameof(GetSpecializationsList));
            

                throw new Exception("Error occurred while fetching specializations list: " + ex.Message, ex);
            }

            return specializationsList;
        }


    }
}
