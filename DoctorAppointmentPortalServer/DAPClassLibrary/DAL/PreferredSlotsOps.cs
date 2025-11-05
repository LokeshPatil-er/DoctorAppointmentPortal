using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAPClassLibrary.Helpers.Services;

namespace DAPClassLibrary
{
    public class PreferredSlotsOps
    {
        public int PreferredSlotId { get; set; }
        public DateTime PreferredDate { get; set; }
        public string PreferredStartTime { get; set; }
        public string PreferredEndTime { get; set; }
        public bool IsApproved { get; set; }
        public bool IsActive { get; set; }

        private Database db;

        public PreferredSlotsOps()
        {
            this.db = DatabaseFactory.CreateDatabase("constr");
        }

        public PreferredSlotsOps(int preferredSlotId)
        {
            this.db = DatabaseFactory.CreateDatabase("constr");
            this.PreferredSlotId = preferredSlotId;
        }

        public int CheckPreferredSlotApproved()
        {
            int approvedId = 0;

            try
            {
                
                DbCommand dbCommand = db.GetStoredProcCommand("dap_perferredSlotIsApproved");

                
                db.AddInParameter(dbCommand, "@PreferredSlotId", DbType.Int32, this.PreferredSlotId);

               
                db.AddOutParameter(dbCommand, "@ApprovedSlotId", DbType.Int32, sizeof(int));

              
                db.ExecuteNonQuery(dbCommand);

                
                object result = db.GetParameterValue(dbCommand, "@ApprovedSlotId");
                if (result != DBNull.Value && result != null)
                {
                    approvedId = Convert.ToInt32(result);
                }
            }
            catch (Exception ex)
            {
                ExceptionLogService.LogExceptionInDB(ex, nameof(PreferredSlotsOps), nameof(CheckPreferredSlotApproved));
              
                throw new ApplicationException("Error occurred while checking approved preferred slot.", ex);
            }

            return approvedId;
        }

    }
}
