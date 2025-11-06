 using DAPClassLibrary.Helpers.Services;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAPClassLibrary
{
    public class AppointmentsOps
    {
        public int AppointmentId { get; set; }
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public int AppointmentStatusId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string AppointmentStatus { get; set; }
        public bool IsApproved { get; set; }
        public bool IsAlternateSlot { get; set; }
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
        public int ModifiedBy { get; set; }

        private Database db;

        public AppointmentsOps()
        {
            db = DatabaseFactory.CreateDatabase("constr");
        }

        public AppointmentsOps(int appointmentId)
        {
            db = DatabaseFactory.CreateDatabase("constr");
            this.AppointmentId = appointmentId;
        }

        public PatientAppointmentRequest LoadAppointment()
        {
            PatientAppointmentRequest appointmentRequest = new PatientAppointmentRequest();

            try
            {
                if (this.AppointmentId <= 0)
                {
                    return new PatientAppointmentRequest();
                }

                Database db = DatabaseFactory.CreateDatabase();
                DbCommand dbCommand = db.GetStoredProcCommand("dap_appointmentsGetById");
                db.AddInParameter(dbCommand, "@AppointmentId", DbType.Int32, this.AppointmentId);

                DataSet ds = db.ExecuteDataSet(dbCommand);

               
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DataRow row = ds.Tables[0].Rows[0];

                    appointmentRequest = new PatientAppointmentRequest
                    {
                        Appointment = new Appointments
                        {
                            AppointmentId = Convert.ToInt32(row["AppointmentId"]),
                            DoctorId = Convert.ToInt32(row["DoctorId"]),
                            ReasonOfAppointment = Convert.ToString(row["ReasonForVisit"]),
                            AppointmentStatus = Convert.ToString(row["AppointmentStatusShortCode"]),
                            MedicalHistory = Convert.ToString(row["MedicalHistory"]),
                          
                        },
                        Patient = new Patients
                        {
                            PatientId = Convert.ToInt32(row["PatientId"]),
                            FirstName = Convert.ToString(row["PatientFirstName"]),
                            LastName = Convert.ToString(row["PatientLastName"]),
                            Gender = Convert.ToString(row["Gender"]),
                            DateOfBirth = row["DOB"] != DBNull.Value ? Convert.ToDateTime(row["DOB"]) : DateTime.MinValue,
                            BloodGroup = Convert.ToString(row["BloodGroupName"]),
                            ContactNo = Convert.ToString(row["ContactNo"]),
                            Email = Convert.ToString(row["Email"]),
                            AddressLine1 = Convert.ToString(row["AddressLine1"]),
                            Pincode = Convert.ToString(row["Pincode"]),
                            Taluka = Convert.ToString(row["TalukaName"]),
                            TalukaId = Convert.ToInt32(row["TalukaId"]),

                            ProviderName = row["InsuranceProvider"] != DBNull.Value ? Convert.ToString(row["InsuranceProvider"]) : string.Empty,
                            PolicyNumber = row["PolicyNumber"] != DBNull.Value ? Convert.ToString(row["PolicyNumber"]) : string.Empty,
                            PolicyName = row["PolicyName"] != DBNull.Value ? Convert.ToString(row["PolicyName"]) : string.Empty,
                            ValidTill = row["InsuranceValidTill"] != DBNull.Value ? Convert.ToDateTime(row["InsuranceValidTill"]) : (DateTime?)null
                        }
                    };
                }

               
                if (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
                {
                    appointmentRequest.Patient.PreferredSlotsList = ds.Tables[1]
                        .AsEnumerable()
                        .Select(sl => new PreferredSlots
                        {
                            PreferredSlotId = Convert.ToInt32(sl["PreferredSlotId"]),
             
                            PreferredDate = Convert.ToDateTime(sl["PreferredDate"]),
                            PreferredStartTime =Convert.ToString(sl["PreferredStartTime"]),
                            PreferredEndTime = Convert.ToString(sl["PreferredEndTime"]),
                            IsApproved = Convert.ToBoolean(sl["IsApproved"]),
                            IsAlternateSlot = Convert.ToBoolean(sl["IsAlternateSlot"]),
                        }).ToList();
                }

               
                if (ds.Tables.Count > 2 && ds.Tables[2].Rows.Count > 0)
                {
                    appointmentRequest.Patient.UploadReports = ds.Tables[2]
                        .AsEnumerable()
                        .Select(r => new ReportFiles
                        {
                            ReportId = Convert.ToInt32(r["ReportId"]),
                            AppointmentId = Convert.ToInt32(r["AppointmentId"]),
                            ReportName = Convert.ToString(r["ReportName"]),
                            ReportFileName = Convert.ToString(r["ReportFileName"]),
                            FileType = Convert.ToString(r["FileType"])
                        }).ToList();
                }

                return appointmentRequest;
            }
            catch (Exception ex)
            {
                ExceptionLogService.LogExceptionInDB(ex, nameof(AppointmentsOps), nameof(LoadAppointment));
                throw new Exception("Error while loading appointment details: " + ex.Message, ex);
            }
        }

        public DoctorAvailability GetDoctorAvailabilityAndAcceptedSlots()
        {
            DoctorAvailability response = new DoctorAvailability();
            try
            {
                if (db == null || this.DoctorId <= 0 || this.AppointmentDate == null)
                {
                    return response;
                }

                DbCommand dbCommand = db.GetStoredProcCommand("dap_doctorGetAvailabilityAndAcceptedSlots");
                db.AddInParameter(dbCommand, "@DoctorId", DbType.Int32, this.DoctorId);
                db.AddInParameter(dbCommand, "@RequestedDate", DbType.Date, this.AppointmentDate);

                DataSet ds = db.ExecuteDataSet(dbCommand);


                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        response.AvailableSlots.Add(new DoctorAvailableSlots()
                        {
                            AvaliableSlotId = Convert.ToInt32(row["SlotId"]),
                            DoctorId = Convert.ToInt32(row["DoctorId"]),
                            SlotDate = this.AppointmentDate,
                            DayOfWeek = Convert.ToString(row["DayOfWeek"]),
                            StartTime = Convert.ToString(row["StartTime"]),
                            EndTime = Convert.ToString(row["EndTime"]),
                            IsAvailable = Convert.ToBoolean(row["IsActive"])
                        });
                    }
                }


                if (ds != null && ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[1].Rows)
                    {
                        response.AcceptedAppointments.Add(new Appointments()
                        {
                            AppointmentId = Convert.ToInt32(row["AppointmentId"]),
                            DoctorId = Convert.ToInt32(row["DoctorId"]),
                            PatientId = Convert.ToInt32(row["PatientId"]),
                            ApprovedDate = Convert.ToDateTime(row["PreferredDate"]),
                            ApprovedStartTime = Convert.ToString(row["PreferredStartTime"]),
                            ApprovedEndTime = Convert.ToString(row["PreferredEndTime"]),
                            AppointmentStatusId = Convert.ToInt32(row["AppointmentStatusId"]),
                            AppointmentStatus = Convert.ToString(row["ShortCode"]),

                        });
                    }
                }

                return response;
            }
            catch (Exception ex)
            {
                ExceptionLogService.LogExceptionInDB(ex, nameof(AppointmentsOps), nameof(GetDoctorAvailabilityAndAcceptedSlots));
              
                throw new Exception("Error while getting doctor availability and accepted slots", ex);
            }
        }

        public bool InsertAppointment(PatientAppointmentRequest patientAppointmentRequest)
        {
            bool result = false;

            try
            {
                DbCommand dbCommand = db.GetStoredProcCommand("dap_appointmentInsert");
                
                 
                    db.AddInParameter(dbCommand, "@DoctorId", DbType.Int32, patientAppointmentRequest.Appointment.DoctorId);
                    db.AddInParameter(dbCommand, "@FirstName", DbType.String, patientAppointmentRequest.Patient.FirstName);
                    db.AddInParameter(dbCommand, "@LastName", DbType.String, patientAppointmentRequest.Patient.LastName);
                    db.AddInParameter(dbCommand, "@DOB", DbType.Date, patientAppointmentRequest.Patient.DateOfBirth);
                    db.AddInParameter(dbCommand, "@GenderId", DbType.Int32, patientAppointmentRequest.Patient.GenderId);
                    db.AddInParameter(dbCommand, "@BloodGroupId", DbType.Int32, patientAppointmentRequest.Patient.BloodGroupId);
                    db.AddInParameter(dbCommand, "@Email", DbType.String, patientAppointmentRequest.Patient.Email);
                    db.AddInParameter(dbCommand, "@ContactNo", DbType.String, patientAppointmentRequest.Patient.ContactNo);
                    db.AddInParameter(dbCommand, "@AddressLine1", DbType.String, patientAppointmentRequest.Patient.AddressLine1);
                    db.AddInParameter(dbCommand, "@TalukaId", DbType.Int32, patientAppointmentRequest.Patient.TalukaId);
                    db.AddInParameter(dbCommand, "@PinCode", DbType.String, patientAppointmentRequest.Patient.Pincode);

                  
                  
                    db.AddInParameter(dbCommand, "@ReasonOfAppointment", DbType.String, patientAppointmentRequest.Appointment.ReasonOfAppointment);

                    if(!string.IsNullOrEmpty(patientAppointmentRequest.Appointment.MedicalHistory))
                        db.AddInParameter(dbCommand, "@MedicalHistory", DbType.String, patientAppointmentRequest.Appointment.MedicalHistory);
                    else
                        db.AddInParameter(dbCommand, "@MedicalHistory", DbType.String, DBNull.Value);

              
                    if (!string.IsNullOrWhiteSpace(patientAppointmentRequest.Patient.ProviderName))
                        db.AddInParameter(dbCommand, "@ProviderName", DbType.String, patientAppointmentRequest.Patient.ProviderName.Trim());
                    else
                        db.AddInParameter(dbCommand, "@ProviderName", DbType.String, DBNull.Value);

                    if (!string.IsNullOrWhiteSpace(patientAppointmentRequest.Patient.PolicyNumber))
                        db.AddInParameter(dbCommand, "@PolicyNumber", DbType.String, patientAppointmentRequest.Patient.PolicyNumber.Trim());
                    else
                        db.AddInParameter(dbCommand, "@PolicyNumber", DbType.String, DBNull.Value);

                    if (!string.IsNullOrWhiteSpace(patientAppointmentRequest.Patient.PolicyName))
                        db.AddInParameter(dbCommand, "@PolicyName", DbType.String, patientAppointmentRequest.Patient.PolicyName.Trim());
                    else
                        db.AddInParameter(dbCommand, "@PolicyName", DbType.String, DBNull.Value);

                    if (patientAppointmentRequest.Patient.ValidTill != default(DateTime))
                        db.AddInParameter(dbCommand, "@ValidTill", DbType.Date, patientAppointmentRequest.Patient.ValidTill);
                    else
                        db.AddInParameter(dbCommand, "@ValidTill", DbType.Date, DBNull.Value);

                    TVPTableService objTVPTableService= new TVPTableService();

                    DataTable slotsTVP = objTVPTableService.ConvertPreferredSlotsToDataTable(patientAppointmentRequest.Patient.PreferredSlotsList);
                    db.AddParameter(dbCommand, "@preferredSlotsTVP", DbType.Object, ParameterDirection.Input, null, DataRowVersion.Current, slotsTVP);
                    ((SqlParameter)dbCommand.Parameters["@preferredSlotsTVP"]).SqlDbType = SqlDbType.Structured;
                    ((SqlParameter)dbCommand.Parameters["@preferredSlotsTVP"]).TypeName = "dbo.DAP_PreferredSlotsTVP";

                  
                    DataTable reportsTVP = objTVPTableService.ConvertReportsToDataTable(patientAppointmentRequest.Patient.UploadReports);
                    db.AddParameter(dbCommand, "@proviousReportsTVP", DbType.Object, ParameterDirection.Input, null, DataRowVersion.Current, reportsTVP);
                    ((SqlParameter)dbCommand.Parameters["@proviousReportsTVP"]).SqlDbType = SqlDbType.Structured;
                    ((SqlParameter)dbCommand.Parameters["@proviousReportsTVP"]).TypeName = "dbo.DAP_PatientReportsTVP";

                    db.AddOutParameter(dbCommand, "@OutPatientId", DbType.Int32, sizeof(int));
                    db.AddOutParameter(dbCommand, "@OutAppointmentId", DbType.Int32, sizeof(int));

               
                 db.ExecuteNonQuery(dbCommand);
                this.PatientId=Convert.ToInt32(db.GetParameterValue(dbCommand, "@OutPatientId"));
                this.AppointmentId = Convert.ToInt32(db.GetParameterValue(dbCommand, "@OutAppointmentId"));

                if (PatientId <= 0 || AppointmentId <= 0)
                    return false;

                result = true;

            }
            catch (Exception ex)
            {
                ExceptionLogService.LogExceptionInDB(ex, nameof(AppointmentsOps), nameof(InsertAppointment));
              
                throw new Exception("Error inserting appointment: " + ex.Message, ex);
            }

            return result;
        }

        public AppointmentsInfoList GetAppointmentsListWithFilter(AppointmentsListFilters appointmentFilters)
        {
            AppointmentsInfoList result = new AppointmentsInfoList
            {
                patientsAppointmentsList = new List<PatientAppointmentRequest>(),
                TotalRecored = 0
            };

            try
            {
                Database db = DatabaseFactory.CreateDatabase();
                DbCommand dbCommand = db.GetStoredProcCommand("dap_appointmentsGetAllOrByDoctorId");

               
                if (appointmentFilters.DoctorId > 0)
                    db.AddInParameter(dbCommand, "@DoctorId", DbType.Int32, appointmentFilters.DoctorId);
                else
                    db.AddInParameter(dbCommand, "@DoctorId", DbType.Int32, DBNull.Value);

                if(appointmentFilters.SpecializationId>0)
                    db.AddInParameter(dbCommand, "@SpecializationId", DbType.Int32, appointmentFilters.SpecializationId);
                else
                    db.AddInParameter(dbCommand, "@SpecializationId", DbType.Int32, DBNull.Value);

                if (!string.IsNullOrEmpty(appointmentFilters.PatientName))
                    db.AddInParameter(dbCommand, "@PatientName", DbType.String, appointmentFilters.PatientName);
                else
                    db.AddInParameter(dbCommand, "@PatientName", DbType.String, DBNull.Value);


               
                if (!string.IsNullOrEmpty(appointmentFilters.AppointmentStatus))
                    db.AddInParameter(dbCommand, "@AppointmentStatusShortCode", DbType.String, appointmentFilters.AppointmentStatus);
                else
                    db.AddInParameter(dbCommand, "@AppointmentStatusShortCode", DbType.String, DBNull.Value);

              
                if (appointmentFilters.FromDate != DateTime.MinValue)
                    db.AddInParameter(dbCommand, "@FromDate", DbType.Date, appointmentFilters.FromDate);
                else
                    db.AddInParameter(dbCommand, "@FromDate", DbType.Date, DBNull.Value);

               
                if (appointmentFilters.ToDate != DateTime.MinValue)
                    db.AddInParameter(dbCommand, "@ToDate", DbType.Date, appointmentFilters.ToDate);
                else
                    db.AddInParameter(dbCommand, "@ToDate", DbType.Date, DBNull.Value);

                
                if (appointmentFilters.PageNumber > 0)
                    db.AddInParameter(dbCommand, "@PageNumber", DbType.Int32, appointmentFilters.PageNumber);
                else
                    db.AddInParameter(dbCommand, "@PageNumber", DbType.Int32, 1);

               
                if (appointmentFilters.PageSize > 0)
                    db.AddInParameter(dbCommand, "@PageSize", DbType.Int32, appointmentFilters.PageSize);
                else
                    db.AddInParameter(dbCommand, "@PageSize", DbType.Int32, 10);

               
                DataSet ds = db.ExecuteDataSet(dbCommand);

                DataTable dtAppointments = ds.Tables[0];
                DataTable dtPreferredSlots = ds.Tables.Count > 1 ? ds.Tables[1] : new DataTable();
                DataTable dtReports = ds.Tables.Count > 2 ? ds.Tables[2] : new DataTable();
                DataTable dtDoctorSpecializations = ds.Tables.Count > 3 ? ds.Tables[3] : new DataTable();

                if (ds.Tables.Count > 4 && ds.Tables[4].Rows.Count > 0)
                {
                    result.TotalRecored = Convert.ToInt32(ds.Tables[4].Rows[0]["TotalRecords"]);
                }

                foreach (DataRow row in dtAppointments.Rows)
                {
                    var patient = new Patients
                    {
                        PatientId = Convert.ToInt32(row["PatientId"]),
                        FirstName = Convert.ToString(row["PatientFirstName"]),
                        LastName = Convert.ToString(row["PatientLastName"]),
                        DateOfBirth = Convert.ToDateTime(row["DOB"]),
                        ContactNo =Convert.ToString( row["ContactNo"]),
                        Gender = Convert.ToString(row["Gender"]),
                        Email = Convert.ToString(row["Email"]),
                        AddressLine1 = Convert.ToString(row["AddressLine1"]),
                        Pincode = Convert.ToString(row["Pincode"]),
                        ProviderName = row["InsuranceProvider"] == DBNull.Value ? null : Convert.ToString(row["InsuranceProvider"]),
                        PolicyNumber = row["PolicyNumber"] == DBNull.Value ? null : Convert.ToString(row["PolicyNumber"]),
                        PolicyName = row["PolicyName"] == DBNull.Value ? null : Convert.ToString(row["PolicyName"]),
                        ValidTill = row["InsuranceValidTill"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(row["InsuranceValidTill"]),
                        PreferredSlotsList = new List<PreferredSlots>(),
                        UploadReports = new List<ReportFiles>()
                    };

                    var appointment = new Appointments
                    {
                        AppointmentId = Convert.ToInt32(row["AppointmentId"]),
                        PatientId = Convert.ToInt32(row["PatientId"]),
                        DoctorId = Convert.ToInt32(row["DoctorId"]),
                        DoctorFirstName = Convert.ToString(row["DoctorFirstName"]),
                        DoctorLastName = Convert.ToString(row["DoctorLastName"]),
                        ReasonOfAppointment = Convert.ToString(row["ReasonForVisit"]),
                        MedicalHistory = row["MedicalHistory"] == DBNull.Value ? null : Convert.ToString(row["MedicalHistory"]),
                        AppointmentStatus = Convert.ToString(row["AppointmentStatusShortCode"]),
                        DoctorSpecializations = new List<string>()
                    };

                   
                    if (dtDoctorSpecializations.Rows.Count > 0)
                    {
                        var specializationRows = dtDoctorSpecializations.Select($"DoctorId = {appointment.DoctorId}");
                        foreach (var spec in specializationRows)
                        {
                            appointment.DoctorSpecializations.Add(spec["Specialization"].ToString());
                        }
                    }

                  
                    var slotRows = dtPreferredSlots.Select($"AppointmentId = {appointment.AppointmentId}");
                    foreach (var slot in slotRows)
                    {
                        patient.PreferredSlotsList.Add(new PreferredSlots
                        {
                            PreferredSlotId = Convert.ToInt32(slot["PreferredSlotId"]),
                            PreferredDate = Convert.ToDateTime(slot["PreferredDate"]),
                            PreferredStartTime = Convert.ToString(slot["PreferredStartTime"]),
                            PreferredEndTime = Convert.ToString(slot["PreferredEndTime"]),
                            IsApproved = Convert.ToBoolean(slot["IsApproved"]),
                            IsAlternateSlot = Convert.ToBoolean(slot["IsAlternateSlot"]),
                            IsActive = true
                        });
                    }

                  
                    var reportRows = dtReports.Select($"AppointmentId = {appointment.AppointmentId}");
                    foreach (var report in reportRows)
                    {
                        patient.UploadReports.Add(new ReportFiles
                        {
                            AppointmentId = Convert.ToInt32(report["AppointmentId"]),
                            ReportId = Convert.ToInt32(report["ReportId"]),
                            ReportName = Convert.ToString(report["ReportName"]),
                            ReportFileName = Convert.ToString(report["ReportFileName"]),
                            FileType = Convert.ToString(report["FileType"])
                        });
                    }

                    result.patientsAppointmentsList.Add(new PatientAppointmentRequest
                    {
                        Patient = patient,
                        Appointment = appointment
                    });
                }
            }
            catch (Exception ex)
            {
                ExceptionLogService.LogExceptionInDB(ex, nameof(AppointmentsOps), nameof(GetAppointmentsListWithFilter));
                
                throw new Exception("Error fetching appointments: " + ex.Message, ex);
            }

            return result;
        }


        public int UpdateAppointmentStatus(AppointmentStatusUpdate appointmentStatusUpdate)
        {
            try
            {
                DbCommand dbCommand = db.GetStoredProcCommand("dap_appointmentUpdateStatus");

                db.AddInParameter(dbCommand, "@AppointmentId", DbType.Int32, appointmentStatusUpdate.AppointmentId);
                db.AddInParameter(dbCommand, "@NewAppointmentStatus", DbType.String, appointmentStatusUpdate.NewAppointmentStatus);

                if(appointmentStatusUpdate.ModifiedBy>0)
                {
                  db.AddInParameter(dbCommand, "@ModifiedBy", DbType.Int32, appointmentStatusUpdate.ModifiedBy);
                }
                else
                {
                    db.AddInParameter(dbCommand, "@ModifiedBy", DbType.Int32, DBNull.Value);
                }

                if(appointmentStatusUpdate.PreferredSlotId>0)
                {
                    db.AddInParameter(dbCommand, "@PreferredSlotId", DbType.Int32, appointmentStatusUpdate.PreferredSlotId);
                }
                else
                {
                    db.AddInParameter(dbCommand, "@PreferredSlotId", DbType.Int32, DBNull.Value);
                }

                if (appointmentStatusUpdate.AlternateDate != DateTime.MinValue)
                {
                    db.AddInParameter(dbCommand, "@AlternateDate", DbType.Date, appointmentStatusUpdate.AlternateDate);
                }
                else
                {
                    db.AddInParameter(dbCommand, "@AlternateDate", DbType.Date, DBNull.Value);
                }

                if (!string.IsNullOrEmpty(appointmentStatusUpdate.AlternateStartTime))
                {
                    db.AddInParameter(dbCommand, "@AlternateStartTime", DbType.Time, appointmentStatusUpdate.AlternateStartTime);
                }
                else
                {
                    db.AddInParameter(dbCommand, "@AlternateStartTime", DbType.Time, DBNull.Value);
                }

                if (!string.IsNullOrEmpty(appointmentStatusUpdate.AlternateEndTime))
                {
                    db.AddInParameter(dbCommand, "@AlternateEndTime", DbType.Time, appointmentStatusUpdate.AlternateEndTime);
                }
                else
                {
                    db.AddInParameter(dbCommand, "@AlternateEndTime", DbType.Time, DBNull.Value);
                }

                return db.ExecuteNonQuery(dbCommand);
            }
            catch (Exception ex)
            {
          
                ExceptionLogService.LogExceptionInDB(ex, nameof(AppointmentsOps), nameof(UpdateAppointmentStatus));
                throw new ApplicationException("Error occurred while updating appointment status.", ex);
            }
        }







    }
}
