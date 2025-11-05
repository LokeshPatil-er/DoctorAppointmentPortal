using DAPClassLibrary.Helpers.Services;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAPClassLibrary
{
    public class AppointmentAlternateSlotActionTokenOps
    {
        public int ActionTokenId { get; set; }
        public string Token { get; set; }
        public int AppointmentId { get; set; }
        public int PreferredSlotId { get; set; }
        public DateTime ExpiryDate { get; set; }
        public bool IsUsed { get; set; }
        public DateTime CreatedOn { get; set; }

        private Database db;

        public AppointmentAlternateSlotActionTokenOps()
        {
            this.db = DatabaseFactory.CreateDatabase("constr");
        }

        public AppointmentAlternateSlotActionTokenOps(int actionTokenId)
        {
            this.db = DatabaseFactory.CreateDatabase("constr");
            this.ActionTokenId = actionTokenId;
        }


        public void LoadToken()
        {
            try
            {
             
                if (string.IsNullOrEmpty(this.Token))
                    return;

                DbCommand cmd = db.GetStoredProcCommand("dap_appointmentAlternateSlotActionTokenLoad");
                db.AddInParameter(cmd, "@Token", DbType.String, this.Token);

                DataSet ds = db.ExecuteDataSet(cmd);

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DataRow row = ds.Tables[0].Rows[0];

                    this.ActionTokenId = Convert.ToInt32(row["ActionTokenId"]);
                    this.Token = row["Token"].ToString();
                    this.AppointmentId = Convert.ToInt32(row["AppointmentId"]);

                    if (row["PreferredSlotId"] != DBNull.Value)
                        this.PreferredSlotId = Convert.ToInt32(row["PreferredSlotId"]);
                    else
                        this.PreferredSlotId = 0; 

                    this.ExpiryDate = Convert.ToDateTime(row["ExpiryDate"]);
                    this.IsUsed = Convert.ToBoolean(row["IsUsed"]);
                    this.CreatedOn = Convert.ToDateTime(row["CreatedOn"]);
                }
             
            }
            catch (Exception ex)
            {
                ExceptionLogService.LogExceptionInDB(ex, nameof(AppointmentAlternateSlotActionTokenOps), nameof(LoadToken));
            }
        }
        public bool SaveData()
        {
            if(this.ActionTokenId==0)
            {
                return this.InsertToken();
            }
            else if(this.ActionTokenId>0)
            {
                return this.UpdateToken();
            }
            else
            {
                return false;
            }
        }

        private bool InsertToken()
        {
            try
            {
                DbCommand cmd = db.GetStoredProcCommand("dap_appointmentAlternateSlotActionTokenInsert");

                db.AddInParameter(cmd, "@Token", DbType.String, this.Token);
                db.AddInParameter(cmd, "@AppointmentId", DbType.Int32, this.AppointmentId);

               
                if (this.PreferredSlotId>0)
                {
                    db.AddInParameter(cmd, "@PreferredSlotId", DbType.Int32, this.PreferredSlotId);
                }
                else
                {
                    db.AddInParameter(cmd, "@PreferredSlotId", DbType.Int32, DBNull.Value);
                }

                db.AddInParameter(cmd, "@ExpiryDate", DbType.DateTime, this.ExpiryDate);

                db.AddOutParameter(cmd, "@ActionTokenId", DbType.Int32, sizeof(int));

                db.ExecuteNonQuery(cmd);

                return Convert.ToInt32(db.GetParameterValue(cmd, "@ActionTokenId"))>0;
            }
            catch (Exception ex)
            {
                ExceptionLogService.LogExceptionInDB(ex, nameof(AppointmentAlternateSlotActionTokenOps), nameof(InsertToken));
                return false;
            }
        }

        private bool UpdateToken()
        {
            try
            {
                DbCommand cmd = db.GetStoredProcCommand("dap_appointmentAlternateSlotActionTokenUpdate");
                db.AddInParameter(cmd, "@ActionTokenId", DbType.Int32, this.ActionTokenId);
                db.AddInParameter(cmd, "@IsUsed", DbType.Boolean, this.IsUsed);

                int rowsAffected = db.ExecuteNonQuery(cmd);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                ExceptionLogService.LogExceptionInDB(ex, nameof(AppointmentAlternateSlotActionTokenOps), nameof(UpdateToken));
               return false;
            }
        }
    }
}
