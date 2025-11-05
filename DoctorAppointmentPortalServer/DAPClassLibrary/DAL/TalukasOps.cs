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
    public class TalukasOps
    {
        public int TalukaId { get; set; }
        public string TalukaName { get; set; }
        public int DistrictId { get; set; }
        public bool IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }

        private Database db;
        public TalukasOps()
        {
            db = DatabaseFactory.CreateDatabase("constr");
        }

        public TalukasOps(int talukaId)
        {
            db = DatabaseFactory.CreateDatabase("constr");
            this.TalukaId = talukaId;
        }


        public List<Talukas> GetTalukasListAllOrByDistrictId()
        {
            List<Talukas> talukasList = new List<Talukas>();
            try
            {
               
                DbCommand dbCommand = this.db.GetStoredProcCommand("dap_talukasGetAllOrByDistrictId");
               
                if (this.DistrictId > 0)
                {
                    this.db.AddInParameter(dbCommand, "DistrictId", DbType.Int32, this.DistrictId);
                }
                else
                {
                    this.db.AddInParameter(dbCommand, "DistrictId", DbType.Int32, DBNull.Value);
                }

                DataSet ds = this.db.ExecuteDataSet(dbCommand);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        Talukas taluka = new Talukas
                        {
                            TalukaId = dr["TalukaId"] != DBNull.Value ? Convert.ToInt32(dr["TalukaId"]) : 0,
                            TalukaName = dr["TalukaName"] != DBNull.Value ? dr["TalukaName"].ToString() : string.Empty,
                            DistrictId = dr["DistrictId"] != DBNull.Value ? Convert.ToInt32(dr["DistrictId"]) : 0,
                            IsActive = dr["IsActive"] != DBNull.Value && Convert.ToBoolean(dr["IsActive"]),
                            CreatedBy = dr["CreatedBy"] != DBNull.Value ? Convert.ToInt32(dr["CreatedBy"]) : 0,
                            CreatedOn = dr["CreatedOn"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedOn"]) : DateTime.MinValue,
                            ModifiedBy = dr["ModifiedBy"] != DBNull.Value ? Convert.ToInt32(dr["ModifiedBy"]) : 0,
                            ModifiedOn = dr["ModifiedOn"] != DBNull.Value ? Convert.ToDateTime(dr["ModifiedOn"]) : DateTime.MinValue
                        };

                        talukasList.Add(taluka);
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLogService.LogExceptionInDB(ex, nameof(TalukasOps), nameof(GetTalukasListAllOrByDistrictId));
             
                throw new Exception("Error occurred while fetching talukas list: " + ex.Message, ex);
            }

            return talukasList;
        }


    }
}
