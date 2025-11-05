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
    public class ExceptionsLogOps
    {
        public int ExceptionLogId { get; set; }

        public string ExceptionMessage { get; set; }

        public string ExceptionStackTrace { get; set; }

        public string InnerException { get; set; }

        public string Source { get; set; }

        public string TargetSite { get; set; }

        public int UserId { get; set; }  

        public DateTime LoggedOn { get; set; }

        public string AdditionalInfo { get; set; }

        public bool IsResolved { get; set; }

        private Database db;

        public ExceptionsLogOps()
        {
            this.db=DatabaseFactory.CreateDatabase("constr");
        }

        public ExceptionsLogOps(int exceptionLogId)
        {
            this.db = DatabaseFactory.CreateDatabase("constr");
            this.ExceptionLogId = exceptionLogId;
        }

        public bool exceptionsLogInsert(ExceptionsLog log)
        {
            try
            {
                if (log == null)
                    throw new ArgumentNullException(nameof(log));

                DbCommand dbCommand = this.db.GetStoredProcCommand("dap_exceptionLogInsert");

                this.db.AddInParameter(dbCommand, "@ExceptionMessage", DbType.String, log.ExceptionMessage);
              
                if (!string.IsNullOrEmpty(log.ExceptionStackTrace))
                {
                    this.db.AddInParameter(dbCommand, "@ExceptionStackTrace", DbType.String, log.ExceptionStackTrace);
                }
                else
                {
                    this.db.AddInParameter(dbCommand, "@ExceptionStackTrace", DbType.String, DBNull.Value);
                }

              
                if (!string.IsNullOrEmpty(log.InnerException))
                {
                    this.db.AddInParameter(dbCommand, "@InnerException", DbType.String, log.InnerException);
                }
                else
                {
                    this.db.AddInParameter(dbCommand, "@InnerException", DbType.String, DBNull.Value);
                }

               
                if (!string.IsNullOrEmpty(log.Source))
                {
                    this.db.AddInParameter(dbCommand, "@Source", DbType.String, log.Source);
                }
                else
                {
                    this.db.AddInParameter(dbCommand, "@Source", DbType.String, DBNull.Value);
                }

             
                if (!string.IsNullOrEmpty(log.TargetSite))
                {
                    this.db.AddInParameter(dbCommand, "@TargetSite", DbType.String, log.TargetSite);
                }
                else
                {
                    this.db.AddInParameter(dbCommand, "@TargetSite", DbType.String, DBNull.Value);
                }

             
                if (log.UserId > 0)
                {
                    this.db.AddInParameter(dbCommand, "@UserId", DbType.Int32, log.UserId);
                }
                else
                {
                    this.db.AddInParameter(dbCommand, "@UserId", DbType.Int32, DBNull.Value);
                }

              
                if (!string.IsNullOrEmpty(log.AdditionalInfo))
                {
                    this.db.AddInParameter(dbCommand, "@AdditionalInfo", DbType.String, log.AdditionalInfo);
                }
                else
                {
                    this.db.AddInParameter(dbCommand, "@AdditionalInfo", DbType.String, DBNull.Value);
                }

                int result = this.db.ExecuteNonQuery(dbCommand);
                return result > 0;
            }
            catch
            {
             
                return false;
            }
        }
    }
}
