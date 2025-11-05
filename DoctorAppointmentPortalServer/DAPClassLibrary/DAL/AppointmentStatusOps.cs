using DAPClassLibrary.Helpers.Services;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAPClassLibrary
{
    public class AppointmentStatusOps
    {
        public int AppointmentStatusId { get; set; }
        public string Status { get; set; }
        public string ShortCode { get; set; }
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }

        private Database db;

        public AppointmentStatusOps()
        {
            this.db = DatabaseFactory.CreateDatabase("constr");
        }

        public AppointmentStatusOps(int appointmentStatusId)
        {
            this.db = DatabaseFactory.CreateDatabase("constr");
            this.AppointmentStatusId = appointmentStatusId;
        }

        public List<AppointmentStatus> GetAppointmentStatusesList()
        {
            List<AppointmentStatus> statusesList = new List<AppointmentStatus>();
            try
            {
                DbCommand dbCommand = db.GetStoredProcCommand("dap_appointmentStatusGetList");

                DataSet ds = db.ExecuteDataSet(dbCommand);

                if (ds !=null & ds.Tables[0].Rows.Count>0)
                {
                    foreach(DataRow row in ds.Tables[0].Rows)
                    {
                        statusesList.Add(new AppointmentStatus
                        {
                            AppointmentStatusId = Convert.ToInt32(row["AppointmentStatusId"]),
                            Status=Convert.ToString(row["status"]),
                            ShortCode = Convert.ToString(row["ShortCode"]),
                            IsActive = Convert.ToBoolean(row["IsActive"]),
                            CreatedBy = row["CreatedBy"] == DBNull.Value ? 0 : Convert.ToInt32(row["CreatedBy"]),
                            CreatedOn = row["CreatedOn"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(row["CreatedOn"]),
                            ModifiedBy = row["ModifiedBy"] == DBNull.Value ? 0 : Convert.ToInt32(row["ModifiedBy"]),
                            ModifiedOn = row["ModifiedOn"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(row["ModifiedOn"])
                        });
                    }
                }
            }
            catch(Exception ex)
            {
                ExceptionLogService.LogExceptionInDB(ex, nameof(AppointmentStatusOps), nameof(GetAppointmentStatusesList));
                throw new Exception("Error while getting appointment statuses", ex);

            }
            return statusesList;
        }
    }
}
