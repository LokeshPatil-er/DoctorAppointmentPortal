using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAPClassLibrary.Helpers.Services;

namespace DAPClassLibrary
{
    public class TVPTableService
    {
        public DataTable ConvertPreferredSlotsToDataTable(List<PreferredSlots> preferredSlots)
        {
            try
            {
                DataTable table = new DataTable();
                table.Columns.Add("PreferredDate", typeof(DateTime));
                table.Columns.Add("PreferredStartTime", typeof(TimeSpan));
                table.Columns.Add("PreferredEndTime", typeof(TimeSpan));

                if (preferredSlots != null)
                {
                    foreach (var slot in preferredSlots)
                    {
                        if (slot.PreferredDate != default &&
                            !string.IsNullOrEmpty(slot.PreferredStartTime) &&
                            !string.IsNullOrEmpty(slot.PreferredEndTime))
                        {
                            table.Rows.Add(slot.PreferredDate,
                                           TimeSpan.Parse(slot.PreferredStartTime),
                                           TimeSpan.Parse(slot.PreferredEndTime));
                        }
                    }
                }

                return table;
            }
            catch (Exception ex)
            {
                ExceptionLogService.LogExceptionInDB(ex, nameof(TVPTableService), nameof(ConvertPreferredSlotsToDataTable));
                throw;
            }
        }

        public DataTable ConvertReportsToDataTable(List<ReportFiles> reports)
        {
            try
            {
                DataTable table = new DataTable();
                table.Columns.Add("ReportName", typeof(string));
                table.Columns.Add("ReportFileName", typeof(string));
                table.Columns.Add("FileType", typeof(string));

                if (reports != null)
                {
                    foreach (var report in reports)
                    {
                        if (!string.IsNullOrWhiteSpace(report.ReportName) &&
                            !string.IsNullOrWhiteSpace(report.ReportFileName))
                        {
                            table.Rows.Add(report.ReportName, report.ReportFileName, report.FileType ?? "unknown");
                        }
                    }
                }

                return table;
            }
            catch (Exception ex)
            {
                ExceptionLogService.LogExceptionInDB(ex, nameof(TVPTableService), nameof(ConvertReportsToDataTable));
                throw;
            }
        }

        public DataTable IdsListToDataTable(List<int> idList, string columnName)
        {
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add(columnName, typeof(int));

                if (idList != null)
                {
                    foreach (int id in idList)
                        dt.Rows.Add(id);
                }

                return dt;
            }
            catch (Exception ex)
            {
                ExceptionLogService.LogExceptionInDB(ex, nameof(TVPTableService), nameof(IdsListToDataTable));
                throw;
            }
        }

        public DataTable CreateAvailableSlotsDataTable(List<DoctorAvailableSlots> slotList)
        {
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("SLotId", typeof(int));
                dt.Columns.Add("DayOfWeek", typeof(string));
                dt.Columns.Add("StartTime", typeof(string));
                dt.Columns.Add("EndTime", typeof(string));
                dt.Columns.Add("IsAvailable", typeof(bool));

                if (slotList != null)
                {
                    foreach (var slot in slotList)
                    {
                        dt.Rows.Add(slot.AvaliableSlotId, slot.DayOfWeek, slot.StartTime, slot.EndTime, slot.IsAvailable);
                    }
                }

                return dt;
            }
            catch (Exception ex)
            {
                ExceptionLogService.LogExceptionInDB(ex, nameof(TVPTableService), nameof(CreateAvailableSlotsDataTable));
                throw;
            }
        }
    }
}
