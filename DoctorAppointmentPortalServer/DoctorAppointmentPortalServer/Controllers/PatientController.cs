using DAPClassLibrary;
using DAPClassLibrary.Helpers.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Data;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace DoctorAppointmentPortalServer.Controllers
{
    public class PatientController : ApiController
    {
        [HttpGet]
        public HttpResponseMessage DoctorsBySpecialization(int SpecializationId)
        {
            HttpResponseMessage response;
            try
            {
                if (SpecializationId <= 0)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new
                    {
                        Success = false,
                        Message = "Invalid specialization ID."
                    });
                }

                DoctorsOps objDoctorsOps = new DoctorsOps
                {
                    SpecializationId = SpecializationId
                };

                List<DoctorsDropDownList> doctorsList = objDoctorsOps.getDoctorsListAllOrBySpecialization();

                response = Request.CreateResponse(HttpStatusCode.OK, new
                {
                    Success = true,
                    Data = doctorsList ?? new List<DoctorsDropDownList>()
                });
            }
            catch (Exception ex)
            {
                ExceptionLogService.LogExceptionInDB(ex, nameof(PatientController), nameof(DoctorsBySpecialization));
                response = Request.CreateResponse(HttpStatusCode.InternalServerError, new
                {
                    Success = false,
                    Message = "Error occurred while fetching doctors.",
                    Details = ex.Message
                });
            }

            return response;
        }

        [HttpGet]
        public HttpResponseMessage GetAppointmentFormDropDowns()
        {
            HttpResponseMessage response;
            try
            {
                CountriesOps objCountriesOps = new CountriesOps();
                StatesOps objStatesOps = new StatesOps();
                DistrictsOps objDistrictsOps = new DistrictsOps();
                TalukasOps objTalukasOps = new TalukasOps();
                BloodGroupsOps objBloodGroupsOps = new BloodGroupsOps();
                GendersOps objGendersOps = new GendersOps();
                SpecializationsOps objSpecializationsOps = new SpecializationsOps();

                AppointmentFormDropDowns objAppointmentFormDropDowns = new AppointmentFormDropDowns
                {
                    CountriesList = objCountriesOps.GetCountriesList(),
                    StatesList = objStatesOps.GetStatesListAllOrByCountryId(),
                    DistrictsList = objDistrictsOps.GetDistrictsListAllOrByStateId(),
                    TalukasList = objTalukasOps.GetTalukasListAllOrByDistrictId(),
                    BloodGroupsList = objBloodGroupsOps.GetBloodGroupsList(),
                    GendersList = objGendersOps.GetGendersList(),
                    SpecializationsList = objSpecializationsOps.GetSpecializationsList()
                };

                response = Request.CreateResponse(HttpStatusCode.OK, new
                {
                    Success = true,
                    Data = objAppointmentFormDropDowns
                });
            }
            catch (Exception ex)
            {
                ExceptionLogService.LogExceptionInDB(ex, nameof(PatientController), nameof(GetAppointmentFormDropDowns));
                response = Request.CreateResponse(HttpStatusCode.InternalServerError, new
                {
                    Success = false,
                    Message = "Error occurred while fetching dropdown data.",
                    Details = ex.Message
                });
            }

            return response;
        }

        [HttpGet]
        public HttpResponseMessage GetAvailableSlotsAtPatient(int doctorId, string requestedDate)
        {
            HttpResponseMessage response;
            try
            {
                if (doctorId <= 0)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new
                    {
                        Success = false,
                        Message = "Invalid doctor ID."
                    });
                }

                AppointmentService appointmentService = new AppointmentService();
                var availableSlots = appointmentService.GetAvailableSlots(doctorId, requestedDate);

                response = Request.CreateResponse(HttpStatusCode.OK, new
                {
                    Success = true,
                    Data = availableSlots
                });
            }
            catch (Exception ex)
            {
                ExceptionLogService.LogExceptionInDB(ex, nameof(PatientController), nameof(GetAvailableSlotsAtPatient));
                response = Request.CreateResponse(HttpStatusCode.InternalServerError, new
                {
                    Success = false,
                    Message = "Error occurred while fetching available slots.",
                    Details = ex.Message
                });
            }

            return response;
        }

        [HttpPost]
        public async Task<HttpResponseMessage> InsertPatientAppointmentRequest()
        {
            HttpResponseMessage response;
            try
            {
                var httpRequest = HttpContext.Current.Request;
                string jsonModel = httpRequest.Form["PatientAppointmentRequest"];

                if (string.IsNullOrEmpty(jsonModel))
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new
                    {
                        Success = false,
                        Message = "Missing appointment data."
                    });
                }

                var patientAppointmentModel = JsonConvert.DeserializeObject<PatientAppointmentRequest>(jsonModel);

                List<ReportFiles> uploadedFilesInfo = new List<ReportFiles>();
                FileService fileService = new FileService();

                if (httpRequest.Files.Count > 0)
                {
                    uploadedFilesInfo = fileService.SaveFiles(httpRequest);

                    if (uploadedFilesInfo.Count != httpRequest.Files.Count)
                    {
                        return Request.CreateResponse(HttpStatusCode.InternalServerError, new
                        {
                            Success = false,
                            Message = "Some files could not be saved. Please try again."
                        });
                    }
                }

                patientAppointmentModel.Patient.UploadReports = uploadedFilesInfo;

                AppointmentsOps objAppointmentOps = new AppointmentsOps();
                bool isInserted = objAppointmentOps.InsertAppointment(patientAppointmentModel);

                if (!isInserted)
                {
                    fileService.ClearTempFolder();
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, new
                    {
                        Success = false,
                        Message = "Error while saving appointment data."
                    });
                }

                fileService.MoveTempFilesToFinalFolder(objAppointmentOps.AppointmentId);
                fileService.ClearTempFolder();

                try
                {
                    AppointmentEmailNotifier objAppointmentEmailNotifier = new AppointmentEmailNotifier();
                    patientAppointmentModel.Appointment.AppointmentId = objAppointmentOps.AppointmentId;

                    await objAppointmentEmailNotifier.NotifyAppointmentSubmittedAsync(patientAppointmentModel);
                }
                catch (Exception emailEx)
                {
                    ExceptionLogService.LogExceptionInDB(emailEx, nameof(PatientController), nameof(InsertPatientAppointmentRequest));
                   
                }

                response = Request.CreateResponse(HttpStatusCode.OK, new
                {
                    Success = true,
                    Message = "Appointment submitted successfully and notification sent.",
               
                });
            }
            catch (Exception ex)
            {
                ExceptionLogService.LogExceptionInDB(ex, nameof(PatientController), nameof(InsertPatientAppointmentRequest));
                response = Request.CreateResponse(HttpStatusCode.InternalServerError, new
                {
                    Success = false,
                    Message = "An unexpected error occurred while processing the appointment.",
                    Details = ex.Message
                });
            }

            return response;
        }


        [HttpGet]
        public async Task<HttpResponseMessage> AlternateSlotStatusUpdate(string token,string action)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(action))
                {
                    response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                    response.Content = new StringContent("Invalid token or action.");
                    return response;
                }

            
                AppointmentAlternateSlotActionTokenOps objAppointmentAlternateSlotActionTokenOps = new AppointmentAlternateSlotActionTokenOps{ Token = token };
                objAppointmentAlternateSlotActionTokenOps.LoadToken();

                if (objAppointmentAlternateSlotActionTokenOps.ActionTokenId == 0)
                {
                  return Request.CreateResponse(HttpStatusCode.NotFound, "Token not found or expired.");
                   
                   
                }

                if (objAppointmentAlternateSlotActionTokenOps.IsUsed)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "This token has already been used.");
                  
                }

            
                if (DateTime.Now > objAppointmentAlternateSlotActionTokenOps.ExpiryDate)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "This link has expired.");
              
                }

             
                bool isAccepted = AppointmentActions.AlternateSlotAccept.ToString() == action;
                bool isRejected = AppointmentActions.AlternateSlotReject.ToString() == action;

                if (!isAccepted && !isRejected)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid action.");
                   
                }


                AppointmentsOps objAppointmentsOps = new AppointmentsOps();

                AppointmentStatusUpdate appointmentStatusUpdate = new AppointmentStatusUpdate();
                appointmentStatusUpdate.AppointmentId = objAppointmentAlternateSlotActionTokenOps.AppointmentId;
                appointmentStatusUpdate.PreferredSlotId = objAppointmentAlternateSlotActionTokenOps.PreferredSlotId;

                appointmentStatusUpdate.NewAppointmentStatus = isAccepted ? AppointmentStatuses.ACCEPT : AppointmentStatuses.REJECT;


                int updated=objAppointmentsOps.UpdateAppointmentStatus(appointmentStatusUpdate);

                if (updated < 0)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, $"{action} is not completed try again or contact to doctor");
         
                }
 

                objAppointmentAlternateSlotActionTokenOps.IsUsed = true;
                objAppointmentAlternateSlotActionTokenOps.SaveData();

                AppointmentEmailNotifier appointmentEmailNotifier = new AppointmentEmailNotifier();
                await appointmentEmailNotifier.NotifyAlternateSlotResponseAsync(objAppointmentAlternateSlotActionTokenOps.AppointmentId, action);

                response=Request.CreateResponse(HttpStatusCode.OK, $"Appointment has been {(isAccepted ? "accepted" : "rejected")} successfully.");
            
               

            }
            catch (Exception ex)
            {
                ExceptionLogService.LogExceptionInDB(ex, nameof(PatientController), nameof(AlternateSlotStatusUpdate));
                response = Request.CreateResponse(HttpStatusCode.InternalServerError, "An error occurred while processing your request.");
                
            }

            return response;
        }



    }
}
