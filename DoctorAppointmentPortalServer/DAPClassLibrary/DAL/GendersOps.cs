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
    public class GendersOps
    {
        public int GenderId { get; set; }
        public string Gender { get; set; }
        public bool IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }

        private Database db;
        public GendersOps()
        {
            db = DatabaseFactory.CreateDatabase("constr");
        }

        public GendersOps(int genderId)
        {
            db = DatabaseFactory.CreateDatabase("constr");
            this.GenderId = genderId;
        }

        public List<Genders> GetGendersList()
        {
            List<Genders> gendersList = new List<Genders>();
            try
            {
                
                DbCommand dbCommand = this.db.GetStoredProcCommand("dap_gendersGetAll");

            
                DataSet ds = this.db.ExecuteDataSet(dbCommand);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        Genders gender = new Genders
                        {
                            GenderId = dr["GenderId"] != DBNull.Value ? Convert.ToInt32(dr["GenderId"]) : 0,
                            Gender = dr["Gender"] != DBNull.Value ? dr["Gender"].ToString() : string.Empty,
                            IsActive = dr["IsActive"] != DBNull.Value && Convert.ToBoolean(dr["IsActive"]),
                            CreatedBy = dr["CreatedBy"] != DBNull.Value ? Convert.ToInt32(dr["CreatedBy"]) : 0,
                            CreatedOn = dr["CreatedOn"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedOn"]) : DateTime.MinValue,
                            ModifiedBy = dr["ModifiedBy"] != DBNull.Value ? Convert.ToInt32(dr["ModifiedBy"]) : 0,
                            ModifiedOn = dr["ModifiedOn"] != DBNull.Value ? Convert.ToDateTime(dr["ModifiedOn"]) : DateTime.MinValue
                        };

                        gendersList.Add(gender);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while fetching genders list: " + ex.Message, ex);
            }

            return gendersList;
        }

    }
}
