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
    public class QualificationsOps
    {

        public int QualificationId { get; set; }
        public string Degree { get; set; }
        public string DegreeDescription { get; set; }
        public bool IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }

        private Database db;

        public QualificationsOps()
        {
            db = DatabaseFactory.CreateDatabase("constr");
        }

        public QualificationsOps(int qualificationId)
        {
            db = DatabaseFactory.CreateDatabase("constr");
            this.QualificationId = qualificationId;
        }

        public List<Qualifications> GetQualificationsList()
        {
            List<Qualifications> qualificationsList = new List<Qualifications>();
            try
            {
                
                DbCommand dbCommand = this.db.GetStoredProcCommand("dap_qualificationsGetAll");
               
                DataSet ds = this.db.ExecuteDataSet(dbCommand);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        Qualifications qualification = new Qualifications
                        {
                            QualificationId = dr["QualificationId"] != DBNull.Value ? Convert.ToInt32(dr["QualificationId"]) : 0,
                            Degree = dr["Degree"] != DBNull.Value ? dr["Degree"].ToString() : string.Empty,
                            DegreeDescription = dr["DegreeDescription"] != DBNull.Value ? dr["DegreeDescription"].ToString() : string.Empty,
                            IsActive = dr["IsActive"] != DBNull.Value && Convert.ToBoolean(dr["IsActive"]),
                            CreatedBy = dr["CreatedBy"] != DBNull.Value ? Convert.ToInt32(dr["CreatedBy"]) : 0,
                            CreatedOn = dr["CreatedOn"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedOn"]) : DateTime.MinValue,
                            ModifiedBy = dr["ModifiedBy"] != DBNull.Value ? Convert.ToInt32(dr["ModifiedBy"]) : 0,
                            ModifiedOn = dr["ModifiedOn"] != DBNull.Value ? Convert.ToDateTime(dr["ModifiedOn"]) : DateTime.MinValue
                        };

                        qualificationsList.Add(qualification);
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLogService.LogExceptionInDB(ex, nameof(QualificationsOps), nameof(GetQualificationsList));
               
                throw new Exception("Error occurred while fetching qualifications list: " + ex.Message, ex);
            }

            return qualificationsList;
        }

    }
}
