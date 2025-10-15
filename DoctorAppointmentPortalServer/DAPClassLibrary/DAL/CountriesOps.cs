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
    public class CountriesOps
    {
        public int CountryId { get; set; }
        public string CountryName { get; set; }
        public bool IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }

        private Database db;
        public CountriesOps() { 
            db= DatabaseFactory.CreateDatabase("constr");
        }

        public CountriesOps(int countryId)
        {
            db = DatabaseFactory.CreateDatabase("constr");
            this.CountryId = countryId;
        }

        public List<Countries> GetCountriesList()
        {
            List<Countries> countriesList = new List<Countries>();
            try
            {
                
                DbCommand dbCommand = this.db.GetStoredProcCommand("dap_countriesGetAll");

                
                DataSet ds = this.db.ExecuteDataSet(dbCommand);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        Countries country = new Countries
                        {
                            CountryId = dr["CountryId"] != DBNull.Value ? Convert.ToInt32(dr["CountryId"]) : 0,
                            CountryName = dr["CountryName"] != DBNull.Value ? dr["CountryName"].ToString() : string.Empty,
                            IsActive = dr["IsActive"] != DBNull.Value && Convert.ToBoolean(dr["IsActive"]),
                            CreatedBy = dr["CreatedBy"] != DBNull.Value ? Convert.ToInt32(dr["CreatedBy"]) : 0,
                            CreatedOn = dr["CreatedOn"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedOn"]) : DateTime.MinValue,
                            ModifiedBy = dr["ModifiedBy"] != DBNull.Value ? Convert.ToInt32(dr["ModifiedBy"]) : 0,
                            ModifiedOn = dr["ModifiedOn"] != DBNull.Value ? Convert.ToDateTime(dr["ModifiedOn"]) : DateTime.MinValue
                        };

                        countriesList.Add(country);
                    }
                }
            }
            catch (Exception ex)
            {
                
                throw new Exception("Error occurred while fetching countries list: " + ex.Message, ex);
            }

            return countriesList;
        }

    }
}
